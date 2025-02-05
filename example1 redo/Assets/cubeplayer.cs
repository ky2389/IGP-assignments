using UnityEngine;

public class cubeplayer : MonoBehaviour
{
    [SerializeField]
    private Vector3 startPos;
    private float movementTimer=0;
    [SerializeField]
    private GameManager manager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTarget=Vector3.up*0.5f;
    }
    public void Moveto(Vector3 newTarget){
        startPos=transform.position;
        currentTarget=newTarget+Vector3.up*0.5f;
        movementTimer=0;
    }
    // Update is called once per frame
    
    void Update()
    {
        Vector3 targetDistance =currentTarget-transform.position;
        targetDistance.y=0;
        Vector3 moveDirection=targetDistance.normalized;
        Vector3 newPos=Vector3.zero;
        if(movingPatterns==MovingPatterns.teleport){
            newPos=currentTarget;
        }
        else if(movingPatterns==MovingPatterns.Straight){
            Vector3 moveVector=moveDirection*10f*Time.deltaTime;
            if(moveVector.sqrMagnitude>targetDistance.sqrMagnitude){
                newPos=currentTarget;
            }
            else{

            }
        }
        transform.position=newPos;
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTage("Coin")){
            
        }
    }
}
