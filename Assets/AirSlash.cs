using UnityEngine;

public class AirSlash : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    void Start()
    {
        Destroy(gameObject, 3f); 
    }

    void Update()
    {

    }
}