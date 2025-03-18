using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{   
    public static SpawnManager instance;
    Bullet prefab_Bullet;
    [SerializeField]
    Transform bulletGroup;
    List<Bullet> bulletList = new List<Bullet>();
    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void SpawnBullets(Vector3 pos, Quaternion rot)
    {
        Bullet bullet = Instantiate(prefab_Bullet, bulletGroup);
        bullet.transform.position = pos;
        bullet.transform.rotation = rot;

        bulletList.Add(bullet);
    }
}
