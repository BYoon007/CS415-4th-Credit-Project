using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;
    public float speed = 3.5f; 
    public float maxSpeed = 7f; 
    public float speedIncrease = 0.5f; 
    public int scoreThreshold = 10; 

    private float distance;
    private Scoring scoring;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        scoring = FindObjectOfType<Scoring>();
    }

    void Update()
    {
        UpdateSpeed();

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-7, 7, 1); 
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(7, 7, 1); 
        }
    }

    private void UpdateSpeed()
    {
        if (scoring != null)
        {
            int intervals = scoring.score / scoreThreshold;

            speed = Mathf.Min(3.5f + intervals * speedIncrease, maxSpeed);
        }
    }
}
