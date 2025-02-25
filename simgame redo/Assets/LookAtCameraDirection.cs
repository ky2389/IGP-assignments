using UnityEngine;

public class LookAtCameraDirection : MonoBehaviour
{ 
    Transform targetToLook;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetToLook=Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation=Quaternion.LookRotation(-targetToLook.forward,Vector3.up);
    }
}
