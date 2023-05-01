using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Move Power")]
    [SerializeField] private float movePowerMin;
    [SerializeField] private float movePowerMax;

    [Header("Move Line")]
    [SerializeField] private LineRenderer lineRenderer;
    [Tooltip("How much percent of the screen the move line can take up. Can only be changed before play.")]
    [SerializeField][Range(0.15f, 0.5f)] private float moveLineMaxLength;
    [SerializeField] private float minLineThickness;
    [SerializeField] private float maxLineThickness;
    [SerializeField] private Color smallColor;
    [SerializeField] private Color largeColor;

    [Header("Move Cooldown")]
    [SerializeField] private Slider moveCooldownSlider;
    [SerializeField] private float moveCooldownMin;
    [SerializeField] private float moveCooldownMax;

    [Header("Stamina")]
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float staminaMax;
    [SerializeField] private float staminaUsageMin;
    [SerializeField] private float staminaUsageMax;
    [SerializeField] private float staminaRegenRate;
    [SerializeField] private float staminaRegenTimer;

    // Movement variables.
    private bool isMoving = false;
    private bool isSettingMoveLine = false;
    private float moveCooldown;
    private float moveCooldownTimer;
    private float movePower;

    // Move line variables.
    private float moveLineEffectiveDistanceScreenSpace;
    private float moveLineDistanceRange; // Number 0-1, 0 being the min distance and 1 being the max distance. Can go above 1.
    private float moveLineLength;

    // Mouse position variables.
    private float distBetweenMouseEventsClamped;
    private Vector2 dirTowardsMousePos;
    private Vector2 mousePos;
    private Vector2 lastMouseClickPos;

    // Stamina variables.
    private float stamina;
    private IEnumerator staminaCoroutine;
    private bool isStaminaRegenTimerRunning;

    private void Start()
    {
        mainCamera = Camera.main;
        moveLineEffectiveDistanceScreenSpace = CalculateDiagonalScreenSize() * moveLineMaxLength;

        stamina = staminaMax;
        staminaSlider.maxValue = staminaMax;
        staminaSlider.value = staminaMax;

    }

    private void Update()
    {
        if (isSettingMoveLine)
            CalculateMoveLine();
    }

    private void OnLMB(InputValue value)
    {
        // LMB Pressed
        if (value.isPressed)
        {
            // The player has clicked a button to begin setting the move line.
            isSettingMoveLine = true;

            // Get the mouse position.
            mousePos = Mouse.current.position.ReadValue();

            // Set the last mouse click position. This is used to calculate the move line length.
            lastMouseClickPos = mousePos;

            // If the player is already moving, don't allow them to set a new move line.
            if (isMoving)
            {
                // Set the start position of the move line.
                lineRenderer.SetPosition(0, mainCamera.ScreenToWorldPoint(lastMouseClickPos));
                lineRenderer.SetPosition(0, new Vector3(lineRenderer.GetPosition(0).x, lineRenderer.GetPosition(0).y, 0));
            }

        }

        // LMB Released
        else if (!value.isPressed && isSettingMoveLine == true)
        {
            isSettingMoveLine = false;

            // Move cooldown is the max distance multiplied by the difference between the max and min move cooldowns.
            moveCooldown = Mathf.Lerp(moveCooldownMin, moveCooldownMax, moveLineDistanceRange);

            // Move power is the max distance multiplied by the difference between the max and min move powers.
            movePower = Mathf.Lerp(movePowerMin, movePowerMax, moveLineDistanceRange);

            // Reset the move line.
            lineRenderer.SetPosition(0, Vector2.zero);
            lineRenderer.SetPosition(1, Vector2.zero);

            // If the player is already moving, don't allow them to set a new move line.
            if (isMoving)
                return;

            // If the move would make the stamina fall below 0, don't allow the player to move.
            if (stamina - Mathf.Lerp(staminaUsageMin, staminaUsageMax, moveLineDistanceRange) <= 0)
                return;

            isMoving = true;
            rigidBody.AddForce(dirTowardsMousePos * movePower, ForceMode2D.Impulse);

            StartCoroutine(UseStamina());
            StartCoroutine(CountdownMoveCooldown());
        }
    }

    private IEnumerator CountdownMoveCooldown()
    {
        moveCooldownSlider.gameObject.SetActive(true);
        moveCooldownSlider.maxValue = moveCooldown;

        moveCooldownTimer = moveCooldown;

        while (moveCooldownTimer > 0)
        {
            moveCooldownTimer -= Time.deltaTime;
            moveCooldownSlider.value = moveCooldownTimer;
            yield return null;
        }

        moveCooldownSlider.gameObject.SetActive(false);
        isMoving = false;

        yield return null;
    }

    private IEnumerator UseStamina()
    {
        stamina -= Mathf.Lerp(staminaUsageMin, staminaUsageMax, moveLineDistanceRange);
        staminaSlider.value = stamina;

        if (isStaminaRegenTimerRunning)
        {
            StopCoroutine(staminaCoroutine);
        }

        staminaCoroutine = StaminaRegenTimer();

        yield return StartCoroutine(staminaCoroutine);

        StartCoroutine(RegenStamina());
    }

    private IEnumerator StaminaRegenTimer()
    {
        Debug.Log("isStaminaRegenTimerRunning = true");
        isStaminaRegenTimerRunning = true;

        yield return new WaitForSeconds(staminaRegenTimer);

        isStaminaRegenTimerRunning = false;
        Debug.Log("isStaminaRegenTimerRunning = false");
    }

    private IEnumerator RegenStamina()
    {
        while (stamina < staminaMax && !isStaminaRegenTimerRunning)
        {
            stamina += staminaRegenRate * Time.deltaTime;
            staminaSlider.value = stamina;

            yield return null;
        }

        yield return null;
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

        lineRenderer.startColor = Color.Lerp(smallColor, largeColor, moveLineDistanceRange);
        lineRenderer.endColor = Color.Lerp(smallColor, largeColor, moveLineDistanceRange);
        lineRenderer.startWidth = Mathf.Lerp(minLineThickness, maxLineThickness, moveLineDistanceRange);

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
