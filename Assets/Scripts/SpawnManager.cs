using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject gameOverPanel;
    private PlayerController playerControllerScript;
    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Spawns 3 enemy's at random locations 
        SpawnEnemyWave(waveNumber);
        SpawnPowerup();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<Enemy>().Length;

        if(enemyCount == 0 && playerControllerScript.gameOver == false)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            SpawnPowerup();
        } else if(playerControllerScript.gameOver == true)
        {
            TriggerGameOver();
        }
    }

    /**
    * I decide to make this a method so I could spawn multiple enemy waves
    */
    void SpawnEnemyWave(int enemiesToSpawn)
    {
       for(int i = 0; i < enemiesToSpawn; i++)
        {
            //Summons an enemy at the random position generated on call
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        } 
    }

    void SpawnPowerup()
    {
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
    }

    /**
    * Creates a random spawn position for enemies and Powerups to provide variety for the player
    * I made this a method since I'll be spawning both consistently
    */
    private Vector3 GenerateSpawnPosition ()
    {

        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    public void TriggerGameOver()
    {
        string[] tagsToDelete = { "Enemy", "Powerup"};

        foreach (string tag in tagsToDelete)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objectsWithTag)
            {
                Destroy(obj);
            }
        }
        gameOverPanel.SetActive(true);
    }
}
