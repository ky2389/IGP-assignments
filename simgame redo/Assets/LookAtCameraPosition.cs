using UnityEngine;

public class LookAtCameraPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Transform targetToLook;
    void Start()
    {
        targetToLook=Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetToLook);
    }
}
