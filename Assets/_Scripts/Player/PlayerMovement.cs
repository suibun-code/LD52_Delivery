using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform planetTransform;
    [SerializeField] private float moveCooldownMin;
    [SerializeField] private float moveCooldownMax;
    [SerializeField] private float movePowerMin;
    [SerializeField] private float movePowerMax;

    [Tooltip("How much percent of the screen the move line can take up.")]
    [SerializeField] [Range(0.15f, 0.5f)] private float moveLineMaxLength;

    [SerializeField] private LineRenderer lineRenderer;

    private Camera mainCamera;
    private Rigidbody2D rb;

    // Movement variables.
    private bool isMoving = false;
    private bool isSettingMoveLine = false;
    private float moveCooldown;
    [SerializeField] private float moveCooldownTimer;
    private float movePower;

    // Move line variables.
    private float moveLineEffectiveDistanceScreenSpace;
    private float moveLineDistanceRange; // Number 0-1, 0 being the min distance and 1 being the max distance. Can go above 1.
    private float moveLineLength;

    // Mouse position variables.
    Vector2 dirTowardsMousePos;
    float distBetweenMouseEventsClamped;
    private Vector2 mousePos;
    private Vector2 lastMouseClickPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        moveLineEffectiveDistanceScreenSpace = CalculateDiagonalScreenSize() * moveLineMaxLength;
    }

    private void Update()
    {
        if (isSettingMoveLine)
        {
            CalculateMoveLine();
        }
    }

    private void OnLMB(InputValue value)
    {
        if (isMoving)
            return;

        // Get the mouse position.
        mousePos = Mouse.current.position.ReadValue();

        if (value.isPressed)       // LMB Pressed
        {
            // Set the last mouse click position. This is used to calculate the move line length.
            lastMouseClickPos = mousePos;

            // Set the start position of the move line.
            lineRenderer.SetPosition(0, mainCamera.ScreenToWorldPoint(lastMouseClickPos));
            lineRenderer.SetPosition(0, new Vector3(lineRenderer.GetPosition(0).x, lineRenderer.GetPosition(0).y, 0));

            // The player has clicked a button to begin setting the move line.
            isSettingMoveLine = true;
        }
        else if (!value.isPressed && isSettingMoveLine == true) // LMB Released
        {
            // Move cooldown is the max distance multiplied by the difference between the max and min move cooldowns.
            moveCooldown = Mathf.Lerp(moveCooldownMin, moveCooldownMax, moveLineDistanceRange);

            // Move power is the max distance multiplied by the difference between the max and min move powers.
            movePower = Mathf.Lerp(movePowerMin, movePowerMax, moveLineDistanceRange);

            // Reset the move line.
            lineRenderer.SetPosition(0, Vector2.zero);
            lineRenderer.SetPosition(1, Vector2.zero);

            // The player has released the button and is no longer setting the move line.
            isSettingMoveLine = false;

            isMoving = true;

            rb.AddForce(dirTowardsMousePos * movePower, ForceMode2D.Impulse);

            StartCoroutine(CountdownMoveCooldown());
        }
    }

    private IEnumerator CountdownMoveCooldown()
    {
        moveCooldownTimer = moveCooldown;

        while (moveCooldownTimer > 0)
        {
            moveCooldownTimer -= Time.deltaTime;
            yield return null;
        }

        isMoving = false;

        yield return null;
    }

    public void AddPlanetGravity(Vector2 force, Vector2 direction)
    {
        rb.AddForce(force);
    }

    private void CalculateMoveLine()
    {
        // Get the mouse position.
        mousePos = Mouse.current.position.ReadValue();

        // Get the direction towards the mouse position from the last mouse click position.
        dirTowardsMousePos = (mousePos - lastMouseClickPos).normalized;

        // Get the distance between the last mouse click position and the mouse position.
        float distBetweenMouseEvents = Vector2.Distance(lastMouseClickPos, mousePos);

        // Clamp the distance between the last mouse click position and the mouse position to the max move line distance.
        distBetweenMouseEventsClamped = Mathf.Clamp(distBetweenMouseEvents, 0, moveLineEffectiveDistanceScreenSpace);

        // Get the current move line distance range (0 = min, 1 = max).
        moveLineDistanceRange = distBetweenMouseEventsClamped / moveLineEffectiveDistanceScreenSpace;

        // Lerp the move line length between 0 and the max move line length based on the current move line distance range.
        moveLineLength = Mathf.Lerp(0, moveLineEffectiveDistanceScreenSpace, moveLineDistanceRange);

        // Set the end position of the move line.
        Vector2 endPos = lastMouseClickPos + (dirTowardsMousePos * distBetweenMouseEventsClamped);

        // Set the start position of the move line.
        lineRenderer.SetPosition(0, mainCamera.ScreenToWorldPoint(lastMouseClickPos));
        lineRenderer.SetPosition(0, new Vector3(lineRenderer.GetPosition(0).x, lineRenderer.GetPosition(0).y, 0));

        // Set the end position of the move line in world space.
        lineRenderer.SetPosition(1, mainCamera.ScreenToWorldPoint(endPos));
        lineRenderer.SetPosition(1, new Vector3(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y, 0));

        //Debug.Log("move line length (screen-space): " + moveLineLength);
    }

    private float CalculateDiagonalScreenSize()
    {
        float diagonalScreenSize = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));

        return diagonalScreenSize;
    }
}
