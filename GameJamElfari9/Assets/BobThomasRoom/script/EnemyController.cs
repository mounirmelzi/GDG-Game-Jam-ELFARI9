using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float movementSpeed = 3f;
    public int maxHealth = 7;
    public GameObject bulletPrefab;
    public static int numberOfClones=1;
    private int currentHealth;
    private Transform player;
    private float spawnDelay = 3f;
    private Vector3 currentDirection;
    public float raycastDistance = 2f;
    public Slider healthbar;

    private void Start()
    {
        currentHealth = maxHealth;
        // Start with a random movement direction
    currentDirection = Random.insideUnitSphere;
    currentDirection.y = 0f;

        InvokeRepeating("SpawnEnemy", spawnDelay, spawnDelay);
    }

    private void Update()
    {
        healthbar.value=currentHealth;
        // Move towards the player
         // Move in the current direction
    transform.Translate(currentDirection * movementSpeed * Time.deltaTime);
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
             Debug.Log("touch it ");
        }
        if(other.CompareTag("Wall")){
            // Check for obstacles
            RaycastHit hit;
            if (Physics.Raycast(transform.position, currentDirection, out hit, raycastDistance))
            {                    // If an obstacle is detected, change the movement direction
                currentDirection = Vector3.Reflect(currentDirection, hit.normal);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Add death effects if needed
        numberOfClones -=1;
        Debug.Log(numberOfClones);
        if(numberOfClones==0){
        Debug.Log("change the scene");
         SceneManager.LoadScene("BobRoomAfterBattle");
    }
        Destroy(gameObject);
    }

    private void SpawnEnemy()
    {
        if(numberOfClones<30){
            if (numberOfClones < 4)
            {
                numberOfClones++;
                Instantiate(gameObject, transform.position, Quaternion.identity);
                Invoke("ResetSpawnDelay", 0.5f);
            }
            else
            {
                numberOfClones++;
                Instantiate(gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    private void ResetSpawnDelay()
    {
        CancelInvoke("SpawnEnemy");
        InvokeRepeating("SpawnEnemy", spawnDelay, spawnDelay);
    }
}
