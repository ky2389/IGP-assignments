using UnityEngine;

public class EnemyLV2 : EnemyBase
{
    protected override void TimerContent()
    {   
        hp=Mathf.Min(hp+Time.deltaTime,hpTotal);
        nav.SetDestination(target.position);
    }
}
