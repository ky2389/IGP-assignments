using UnityEngine;

public class EnemyLV3 : EnemyBase
{
    bool secondCharge=true;
    protected override void TimerContent()
    {   
        nav.SetDestination(target.position);
    }
    protected override void Die()
    {
        if(secondCharge)
        {
            secondCharge=false;
            hp=hpTotal;
            nav.speed*=2;
        }
        else
        {
            base.Die();
        }
    }
}
