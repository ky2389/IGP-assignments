using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Singleton pattern
        if(instance==null)
        {
            instance=this;
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
