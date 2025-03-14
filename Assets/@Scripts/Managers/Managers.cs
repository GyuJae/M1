using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _sInstance;
    static Managers Instance
    {
        get
        {
            Init();
            return _sInstance;
        }
    }

    static void Init()
    {
        if (_sInstance == null)
        {
            var go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            // 초기화
            _sInstance = go.GetComponent<Managers>();
        }
    }

    #region Contents

    readonly ObjectManager obj = new();
    readonly GameManager game = new();

    public static ObjectManager Object
    {
        get { return Instance?.obj; }
    }
    public static GameManager Game
    {
        get { return Instance?.game; }
    }

    #endregion

    #region Core

    readonly DataManager data = new();
    readonly PoolManager pool = new();
    readonly ResourceManager resource = new();
    readonly SceneManagerEx scene = new();
    readonly SoundManager sound = new();
    readonly UIManager ui = new();

    public static DataManager Data
    {
        get { return Instance?.data; }
    }
    public static PoolManager Pool
    {
        get { return Instance?.pool; }
    }
    public static ResourceManager Resource
    {
        get { return Instance?.resource; }
    }
    public static SceneManagerEx Scene
    {
        get { return Instance?.scene; }
    }
    public static SoundManager Sound
    {
        get { return Instance?.sound; }
    }
    public static UIManager UI
    {
        get { return Instance?.ui; }
    }

    #endregion
}
