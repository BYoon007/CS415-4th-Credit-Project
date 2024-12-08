using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int health = 5;
    public Color flashColor = Color.red; 
    public float flashDuration = 0.1f;   

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Scoring scoring; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        scoring = FindObjectOfType<Scoring>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject); 
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = flashColor; 
        yield return new WaitForSeconds(flashDuration); 
        spriteRenderer.color = originalColor; 
    }

    private void Die()
    {
        if (scoring != null)
        {
            scoring.AddScore(1); 
        }
        
        Destroy(gameObject); 
    }
}
