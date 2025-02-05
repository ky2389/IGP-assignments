using UnityEngine;
using TMPro;

public class manager : MonoBehaviour
{   
    [SerializeField]
    private GameObject coin_prefab;
    private cubeplayer player;
    private float timertotal=1.5f;
    private float timer=0;
    private int score;
    [SerializeField]
    private TextMeshProUGUI tX_score;
    public void AddScore(){
        score++;
        tX_score.text="Score:"+score;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tX_score.text="Score:"+score;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>timertotal){
            timer=0;
            GameObject coin= Instantiate(coin_prefab);
            coin.transform.position=new Vector3(Random.Range(-8.5f,8.5f),10,Random.Range(-8.5f,8.5f));
            coin.transform.rotation=Quaternion.Euler(new Vector3(Random.Range(0,360),Random.Range(0,360),Random.Range(0,360)));
        }
        else{
            timertotal+=Time.deltaTime;
        }
        if(Input.GetMouseButtonUp(0)){
            Vector2 touchUpPos=Input.mousePosition;
            Ray currentRay=Camera.main.ScreenPointToRay(touchUpPos);
            RaycastHit hit;
            if(Physics.Raycast(currentRay,out hit,3000,rayLayerMask_Floor)){
                player.Moveto(hit.point);
            }
        }
    }

}
