using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
     
    #region Singleton
    private static void Init()
    {
        if (!initialized)
        {
            initialized = true;

            GameObject obj = GameObject.Find("@Main");
            if (obj == null)
            {
                obj = new() { name = @"Main" };
                obj.AddComponent<Main>();
                instance = obj.GetComponent<Main>();
            }
            instance = obj.GetComponent<Main>();
            DontDestroyOnLoad(obj);
        }
    }
    private static Main instance;
    private static bool initialized;
    public static Main Instance {
        get {
            Init();
            return instance;
        }
    }
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        Init();
        Game.Init();
    }
    #endregion

    private PoolManager _pool = new();
    private ResourceManager _resource = new();
    private ObjectManager _objects = new();
    private UIManager _ui = new();
    private DataManager _data = new();
    private SceneManagerEx _scene = new();
    private GameManager _game = new();
    private DungeonManager _dungeon = new();
    public static PoolManager Pool => Instance?._pool;
    public static ResourceManager Resource => Instance?._resource;
    public static ObjectManager Object => Instance?._objects;
    public static UIManager UI => Instance?._ui;
    public static SceneManagerEx Scene => Instance?._scene;
    public static GameManager Game => Instance?._game;
    public static DungeonManager Dungeon => Instance?._dungeon;
    public static DataManager Data => Instance?._data;

    //public static void Clear() {
    //    Audio.Clear();
    //    UI.Clear();
    //    Pool.Clear();
    //    Object.Clear();
    //}

    #region CoroutineHelper

    public new static Coroutine StartCoroutine(IEnumerator coroutine) => (Instance as MonoBehaviour).StartCoroutine(coroutine);
    public new static void StopCoroutine(Coroutine coroutine) => (Instance as MonoBehaviour).StopCoroutine(coroutine);

    #endregion

}
