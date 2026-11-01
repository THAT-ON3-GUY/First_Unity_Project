using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 150f;
    private InputSystem_Actions controls;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private AudioSource playerAudio;
    public AudioSource gameAudio;
    private float powerUpStrength = 15.0f;
    public bool hasPowerup = false;
    public bool gameOver = false;
    public GameObject powerupIndicator;
    public AudioClip hitEnemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameObject targetObject = GameObject.Find("Main Camera");

        controls = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        playerAudio = GetComponent<AudioSource>();
        gameAudio = targetObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnEnable()
    {
        controls.Player.Enable();
        Debug.Log(controls.Player.Move);
        gameAudio.Play();
    }

    private void Update()
    {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();

        float forwardInput = moveInput.y;

        playerRb.AddForce(focalPoint.transform.forward * forwardInput * playerSpeed * Time.deltaTime);

        powerupIndicator.transform.position = transform.position;

        if(transform.position.y < -10 && !gameOver)
        {
            if (gameAudio != null)
            {
                gameAudio.Stop();  
            }

            gameOver = true;
            Destroy(gameObject, 0.1f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            playerAudio.PlayOneShot(hitEnemy, 1.0f);
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collieded with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        } else if (collision.gameObject.CompareTag("Enemy"))
        {
            playerAudio.PlayOneShot(hitEnemy, 1.0f);
        }
    }
}
