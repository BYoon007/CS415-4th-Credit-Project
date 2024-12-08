using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject airSlashPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public Transform player;  
    public float orbitRadius = 1.5f;  
    private PauseMenu pauseMenu; 
    private GameOverMenu gameOver;   


    void Start()
    {
         Time.timeScale = 1f;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (player == null)
            {
                Debug.LogError("Player Transform not found! Ensure the Player GameObject is tagged as 'Player'.");
            }
            else
            {
                Debug.Log("Player Transform successfully assigned.");
            }
        }
        
        
    }

    void Update()
    {
        if(!PauseMenu.isPaused && !GameOverMenu.gameOverStatus)
        {
            OrbitAroundPlayer(); 
        }
    }

    public void Fire()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = 0; 

        Vector3 fireDirection = mousePosition - player.position;

        float orbitRange = 2.5f;  

        if (fireDirection.magnitude < orbitRange)
        {
            fireDirection = fireDirection.normalized;
        }
        else
        {
            fireDirection = (mousePosition - firePoint.position).normalized;
        }

        GameObject airSlash = Instantiate(airSlashPrefab, firePoint.position, Quaternion.identity, null);

        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

        airSlash.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D airSlashRb = airSlash.GetComponent<Rigidbody2D>();
        if (airSlashRb != null)
        {
            airSlashRb.linearVelocity = fireDirection * fireForce; 
        }
        else
        {
            Debug.LogWarning("Rigidbody2D component missing on airSlashPrefab.");
        }
    }

    private void OrbitAroundPlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        Vector3 direction = (mousePosition - player.position).normalized;

        Vector3 orbitPosition = player.position + direction * orbitRadius;
        
        transform.position = orbitPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }


    
}
