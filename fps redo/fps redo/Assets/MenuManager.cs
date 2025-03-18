using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
