using UnityEngine;

public class StoneBlocks : MonoBehaviour
{
    private float health = 15f;
    public GameObject breakEffect;

    public AudioSource audioSource;
    public AudioClip stoneBreakSound;
    private const float volume = 0.5f;

    private void OnCollisionEnter2D(Collision2D colInfo)
    {
        if (colInfo.relativeVelocity.magnitude > health)
        {
            Break();
        }
    }

    private void Break()
    {
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }

        if (audioSource != null && stoneBreakSound != null)
        {
            audioSource.PlayOneShot(stoneBreakSound, volume);
        }

        Destroy(gameObject);
    }
}
