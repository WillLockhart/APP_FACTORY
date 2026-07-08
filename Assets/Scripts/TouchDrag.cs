using UnityEngine;
using UnityEngine.InputSystem;

public class TouchDrag : MonoBehaviour
{
    [Header("Drag Threshold")]
    [SerializeField] private float dragThreshold = 0.1f;
    [SerializeField] private float releaseThreshold = 0.5f;

    [Header("Drag Constraints")]
    [SerializeField] private Vector2 dragAxis = Vector2.right;
    [SerializeField] private float maxDragDistance = 2f;

    [Header("Bounce Back")]
    [SerializeField] private bool bounceBack = true;
    [SerializeField] private float bounceSpeed = 10f;
    [SerializeField] private float bounceDamping = 0.5f;

    public LevelManager levelManager;
    private Camera cam;
    private Vector3 startLocalPosition;
    private Vector3 offset;
    private bool dragging;

    public static bool IsDragging = false;

    //Spring mechanic, not sure how it works
    private Vector3 velocity;
    private bool bouncingBack;

    private Vector2 pressScreenPos;
    private bool pendingDrag;

    void Start()
    {
        cam = Camera.main;
        startLocalPosition = transform.localPosition;
        dragAxis = dragAxis.normalized;
    }

    void Update()
    {
        if (Touchscreen.current != null && levelManager.playerInputAllowed)
        {
            var touch = Touchscreen.current.primaryTouch;
            Vector2 screenPos = touch.position.ReadValue();

            if (touch.press.wasPressedThisFrame) TryStartDrag(screenPos);
            else if (touch.press.isPressed && (pendingDrag || dragging)) DoDrag(screenPos);
            else if (touch.press.wasReleasedThisFrame) { EndDrag(); OnRelease(); }
        }
        else if (Mouse.current != null)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame) TryStartDrag(screenPos);
            else if (Mouse.current.leftButton.isPressed && (pendingDrag || dragging)) DoDrag(screenPos);
            else if (Mouse.current.leftButton.wasReleasedThisFrame) { EndDrag(); OnRelease(); }
        }

        if (bouncingBack)
        {
            BounceTowardsStart();
        }
    }

    private void TryStartDrag(Vector2 screenPos)
    {
        Vector2 worldPos = cam.ScreenToWorldPoint(screenPos);

        //Use raycast all so the collided object isn't randomly selected
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        if (hits.Length == 0) return;

        //The first one the raycast hit is the top one
        RaycastHit2D topHit = hits[0];
        float closestZ = Mathf.Abs(hits[0].collider.transform.position.z - cam.transform.position.z);

        //Find smallest raycast distance in all hits
        foreach (var hit in hits)
        {
            float zDist = Mathf.Abs(hit.collider.transform.position.z - cam.transform.position.z);
            if (zDist < closestZ)
            {
                closestZ = zDist;
                topHit = hit;
            }
        }

        if (topHit.collider.gameObject == gameObject)
        {
            pendingDrag = true;
            bouncingBack = false;
            pressScreenPos = screenPos;
            offset = transform.position - GetWorldPosition(screenPos);
        }
    }

    private void DoDrag(Vector2 screenPos)
    {
        if (pendingDrag)
        {
            //Check drag threshold
            float moveDist = Vector2.Distance(cam.ScreenToWorldPoint(screenPos), cam.ScreenToWorldPoint(pressScreenPos));
            if (moveDist < dragThreshold) return;

            //Passed
            pendingDrag = false;
            dragging = true;
            IsDragging = true;
        }

        if (!dragging) return;

        Vector3 targetWorldPos = GetWorldPosition(screenPos) + offset;
        Vector3 targetLocalPos = transform.parent != null
            ? transform.parent.InverseTransformPoint(targetWorldPos)
            : targetWorldPos;

        Vector3 displacement = targetLocalPos - startLocalPosition;
        float distanceAlongAxis = Vector3.Dot(displacement, dragAxis);
        distanceAlongAxis = Mathf.Clamp(distanceAlongAxis, 0f, maxDragDistance);
        transform.localPosition = startLocalPosition + (Vector3)(dragAxis * distanceAlongAxis);
    }

    private void EndDrag()
    {
        pendingDrag = false;
        dragging = false;
        IsDragging = false;

        if (bounceBack)
        {
            bouncingBack = true;
            velocity = Vector3.zero;
        }

    }

    private void BounceTowardsStart()
    {
        //Spring-damper simulation (Not sure how this works :( )
        Vector3 toStart = startLocalPosition - transform.localPosition;
        Vector3 springForce = toStart * bounceSpeed;
        velocity += springForce * Time.deltaTime;
        velocity *= (1f - bounceDamping * Time.deltaTime);

        transform.localPosition += velocity * Time.deltaTime;

        if (toStart.sqrMagnitude < 0.0001f && velocity.sqrMagnitude < 0.0001f)
        {
            transform.localPosition = startLocalPosition;
            bouncingBack = false;
        }
    }

    private Vector3 GetWorldPosition(Vector2 screenPos)
    {
        Vector3 screenPoint = screenPos;
        screenPoint.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(screenPoint);
    }

    private void OnRelease()
    {
        float moveDist = Vector3.Distance(startLocalPosition, transform.localPosition);
        if (moveDist > releaseThreshold)
        {
            if (levelManager.playerInputAllowed)
            {
                levelManager.AddInput(GetComponent<InputObject>().inputType);
                GetComponent<InputObject>().PlaySound();
            }
        }
    }

    //In case object gets disabled
    private void OnDisable()
    {
        if (dragging)
        {
            dragging = false;
            IsDragging = false;
        }
    }
}