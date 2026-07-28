using UnityEngine;

public class WoodBlocks : MonoBehaviour
{
    private float health = 10f;
    public GameObject breakEffect;

    public AudioSource audioSource;
    public AudioClip woodBreakSound;
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

        if (audioSource != null && woodBreakSound != null)
        {
            audioSource.PlayOneShot(woodBreakSound, volume);
        }

        Destroy(gameObject);
    }
}
