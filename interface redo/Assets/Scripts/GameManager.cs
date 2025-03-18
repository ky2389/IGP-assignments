using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public struct Ability
    {
        public int abilityID;
        public int abilityPower;
        public Ability(int _abilityID, int _abilityPower)
        {
            abilityID = _abilityID;
            abilityPower = _abilityPower;
        }
    }
    [SerializeField]
    TextMeshPro tx_PlayerNum;
    [SerializeField]
    GateController gate_Prefab;
    [SerializeField]
    PlayerUnit playerUnit_Prefab;
    [SerializeField]
    Transform playerGroup;

    public Transform playerTarget;
    float speed=10;
    [HideInInspector]
    public string[] abilityNames= { "+" , "-" , "x" , "÷" };

    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    float gateTimer=0;
    float gateTimerTotal=1;

    int _playerNum;
    public int playerNum
    {
        get { return _playerNum; }
        set
        {
            _playerNum = Mathf.Clamp(value,0,500);
        }
    }
    //only show up to 100 players but can go up to 500 actually
    int playerVisualMaxNum=100;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else //if (instance != this)
        {
            Destroy(gameObject);
        }
        // DontDestroyOnLoad(gameObject);
    }  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerNum=0;
        CreateUnit(1);
    }

    void CreateUnit(int num)
    {
        playerNum+=num;
        var increasedNum=Mathf.Clamp(num,0,playerVisualMaxNum-playerUnitList.Count);
        for (int i = 0; i < increasedNum; i++)
        {
            PlayerUnit unit=Instantiate(playerUnit_Prefab,playerGroup);
            unit.Position=playerTarget.position+new Vector3(Random.value,Random.value,Random.value);
            playerUnitList.Add(unit);
        }

    }

    // Update is called once per frame
    void Update()
    {
        var horizontal=Input.GetAxis("Horizontal");
        playerTarget.Translate(Vector3.right*horizontal*speed*Time.deltaTime);
        if(playerTarget.position.x>5)
        {
            playerTarget.position=new Vector3(5,playerTarget.position.y,playerTarget.position.z);
        }
        else if(playerTarget.position.x<-5)
        {
            playerTarget.position=new Vector3(-5,playerTarget.position.y,playerTarget.position.z);
        }
        if(gateTimer>gateTimerTotal)
        {
            gateTimer = 0;
            gateTimerTotal = 1.0f +Random.Range(0, 3);
            GateController gate=Instantiate(gate_Prefab);
        }
        else
        {
            gateTimer += Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        tx_PlayerNum.text=playerNum.ToString();
    }
    public void ApplyGateAbility(Ability ability)
    {
        if(ability.abilityID==0)
        {
            CreateUnit(ability.abilityPower);
        }
        else if(ability.abilityID==1)
        {
            int decreasedNum=CalculateDecreasePlayerNum(playerNum-ability.abilityPower);
            if(playerUnitList.Count>0)
            {
                for (int i = 0; i < decreasedNum; i++)
                {
                    RemoveUnit(playerUnitList[Random.Range(0,playerUnitList.Count)]);
                    
                }
            }
        }
        else if(ability.abilityID==2)
        {
            CreateUnit(playerNum*ability.abilityPower-1);
        }
        else if(ability.abilityID==3)
        {
            int decreasedNum=CalculateDecreasePlayerNum(Mathf.RoundToInt((float)playerNum/ability.abilityPower));
            if(playerUnitList.Count>0)
            {
                for (int i = 0; i < decreasedNum; i++)
                {
                    RemoveUnit(playerUnitList[Random.Range(0,playerUnitList.Count)]);
                }
            }
        }
    }
    int CalculateDecreasePlayerNum(int formulaResult)
    {
        int decreasedNum=0;
        if (playerNum > playerVisualMaxNum)
        {
            if (formulaResult < playerVisualMaxNum)
            {
                decreasedNum = playerVisualMaxNum - formulaResult;
            }
        }
        else
        {
            decreasedNum=Mathf.Min(playerNum,playerNum-formulaResult);
        }
        playerNum=formulaResult;
        return decreasedNum;
    }  
    void RemoveUnit(PlayerUnit unit)
    {
        playerUnitList.Remove(unit);
        Destroy(unit.gameObject);
    }
}
