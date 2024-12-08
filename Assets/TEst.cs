using UnityEngine;

public class TEst : MonoBehaviour
{
    public Scoring score;

    private GameOverMenu gameOver; 

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !GameOverMenu.gameOverStatus)
        {
            score.AddScore(1);
        }
        
    }
}
