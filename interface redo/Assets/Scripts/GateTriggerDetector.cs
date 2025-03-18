using UnityEngine;

public class GateTriggerDetector : MonoBehaviour
{
    [SerializeField]
    int gateID=0;
    [SerializeField]
    GateController gateController;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gateController.GateTriggered(gateID);
        }
    }
}
