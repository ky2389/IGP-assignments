using UnityEngine;

public class EnemyLV1 : EnemyBase
{
    //Blue heart enemy that moves faster and chasing player faster
    protected override void TimerContent()
    {
        nav.SetDestination(target.position);
    }
    protected override void Damaged(float damage)
    {
        hp=Mathf.Max(0,hp-damage*2);
        if(hp<=0)
        {
            Die();
        }
    }
}
