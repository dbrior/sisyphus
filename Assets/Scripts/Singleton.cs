using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool applicationIsQuitting = false;
    
    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Returning null.");
                return null;
            }

            if (_instance == null)
            {
                FindOrCreateSingleton();
            }

            return _instance;
        }
    }

    private static void FindOrCreateSingleton()
    {
        _instance = FindObjectOfType<T>();
        if (_instance == null)
        {
            GameObject singletonObject = new GameObject();
            _instance = singletonObject.AddComponent<T>();
            singletonObject.name = $"{typeof(T)} (Singleton)";
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
    }

    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Immediately revalidate or recreate the singleton instance
        if (_instance == null || _instance != this)
        {
            FindOrCreateSingleton();
        }
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
