using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerDeathEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Instantiate(playerDeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Ground"))
        {
            StartCoroutine(Example());
        }
    }

    private IEnumerator Example()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
        Instantiate(playerDeathEffect, transform.position, Quaternion.identity);
    }
}
