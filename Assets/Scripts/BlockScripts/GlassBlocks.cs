using UnityEngine;

public class GlassBlocks : MonoBehaviour
{
    private float health = 3f;
    public GameObject breakEffect;

    public AudioSource audioSource;
    public AudioClip glassBreakSound;
    private const float volume = 0.3f;

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

        if (audioSource != null && glassBreakSound != null)
        {
            audioSource.PlayOneShot(glassBreakSound, volume);
        }

        Destroy(gameObject);
    }
}
