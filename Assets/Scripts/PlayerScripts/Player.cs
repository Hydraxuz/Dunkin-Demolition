using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject playerDeathEffect;
    public float lifetimeAfterLaunch = 5f;

    protected Rigidbody2D rb;
    protected bool hasLaunched;
    protected bool hasHitSomething;
    private bool abilityUsed;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Abilities are only usable in flight, once per launch, before any collision
        if (hasLaunched && !hasHitSomething && !abilityUsed &&
            Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            abilityUsed = true;
            UseAbility();
        }
    }

    // Called by the slingshot when the player is launched
    public virtual void Launch()
    {
        hasLaunched = true;
        hasHitSomething = false;
        abilityUsed = false;
        StartCoroutine(LifetimeTimer());
    }

    // Override in donut variants to implement a space-bar ability
    protected virtual void UseAbility()
    {
    }

    private IEnumerator LifetimeTimer()
    {
        yield return new WaitForSeconds(lifetimeAfterLaunch);
        Die();
    }

    // Lets a spawned clone (e.g. from a split) start its own death timer without resetting ability state
    protected void StartLifetime()
    {
        StartCoroutine(LifetimeTimer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            StartCoroutine(Example());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasHitSomething = true;
    }

    private IEnumerator Example()
    {
        yield return new WaitForSeconds(5f);
        Die();
    }

    private void Die()
    {
        Instantiate(playerDeathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
