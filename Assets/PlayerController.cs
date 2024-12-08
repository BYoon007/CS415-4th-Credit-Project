using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;          
    public float maxSpeed = 10f;         
    public float speedIncrease = 1f;      
    public int scoreThreshold = 15;      

    public Animator animator;
    public Rigidbody2D rb;
    public Weapon weapon;
    public float paddingX = 0.5f;
    public float paddingY = 0.5f;
    public float knockbackForce = 10f;    
    public float damageFlashDuration = 0.1f;
    public float invulnerabilityDuration = 1.5f; 
    public float flashInterval = 0.1f;    

    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private Vector3 originalScale;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private bool isKnockedBack = false;
    private bool isInvulnerable = false; 

    public HealthManager healthManager;   
    private Scoring scoring;         

    public PauseMenu pauseMenu;   
    public GameOverMenu gameOverMenu; 

    void Start()
    {
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        originalColor = spriteRenderer.color; 

        if (healthManager == null)
        {
            healthManager = FindObjectOfType<HealthManager>();
        }

        scoring = FindObjectOfType<Scoring>();

    }

    void Update()
    {
        if (!isKnockedBack && !GameOverMenu.gameOverStatus) 
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            if (Input.GetMouseButtonDown(0) && !PauseMenu.isPaused && !GameOverMenu.gameOverStatus)
            {
                weapon.Fire();
            }

            UpdateSpeed();

            moveDirection = new Vector2(moveX, moveY).normalized;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            animator.SetFloat("Speed", Mathf.Abs(moveX));

            if (moveY > 0)
            {
                animator.SetBool("isMovingUp", true);
                animator.SetBool("DownMovement", false);
            }
            else if (moveY == 0)
            {
                animator.SetBool("isMovingUp", false);
                animator.SetBool("DownMovement", false);
            }
            else
            {
                animator.SetBool("DownMovement", true);
                animator.SetBool("isMovingUp", false);
            }

            if (Mathf.Abs(moveX) > 0)
            {
                animator.SetBool("HorizontalMovement", true);
            }
            else if (Mathf.Abs(moveX) == 0)
            {
                animator.SetBool("HorizontalMovement", false);
            }

            if (moveX > 0)
            {
                transform.localScale = originalScale; 
            }
            else if (moveX < 0)
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z); 
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }

        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        float minX = -screenBounds.x + paddingX;
        float maxX = screenBounds.x - paddingX;
        float minY = -screenBounds.y + paddingY;
        float maxY = screenBounds.y - paddingY;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void UpdateSpeed()
    {
        if (scoring != null)
        {
            int intervals = scoring.score / scoreThreshold;

            moveSpeed = Mathf.Min(5f + intervals * speedIncrease, maxSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvulnerable)
        {
            healthManager.TakeDamage(10); 
            StartCoroutine(FlashRed()); 
            StartCoroutine(ApplyKnockback(collision)); 
            StartCoroutine(Invulnerability()); 
        }

        if (healthManager.healthAmount <= 0) 
        {
            StopAllCoroutines(); 
            spriteRenderer.color = originalColor; 
            spriteRenderer.enabled = true;
            return; 
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red; 
        yield return new WaitForSeconds(damageFlashDuration); 
        spriteRenderer.color = originalColor;
    }

    private IEnumerator ApplyKnockback(Collision2D collision)
    {
        isKnockedBack = true;

        Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); 

        yield return new WaitForSeconds(0.3f); 

        rb.linearVelocity = Vector2.zero; 
        isKnockedBack = false; 
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true; 
        float elapsedTime = 0f;

        while (elapsedTime < invulnerabilityDuration)
        {
            spriteRenderer.enabled = false; 
            yield return new WaitForSeconds(flashInterval);
            spriteRenderer.enabled = true; 
            yield return new WaitForSeconds(flashInterval);

            elapsedTime += flashInterval * 2;
        }

        isInvulnerable = false; 
        spriteRenderer.enabled = true; 
    }
}
