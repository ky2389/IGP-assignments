using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private float enemyCurrentHealth;
    [SerializeField]
    private float enemyMaxHealth;
    [SerializeField]

    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {   
        //could add sheild
        enemyCurrentHealth -= damage;
        if (enemyCurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {   
        Debug.Log(enemyName + " defeated!");
        Destroy(gameObject);
    }

    public void Init(EnemyManager enemyManager)
    {
        //set the position of enemy
        RectTransform enemyTrans = GetComponent<RectTransform>();
    }
}
