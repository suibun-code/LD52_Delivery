using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform planetTransform;

    private float moveCooldown;
    private float moveCooldownTimer;
    private float minDistance;
    private float maxDistance;

    [SerializeField] private float moveCooldownMin;
    [SerializeField] private float moveCooldownMax;

    private Rigidbody2D rb;
    private Vector2 lastMouseClickPos;
    private float distanceBetweenMouseEvents;
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnLMB(InputValue value)
    {
        // Get the mouse position.
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (value.isPressed)       // LMB Pressed
        {
            lastMouseClickPos = mousePos;
        }
        else if (!value.isPressed) // LMB Released
        {
            distanceBetweenMouseEvents = Vector2.Distance(mousePos, lastMouseClickPos);

            // Min distance of the mouse events is 1% of the diagonal screen size.
            minDistance = CalculateDiagonalScreenSize() * 0.01f;
            // Max distance of the mouse events is half of the diagonal screen size.
            maxDistance = distanceBetweenMouseEvents / (CalculateDiagonalScreenSize() * 0.5f);

            // Move cooldown is the max distance multiplied by the difference between the max and min move cooldowns.
            moveCooldown = Mathf.Lerp(moveCooldownMin, moveCooldownMax, maxDistance);

            Debug.Log("Mouse Events Distance: " + distanceBetweenMouseEvents);
            Debug.Log("Move CD: " + moveCooldown);
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

// WHEN A PLAYER MOVES AND MAKE THEIR LINE, ACTUALLY DRAW IT.
