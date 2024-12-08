using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public TMP_Text pointsText;

    public static bool gameOverStatus;

    public static bool allowKeyboardInput = true;

    private HealthManager health;
   
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverStatus = false;
       
        GameObject healthObject = GameObject.Find("HealthManager");
        if (healthObject == null)
        {
            Debug.LogWarning("HealthManager GameObject not found. Creating one...");
            healthObject = new GameObject("HealthManager");
            health = healthObject.AddComponent<HealthManager>();
        }
        else
        {
            health = healthObject.GetComponent<HealthManager>();
            if (health == null)
            {
                Debug.LogWarning("HealthManager component not found on 'HealthManager'. Adding one...");
                health = healthObject.AddComponent<HealthManager>();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(int score)
    {   
        gameOverStatus = true;

        gameObject.SetActive(true);

        pointsText.text = score.ToString() + " POINTS";

        Time.timeScale = 0f;

        health.healthAmount = 100f; 

        allowKeyboardInput = false;
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        gameOverStatus = false;

    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameOverStatus = false;
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
