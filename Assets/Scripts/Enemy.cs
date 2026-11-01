using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f;
    private Rigidbody enemyRb;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Sets the lookDirection variable to the Player's location and then normalizes it for use in the speed calculation
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        //Takes the direction found in lookDirection and then moves the Enemy towards the Player using that direction.
        enemyRb.AddForce(lookDirection * speed);

        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
