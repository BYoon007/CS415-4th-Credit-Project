using UnityEngine;
using System.Collections;

public class HealthPotionSpawner : MonoBehaviour
{
    public GameObject healthPotionPrefab;    
    public float spawnInterval = 10f;        
    public LayerMask environmentLayer;       

    private Camera mainCamera;
    private Scoring scoring; 

    void Start()
    {
        mainCamera = Camera.main;
        scoring = FindObjectOfType<Scoring>();

        StartCoroutine(SpawnHealthPotion());
    }

    private IEnumerator SpawnHealthPotion()
    {
        while (true)
        {
            if (scoring != null && scoring.score > 10)
            {
                if (Random.Range(0, 10) < 5) 
                {
                    Vector2 spawnPosition = GetRandomPositionWithinCameraBounds();

                    if (!Physics2D.OverlapCircle(spawnPosition, 0.5f, environmentLayer))
                    {
                        Instantiate(healthPotionPrefab, spawnPosition, Quaternion.identity);
                        Debug.Log("Health potion spawned at position: " + spawnPosition);
                    }
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector2 GetRandomPositionWithinCameraBounds()
    {
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        float minX = -screenBounds.x + 1f; 
        float maxX = screenBounds.x - 1f;
        float minY = -screenBounds.y + 1f;
        float maxY = screenBounds.y - 1f;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector2(x, y);
    }
}
