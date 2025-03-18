using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform player;
    [SerializeField]
    AudioClip bgm;
    public static float score;
    [SerializeField]
    TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Singleton pattern
        if(instance==null)
        {
            instance=this;
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
    }
    private void GameRestart(){
        score=0;
    }
    private void Start()
    {
        MusicManager.instance.SwitchMusic(bgm);
    }
    // Update is called once per frame
    void Update()
    {
        score+=Time.deltaTime;
        scoreText.text="Score: "+Mathf.RoundToInt(score);
    }
}
