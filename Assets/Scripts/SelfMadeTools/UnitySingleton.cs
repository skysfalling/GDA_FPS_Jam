using UnityEngine;

public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    public static bool shouldNotDestroyOnLoad;

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<T>();
            if (shouldNotDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
            
        }
        else
        {
            if(Instance != GetComponent<T>())
            {

                //Destroy(this);
            }
        }
    }

    private void OnApplicationQuit()
    {
        Destroy(this);
    }

}
