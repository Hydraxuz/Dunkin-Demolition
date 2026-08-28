using UnityEngine;

public class VanillaDonut : Player
{
    public float momentumMultiplier = 1.5f;

    protected override void UseAbility()
    {
        if (rb == null)
        {
            return;
        }

        rb.linearVelocity *= momentumMultiplier;
    }
}
