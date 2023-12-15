using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager {
    //public Player Player { get; private set; }
    //public List<Enemy> Enemies { get; set; } = new();
    //private List<Projectile> Projectiles { get; set; } = new();

    public Transform EnemyParent {
        get {
            GameObject root = GameObject.Find("@Enemies");
            if (root == null) root = new("@Enemies");
            return root.transform;
        }
    }

    public T Spawn<T>(string key, Vector2 position) {
        
        // 유형에 따라 스폰: Main.Resource.Instnatiate("프리팹이름", pooling: true/false);
        // 스폰된 오브젝트 초기화 / 설정

        return default;
    }
    
    public void Despawn<T>(T obj) {

        // 유형에 따라 디스폰: Main.Resource.Destroy(obj.gameObject);
        // 디스폰 전 리스트 삭제 등 작업 필요.
    }
}

