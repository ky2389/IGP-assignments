using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerUnit : MonoBehaviour, IMover
{
    Rigidbody rB;
    float speed=1000;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rB=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var pos=Position;
        pos.y=0.3f;
        Position=pos;
        Move();
    }
    public void Move()
    {
       var direction=-transform.localPosition;
       var newVel=direction*speed*Time.deltaTime;
       newVel.y=0;
       if(newVel.sqrMagnitude>1)
       {
           newVel=newVel.normalized*1;
       }
       newVel.y=rB.linearVelocity.y;
       rB.linearVelocity=newVel;
    }
    public void Remove()
    {
        Destroy(gameObject);
    }
}
