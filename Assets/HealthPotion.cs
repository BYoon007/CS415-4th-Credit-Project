using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public float healthIncreaseAmount = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player collided with Health Potion.");

            HealthManager healthManager = collision.GetComponent<HealthManager>();
            if (healthManager != null)
            {

                healthManager.Heal(healthIncreaseAmount);
            }
            else
            {
                Debug.LogError("HealthManager component not found on Player.");
            }

            Destroy(gameObject);
        }
    }
}
