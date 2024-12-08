using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;
    public float maxHealth = 100f;
    private GameOverMenu gameOverMenu;
    private Scoring scoring;

    void Start()
    {
        UpdateHealthBar();
        scoring = FindObjectOfType<Scoring>();
        GameObject gameOverMenuObject = GameObject.Find("GameOverMenu");
        
        if (gameOverMenuObject != null)
        {
            gameOverMenu = gameOverMenuObject.GetComponent<GameOverMenu>();
        }

        if (gameOverMenu != null)
        {
            gameOverMenu.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (healthAmount <= 0)
        {
            GameOver();
        }

        if (!GameOverMenu.gameOverStatus)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                TakeDamage(10);
            }

            if(Input.GetKeyDown(KeyCode.O))
            {
                Heal(10);
            }  
        }      
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth); 
        UpdateHealthBar();
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth); 
        Debug.Log("Player healed. Current Health: " + healthAmount);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = healthAmount / maxHealth;
        Debug.Log($"Health Bar Updated: {healthBar.fillAmount}");
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        if (gameOverMenu != null)
        {
            gameOverMenu.SetUp(scoring.score);
        }
        Time.timeScale = 0; 
        healthAmount = maxHealth;  
    }
}
