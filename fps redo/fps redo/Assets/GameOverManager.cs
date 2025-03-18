using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    void Start()
    {
        scoreText.text="Score: "+Mathf.RoundToInt(GameManager.score);
    }
}
