using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform planetTransform;
    [SerializeField] private float moveCooldownMin;
    [SerializeField] private float moveCooldownMax;
    [SerializeField] private float moveLineMaxLength;
    [SerializeField] private LineRenderer lineRenderer;

    private Camera mainCamera;
    private Rigidbody2D rb;

    // Movement variables.
    private bool isMoving = false;
    private bool isSettingMoveLine = false;

    // Move cooldown variables.
    private float moveCooldown;
    private float moveCooldownTimer;
    private float moveCooldownLineDistanceRange; // Number 0-1, 0 being the min distance and 1 being the max distance. 

    private float moveLineDistance;

    // Mouse position variables.
    private float distanceBetweenMouseEvents;
    private Vector2 mousePos;
    private Vector2 lastValidMousePos; // Last valid mouse position for move line placement. Going above max line distance will stop updating this value.
    private Vector2 lastMouseClickPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isSettingMoveLine)
        {
            mousePos = Mouse.current.position.ReadValue();

            //Debug.Log("DISTANCE: " + Vector2.Distance(lineRenderer.GetPosition(0), mousePos));
            //Debug.Log("MAX LINE DISTANCE: " + maxLineDistance);

            if (Vector2.Distance(lineRenderer.GetPosition(0), mousePos) > moveLineMaxLength)
            {
                lineRenderer.SetPosition(1, mainCamera.ScreenToWorldPoint(lastValidMousePos));
            }
            else
            {
                lineRenderer.SetPosition(1, mainCamera.ScreenToWorldPoint(mousePos));
                lastValidMousePos = mousePos;
            }

            lineRenderer.SetPosition(1, new Vector3(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y, 0));
        }
    }

    private void OnLMB(InputValue value)
    {
        // Get the mouse position.
        mousePos = Mouse.current.position.ReadValue();

        if (value.isPressed)       // LMB Pressed
        {
            lastMouseClickPos = mousePos;

            lineRenderer.SetPosition(0, mainCamera.ScreenToWorldPoint(lastMouseClickPos));
            lineRenderer.SetPosition(0, new Vector3(lineRenderer.GetPosition(0).x, lineRenderer.GetPosition(0).y, 0));

            isSettingMoveLine = true;
        }
        else if (!value.isPressed) // LMB Released
        {
            // Get the distance between the mouse events.
            distanceBetweenMouseEvents = Vector2.Distance(mousePos, lastMouseClickPos);

            // Max distance of the mouse events is half of the diagonal screen size.
            moveCooldownLineDistanceRange = distanceBetweenMouseEvents / (CalculateDiagonalScreenSize() * 0.5f);

            // Move cooldown is the max distance multiplied by the difference between the max and min move cooldowns.
            moveCooldown = Mathf.Lerp(moveCooldownMin, moveCooldownMax, moveCooldownLineDistanceRange);
            moveLineDistance = Mathf.Lerp(0, moveLineMaxLength, moveCooldownLineDistanceRange);

            //Debug.Log("Mouse Events Distance: " + distanceBetweenMouseEvents);
            //Debug.Log("Move CD: " + moveCooldown);

            lineRenderer.SetPosition(0, mainCamera.ScreenToWorldPoint(lastMouseClickPos));
            lineRenderer.SetPosition(1, mainCamera.ScreenToWorldPoint(mousePos));

            // Set the Z position of the line renderer to 0.
            lineRenderer.SetPosition(0, new Vector3(lineRenderer.GetPosition(0).x, lineRenderer.GetPosition(0).y, 0));
            lineRenderer.SetPosition(1, new Vector3(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y, 0));

            isSettingMoveLine = false;
        }
    }

    private IEnumerator AddConstantForceOverTime(Vector2 force)
    {
        rb.AddForce(force);

        yield return null;
    }

    public void AddPlanetGravity(Vector2 force, Vector2 direction)
    {
        rb.AddForce(force);
    }

    private float CalculateDiagonalScreenSize()
    {
        float diagonalScreenSize = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));

        return diagonalScreenSize;
    }
}
