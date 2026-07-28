using UnityEngine;

public class MetalBlocks : MonoBehaviour
{
    public float health = 25f;
    public GameObject breakEffect;

    public AudioSource audioSource;
    public AudioClip metalBreakSound;
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

        if (audioSource != null && metalBreakSound != null)
        {
            audioSource.PlayOneShot(metalBreakSound, volume);
        }

        Destroy(gameObject);
    }
}
