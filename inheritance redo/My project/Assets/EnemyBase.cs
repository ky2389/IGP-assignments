using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected Transform target;
    protected float hp=0;
    [SerializeField]
    protected float hpTotal=100;
    private float timer=0f;
    [SerializeField]
    private float timerTotal=1f;
    protected NavMeshAgent nav;
    void Start()
    {
        hp=hpTotal;
        target=GameManager.instance.player;
        nav=GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        TimerTool();
    }
    //General Function for timer Check
    private void TimerTool()
    {
        timer+=Time.deltaTime;
        if(timer>timerTotal)
        {
            timer=0;
            TimerContent();
        }
        else{
            timer+=Time.deltaTime;
        }
    }
    protected virtual void TimerContent()
    {
        //Override this function to add content
        print("Time's up!");
    }

    protected virtual void Damaged(float damage)
    {
        hp=Mathf.Max(0,hp-damage);
        if(hp<=0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
