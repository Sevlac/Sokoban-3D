using UnityEngine;

public abstract class Singleton <T>: MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static bool IsInstanced
    {
        get
        {
            return _instance != null;
        }
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log($"[Singleton] Instance <{typeof(T)}> does not exists, add the component in Scene directly.");
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    protected virtual void Awake()
    {
        if (!IsInstanced)
        {
            Instance = this as T;

            //Make persistent
            DontDestroyOnLoad(this);

            Debug.Log($"Singleton <{typeof(T)}> instanced.");
        }

        if (Instance != this)
        {
            Debug.Log($"Singleton <{typeof(T)}> already instanced, destroying {name}");
            Destroy(this);
        }
    }
}
