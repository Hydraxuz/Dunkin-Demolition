using UnityEngine;

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
    private bool isCounted;
    private bool hasDied;

    private void Awake()
    {
        immuneUntil = Time.time + spawnImmunityDuration;
    }

    private void Start()
    {
        EnemiesAlive++;
        isCounted = true;
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
        if (hasDied)
        {
            return;
        }

        hasDied = true;

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        if (audioSource != null && enemyDeathSound != null)
        {
            audioSource.PlayOneShot(enemyDeathSound, volume);
        }

        RemoveFromAliveCount();

        if (EnemiesAlive <= 0)
        {
            // Destroying this GameObject would cancel an Invoke scheduled on it,
            // so the delayed load runs on a separate object that outlives the enemy.
            new GameObject("LevelLoadTimer").AddComponent<LevelLoadTimer>().Begin(levelLoadDelay);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        RemoveFromAliveCount();
    }

    private void RemoveFromAliveCount()
    {
        if (!isCounted)
        {
            return;
        }

        EnemiesAlive = Mathf.Max(0, EnemiesAlive - 1);
        isCounted = false;
    }
}
