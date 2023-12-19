using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class ResourceManager : MonoBehaviour {
    public bool Loaded { get; set; }
    private Dictionary<string, UnityEngine.Object> _resources = new();


    private void HandleCallback<T>(string key, AsyncOperationHandle<IList<T>> handle, Action<IList<T>> callback) where T : UnityEngine.Object
    {
        handle.Completed += operationHandle =>
        {
            IList<T> resultList = operationHandle.Result;
            // 리스트의 각 아이템을 _resources에 추가합니다.  

            for (int i = 0; i < resultList.Count; i++)
            {
                _resources.Add(resultList[i].name, resultList[i]);
            }
            callback?.Invoke(resultList);
        };
    }
    // key(주소)를 받아 비동기(Async) 로드
    public void LoadAsync<T>(string key, Action<T> callback = null)
        where T : UnityEngine.Object // T가 UnityEngine.Object에 속해야한다.
    {
        // key로 이미 로드된 리소스인지 확인 (중복 로드 방지)
        // 이미 로드되어있는 리소스면(key값이 이미 Dictionary에 있다면)
        // 다시 로드하지 않고 콜백 호출

        if (_resources.TryGetValue(key, out UnityEngine.Object resource)) {
            // callback이 null이 아닐경우에만 호출
            // 호출될때는 resource를 T로 형변환하여 콜백에 전달
            callback?.Invoke(resource as T);
            return;
        }

        // key를 받아, 실제로 어떤 리소스를 로드할 것인지 결정 
        string loadKey = key;

        

        // Sprite의 경우 key를 그대로 로드하면 Texture2D가 로드되므로,
        // Sprite 정보가 담긴 키값을 따로 로드해야 한다.
        // Texture2D 형식의 데이터에서 sprite를 뽑아내기 위해 key값 변경
        if (key.Contains(".sprite"))
            loadKey = $"{key}[{key.Replace(".sprite", "")}]";
        // ".sprite"를 ""로 치환해서 loadKey에 저장
        // ex) resource.sprite -> resource

        // 리소스 비동기 로드 시작
        if (key.Contains(".sprite")) {
            // Addressables.LoadAssetAsync<T> 을 통해
            // AsyncOperationHandle<T> 형식의 반환값을 asyncOperation에 할당
            var asyncOperation = Addressables.LoadAssetAsync<Sprite>(loadKey);

            // 비동기 작업이 완료될 때 호출되는 이벤트 핸들러를 설정
            // op 매개변수: 비동기 작업의 상태와 결과를 나타냄
            // Completed 이벤트: Action<AsyncOperationHandle<Sprite>> 이벤트
            // op.Result값: 로드한 에셋
            asyncOperation.Completed += op => {
                // Dictionary에 (key, op.Result) 추가
                _resources.Add(key, op.Result);
                // 콜백이 있다면 Invoke
                callback?.Invoke(op.Result as T);
            };
        }
        else if (key.Contains(".multiSprite"))
        {
                AsyncOperationHandle<IList<Sprite>> handle = Addressables.LoadAssetAsync<IList<Sprite>>(loadKey);
                HandleCallback<Sprite>(key, handle, objs => callback?.Invoke(objs as T));
        }
        else // sprite가 아닐경우
        {
            var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
            asyncOperation.Completed += op => {
                _resources.Add(key, op.Result);
                callback?.Invoke(op.Result as T);
            };
        }
    }

    // 해당 label을 가진 모든 리소스를 비동기 로드
    // 완료되면 콜백(key, 현재로드수, 전체로드수) 호출
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback)
        where T : UnityEngine.Object {
        // Label을 통해 에셋의 위치찾기
        // LoadResourceLocationsAsync를 통해 생성된 구조체의 Result에는
        // 해당 label을 가진 모든 에셋의 위치들이 들어있다.
        // label에 해당하는 리소스의 위치 정보를 로드
        // operation: AsyncOperationHandle<IList<IResourceLocation>> 타입의
        // 비동기 작업 핸들
        var operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));

        // op: AsyncOperationHandle<IList<IResourceLocation>> 타입
        operation.Completed += op => {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            // 각각의 위치 정보(result)에 따라
            foreach (IResourceLocation result in op.Result) {
                // result.PrimaryKey: 리소스 위치의 고유 키 -> 를 이용하여 리소스 로드
                LoadAsync<T>(result.PrimaryKey, obj => {
                    loadCount++;
                    // callback을 받아 loadCount / totalCount로 로딩중을 만들 수도 있다.
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };

        Loaded = true;
    }

    public T Load<T>(string key) where T : UnityEngine.Object {
        // key값으로 resources에 리소스가 존재하는지 확인
        if (!_resources.TryGetValue(key, out UnityEngine.Object resource))
            return null;

        // 존재한다면 resource를 T로 형변환해서 반환
        return resource as T;
    }

    public void Unload<T>(string key) where T : UnityEngine.Object {
        if (_resources.TryGetValue(key, out UnityEngine.Object resource)) {
            Addressables.Release(resource);
            _resources.Remove(key);
        }
        else
            Debug.LogError($"Resource Unload {key}");
    }

    // prefab
    //GameObject obj = Thing.Resource.InstantiatePrefab("name.prefab");
    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false) {
        GameObject prefab = Load<GameObject>(key);
        if (prefab == null) {
            Debug.LogError($"[ResourceManager] Instantiate({key}): Failed to load prefab.");
            return null;
        }

        if (pooling) return Main.Pool.Pop(prefab);

        GameObject obj = Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    // 해당 오브젝트를 풀에 돌려놓거나 파괴한다.
    public void Destroy(GameObject obj) {
        if (obj == null) return;

        if (Main.Pool.Push(obj)) return;

        UnityEngine.Object.Destroy(obj);
    }
}
