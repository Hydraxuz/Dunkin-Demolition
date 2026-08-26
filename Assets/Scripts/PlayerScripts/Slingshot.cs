using UnityEngine;
using UnityEngine.InputSystem;

public class Slingshot : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] stripPositions;
    public Transform center;
    public Transform idlePosition;

    public Vector3 currentPosition;

    public float maxLength;
    public float bottomBoundary;

    private bool isMouseDown;

    public GameObject donutPrefab;
    private Rigidbody2D donut;
    private Collider2D donutCollider;
    public float donutPositionOffset;
    public float force;

    public AudioClip slingshotShoot;
    public AudioClip slingshotCharge;

    private Camera mainCamera;
    private Collider2D slingshotCollider;

    private void Start()
    {
        mainCamera = Camera.main;
        slingshotCollider = GetComponent<Collider2D>();

        if (lineRenderers != null && lineRenderers.Length >= 2 && stripPositions != null && stripPositions.Length >= 2)
        {
            lineRenderers[0].positionCount = 2;
            lineRenderers[1].positionCount = 2;
            lineRenderers[0].SetPosition(0, stripPositions[0].position);
            lineRenderers[1].SetPosition(0, stripPositions[1].position);
        }

        CreateDonut();
    }

    private void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && IsPointerOnSlingshot())
        {
            isMouseDown = true;
            if (slingshotCharge != null)
            {
                AudioSource.PlayClipAtPoint(slingshotCharge, transform.position);
            }
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame && isMouseDown)
        {
            isMouseDown = false;
            if (slingshotShoot != null)
            {
                AudioSource.PlayClipAtPoint(slingshotShoot, transform.position);
            }
            Shoot();
        }

        if (isMouseDown)
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            Vector2 pointerPosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, 10f));
            currentPosition = center.position + Vector3.ClampMagnitude(worldPosition - center.position, maxLength);
            currentPosition = ClampBoundary(currentPosition);

            SetStrips(currentPosition);

            if (donutCollider != null)
            {
                donutCollider.enabled = true;
            }
        }
        else
        {
            ResetStrips();
        }
    }

    private bool IsPointerOnSlingshot()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null || slingshotCollider == null)
        {
            return false;
        }

        Vector2 pointerWorldPosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return slingshotCollider.OverlapPoint(pointerWorldPosition);
    }

    private void CreateDonut()
    {
        if (donutPrefab == null)
        {
            return;
        }

        GameObject donutObject = Instantiate(donutPrefab);
        donut = donutObject.GetComponent<Rigidbody2D>();
        if (donut == null)
        {
            return;
        }

        donutCollider = donut.GetComponent<Collider2D>();
        if (donutCollider != null)
        {
            donutCollider.enabled = false;
        }

        donut.bodyType = RigidbodyType2D.Static;
    }

    private void Shoot()
    {
        if (donut == null)
        {
            return;
        }

        donut.bodyType = RigidbodyType2D.Dynamic;
        Vector3 donutForce = (currentPosition - center.position) * force * -1f;
        donut.linearVelocity = donutForce;

        donut = null;
        donutCollider = null;
        Invoke(nameof(CreateDonut), 2f);
    }

    private void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }

    private void SetStrips(Vector3 position)
    {
        if (lineRenderers == null || lineRenderers.Length < 2)
        {
            return;
        }

        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (donut != null)
        {
            Vector3 dir = position - center.position;
            donut.transform.position = position + dir.normalized * donutPositionOffset;
            donut.transform.right = -dir.normalized;
        }
    }

    private Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 10000f);
        return vector;
    }
}
