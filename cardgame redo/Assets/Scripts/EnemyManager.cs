using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{   
    [SerializeField]
    Enemy enemyPrefab;
    List<Enemy> enemies = new List<Enemy>();
    private int curEnemyCount = 0;
    private int maxEnemyCount = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (curEnemyCount < maxEnemyCount)
        {
            Enemy thisenemy;
            thisenemy = Instantiate(enemyPrefab);
            enemies.Add(thisenemy);
            RectTransform enemyTrans = thisenemy.GetComponent<RectTransform>();
            //and set the position of enemy
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(CardManager.gameStatus == GameStatus.End)
        {
            foreach(Enemy enemy in enemies)
            {
                //enemy move
            }
        }
    }
}
