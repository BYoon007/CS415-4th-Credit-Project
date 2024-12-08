using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;       
    public float initialSpawnInterval = 10f; 
    public float minimumSpawnInterval = 3f;  
    public float spawnDecreaseRate = 0.5f; 
    public int scoreThreshold = 10;      

    private float currentSpawnInterval;
    private Scoring scoring; 
    private bool isSpawning = false; 

    void Start()
    {
        Debug.Log("EnemySpawner Start called");

        if (enemyPrefab == null)
        {
            enemyPrefab = Resources.Load<GameObject>("Enemy");
            if (enemyPrefab == null)
            {
                Debug.LogError("Enemy prefab not found in Resources folder!");
                return;
            }
        }

        currentSpawnInterval = initialSpawnInterval;
        scoring = FindObjectOfType<Scoring>();

        
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {   
            Vector2 spawnPosition = GetRandomPositionWithinCameraBounds();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            Debug.Log("Enemy spawned at position: " + spawnPosition);

            yield return new WaitForSeconds(currentSpawnInterval);

            UpdateSpawnInterval();
        }
    }

    private Vector2 GetRandomPositionWithinCameraBounds()
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        float minX = -screenBounds.x + 0.5f; 
        float maxX = screenBounds.x - 0.5f;
        float minY = -screenBounds.y + 0.5f;
        float maxY = screenBounds.y - 0.5f;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector2(x, y);
    }

    private void UpdateSpawnInterval()
    {
        if (scoring != null && scoring.score / scoreThreshold > 0)
        {
            float newSpawnInterval = initialSpawnInterval - (scoring.score / scoreThreshold) * spawnDecreaseRate;
            currentSpawnInterval = Mathf.Max(newSpawnInterval, minimumSpawnInterval);
        }
    }
}
