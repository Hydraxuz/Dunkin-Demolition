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
        if (collisionInfo.relativeVelocity.magnitude > health)
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
        Destroy(gameObject);

        if (EnemiesAlive <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
