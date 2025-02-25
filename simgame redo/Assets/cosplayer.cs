using UnityEngine;
using UnityEngine.AI;
namespace CharacterPack{
    public class cosplayer : MonoBehaviour
    {
        float _hungryValue=0;
        float HungryValue{
            get{
                return _hungryValue;
            }
            set{
                _hungryValue=Mathf.Max(0,value);
            }
        }
        float HungryValueTotal=1000;
        bool isDying=false;

        Transform restaurant;
        [SerializeField]
        Transform displayGroup;
        [SerializeField]
        Transform hungrybar;
        [SerializeField]
        Transform mainBody;
        [SerializeField]
        Animator anim;
 		[SerializeField]
		float hungrySpeed = 35f;

		Transform mainCamera;
        string currentDirection="Right";
        float timer_Direction=0;
        float timer_DirectionTotal=1;

		NavMeshAgent nav;
        float moveTimer=0;
        float moveTimertotal=0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            HungryValue=HungryValueTotal;
            nav=GetComponent<NavMeshAgent>();
            restaurant=GameObject.Find("Food_MaidCoffee").transform;
        }

        // Update is called once per frame
        void Update()
        {
            if(!isDying){
                if(HungryValue>0){
                    HungryValue-=Time.deltaTime*hungrySpeed;

                    //show hungry bar
                    if(HungryValue<HungryValueTotal/2){
                        displayGroup.localScale=Vector3.one;
                        hungrybar.transform.localScale=new Vector3(HungryValue/(HungryValueTotal/2),1,1);
                    }
                    else{
                        displayGroup.localScale=Vector3.zero;
                    }


                    if(moveTimer>moveTimertotal){
                        ResetMoveTimer();
                        
                        nav.isStopped=false;
                        if(HungryValue< HungryValueTotal/2){
                            nav.SetDestination(restaurant.position);
                            nav.speed=8f;
                        }
                        else{
                            nav.SetDestination(transform.position+new Vector3(Random.Range(-10,10),0,Random.Range(-10,10)));
                            nav.speed=3f;
                        }
                    }
                    else{
                        moveTimer+=Time.deltaTime;
                    }
                }
                else{
                    isDying=true;
                    anim.SetTrigger("Death");
                    nav.isStopped=true;
                }
                if(nav.velocity.magnitude<0.1f){
                    anim.SetBool("IsWalking",false);
                }
                else{
                    anim.SetBool("IsWalking",true);
                    anim.SetFloat("Velocity",nav.velocity.magnitude);
                }
                if(timer_Direction>timer_DirectionTotal){
                    timer_Direction=0;
                    timer_DirectionTotal=Random.Range(0.1f,0.3f);
                    
                    float currentRelativeDirection=Vector3.Dot(nav.velocity,Camera.main.transform.right);
                    if (currentRelativeDirection > 0 && currentDirection == "Right")
                    {
                        currentDirection = "Left";
                        var localS = mainBody.localScale;
                        localS.x *= -1;
                        mainBody.localScale = localS;
                    }
                    else if (currentRelativeDirection < 0 && currentDirection == "Left")
                    {
                        currentDirection = "Right";
                        var localS = mainBody.localScale;
                        localS.x *= -1;
                        mainBody.localScale = localS;
                    }
                }
                else{
                    timer_Direction+=Time.deltaTime;
                }

            }
        }
        void ResetMoveTimer(){
            moveTimer=0;
            moveTimertotal=Random.Range(1.1f,3.3f);//让人物走走停停
        }
        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Booth_Food")&&HungryValue< HungryValueTotal/2){
                anim.SetTrigger("Buy");
                ResetMoveTimer();
                nav.isStopped=true;
                HungryValue=HungryValueTotal;
            }
        }
    }
}