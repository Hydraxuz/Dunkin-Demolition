using System.Collections;
using UnityEngine;

public class StrawberryDonut : Player
{
    public int splitCount = 5;
    public float splitTriangleRadius = 0.5f;
    public float splitSpreadAngle = 25f;
    public float splitSpeedMultiplier = 0.5f;
    public float splitColliderDisableDuration = 0.2f;

    protected override void UseAbility()
    {
        if (rb == null)
        {
            return;
        }

        Vector2 baseVelocity = rb.linearVelocity;
        Vector3 splitScale = transform.localScale / 3f;
        Vector2 splitCenter = transform.position;
        float startAngle = -splitSpreadAngle * (splitCount - 1) / 2f;

        for (int i = 0; i < splitCount; i++)
        {
            float angle = startAngle + i * splitSpreadAngle;
            Vector2 splitVelocity = Quaternion.Euler(0f, 0f, angle) * baseVelocity * splitSpeedMultiplier;
            float triangleAngle = 90f + i * 120f;
            Vector2 splitPosition = splitCenter + (Vector2)(Quaternion.Euler(0f, 0f, triangleAngle) * Vector2.up) * splitTriangleRadius;

            if (i == 0)
            {
                transform.position = splitPosition;
                transform.localScale = splitScale;
                rb.linearVelocity = splitVelocity;
                DisableCollidersTemporarily(gameObject);
                continue;
            }

            GameObject clone = Instantiate(gameObject, splitPosition, transform.rotation);
            clone.transform.localScale = splitScale;

            Rigidbody2D cloneRb = clone.GetComponent<Rigidbody2D>();
            if (cloneRb != null)
            {
                cloneRb.linearVelocity = splitVelocity;
            }

            StrawberryDonut cloneDonut = clone.GetComponent<StrawberryDonut>();
            if (cloneDonut != null)
            {
                cloneDonut.StartLifetime();
            }

            clone.SetActive(true);
            DisableCollidersTemporarily(clone);
        }
    }

    private void DisableCollidersTemporarily(GameObject target)
    {
        Collider2D[] colliders = target.GetComponentsInChildren<Collider2D>();
        if (colliders.Length == 0)
        {
            return;
        }

        StartCoroutine(ReenableCollidersAfterDelay(colliders, splitColliderDisableDuration));
    }

    private IEnumerator ReenableCollidersAfterDelay(Collider2D[] colliders, float delay)
    {
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(delay);

        foreach (Collider2D col in colliders)
        {
            if (col != null)
            {
                col.enabled = true;
            }
        }
    }
}
