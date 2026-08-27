using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float health = 5f;
    public GameObject deathEffect;
    public static int EnemiesAlive = 0;

    public AudioSource audioSource;
    public AudioClip enemyDeathSound;
    private const float volume = 1f;
    private const float levelLoadDelay = 1.5f;
    private const float spawnImmunityDuration = 1f;
    private float immuneUntil;

    private void Awake()
    {
        immuneUntil = Time.time + spawnImmunityDuration;
    }

    private void Start()
    {
        EnemiesAlive++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        TakeDamage(collisionInfo.relativeVelocity.magnitude);
    }

    private void TakeDamage(float damage)
    {
        if (damage <= 0f || Time.time < immuneUntil)
        {
            return;
        }

        health -= damage;
        DamageNumber.Create(transform.position, damage, health <= 0f);

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        if (audioSource != null && enemyDeathSound != null)
        {
            audioSource.PlayOneShot(enemyDeathSound, volume);
        }

        EnemiesAlive = Mathf.Max(0, EnemiesAlive - 1);

        if (EnemiesAlive <= 0)
        {
            // Destroying this GameObject would cancel an Invoke scheduled on it,
            // so the delayed load runs on a separate object that outlives the enemy.
            new GameObject("LevelLoadTimer").AddComponent<LevelLoadTimer>().Begin(levelLoadDelay);
        }

        Destroy(gameObject);
    }
}

public class LevelLoadTimer : MonoBehaviour
{
    public void Begin(float delay)
    {
        DontDestroyOnLoad(gameObject);
        Invoke(nameof(LoadNextLevel), delay);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(gameObject);
    }
}
