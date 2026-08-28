using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 hotspot;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    private void OnEnable()
    {
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}