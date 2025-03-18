using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
public class GateController : MonoBehaviour, IMover
{   
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }
    [SerializeField]
    float gateSpeed=-10;
    [SerializeField]
    TextMeshPro[] tx_Gates;

    bool isGateTriggered=false;
    GameManager.Ability[] gateAbility;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //generate two gate's power
        gateAbility=new GameManager.Ability[]
        {
            new GameManager.Ability(Random.Range(0,4),Random.Range(1,10)),
            new GameManager.Ability(Random.Range(0,4),Random.Range(1,10)),
        };
        //display the power
        for(int i=0;i<tx_Gates.Length;i++)
        {
            tx_Gates[i].text=GameManager.instance.abilityNames[gateAbility[i].abilityID]+gateAbility[i].abilityPower;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(Position.z<-20)
        {
            Remove();
        }
    }

    public void Move()
    {
        transform.Translate(Vector3.forward*gateSpeed*Time.deltaTime);
    }
    public void Remove()
    {
        Destroy(gameObject);
    }
    public void GateTriggered(int gateID)
    {
        if(!isGateTriggered)
        {
            isGateTriggered=true;
            GameManager.instance.ApplyGateAbility(gateAbility[gateID]);
        }
    }
}
