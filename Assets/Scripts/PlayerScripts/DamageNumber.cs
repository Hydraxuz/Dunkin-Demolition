using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    private const float Lifetime = 0.8f;
    private const float RiseSpeed = 1.2f;

    private float elapsed;
    private TextMeshPro text;
    private Color startColor;

    public static void Create(Vector3 position, float damage, bool isKillingBlow)
    {
        GameObject damageNumberObject = new GameObject("Damage Number");
        damageNumberObject.transform.position = position + Vector3.up * 0.5f;

        DamageNumber damageNumber = damageNumberObject.AddComponent<DamageNumber>();
        damageNumber.Initialize(damage, isKillingBlow);
    }

    private void Initialize(float damage, bool isKillingBlow)
    {
        text = gameObject.AddComponent<TextMeshPro>();
        text.text = Mathf.RoundToInt(damage).ToString();
        text.fontSize = isKillingBlow ? 15f : 10f;
        text.alignment = TextAlignmentOptions.Center;
        text.color = isKillingBlow ? Color.yellow : Color.white;
        text.outlineWidth = 0.2f;
        text.outlineColor = Color.black;
        text.sortingOrder = 10;
        startColor = text.color;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        transform.position += Vector3.up * (RiseSpeed * Time.deltaTime);

        Color color = startColor;
        color.a = 1f - elapsed / Lifetime;
        text.color = color;

        if (elapsed >= Lifetime)
        {
            Destroy(gameObject);
        }
    }
}