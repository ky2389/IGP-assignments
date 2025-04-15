using UnityEngine;

public class TestManager : MonoBehaviour
{
    public class Enemybase
    {
        public string name;
        public int hp;
        public int attack;
        public int defense;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("enemyDB/enemyData1");
        string textContent = textAsset.text;
        string[] data=textContent.Split("##");
        Enemybase enemy=new Enemybase();
        enemy.name=data[0];
        enemy.hp=int.Parse(data[1]);
        enemy.attack=int.Parse(data[2]);
        enemy.defense=int.Parse(data[3]);
        print("Name: " + enemy.name);
        print("HP: " + enemy.hp);
        print("Attack: " + enemy.attack);
        print("Defense: " + enemy.defense);
    }
}
