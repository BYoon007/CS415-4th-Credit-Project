using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoring : MonoBehaviour
{
    public TMP_Text ScoreText;
    
    public int score = 0;

    public int maxScore;
    
    void Start()
    {
        score = 0;
    }

    public void AddScore(int newScore)
    {
        score += newScore;
    }

    public void UpdateScore()
    {
        ScoreText.text = "Score:" + score;
    }

    void Update()
    {
        UpdateScore();
    }
}
