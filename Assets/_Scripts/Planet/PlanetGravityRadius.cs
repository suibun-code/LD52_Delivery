using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravityRadius : MonoBehaviour
{
    private enum GravityType
    {
        Inner,
        Outer
    }

    [SerializeField] GravityType gravityType;
    [SerializeField] float intensity;
    [SerializeField] float playerMagnituteForceMultiplier;
    [SerializeField] float pull;

    private static float playerDrag = -1f;
    private Rigidbody2D playerRigidBody;
    public bool isCollidingWithPlayer = false;

    public delegate void planetGravityDelegate();
    public planetGravityDelegate gravityDelegate;

    public bool applyingGravity = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRigidBody == null)
                playerRigidBody = other.GetComponent<Rigidbody2D>();

            Debug.Log("TRIGGER ENTER: " + name);

            isCollidingWithPlayer = true;
            gravityDelegate();

            if (playerDrag == -1f)
            {
                playerDrag = playerRigidBody.drag;
                Debug.Log("FIRST SET playerDrag: " + name);
            }

            playerRigidBody.drag = 0;
            Debug.Log("SET RIGIDBODY ZERO: " + name);

            // If the gravity type is outer, apply a boost of force to the player when they enter the gravity field.
            if (applyingGravity && gravityType == GravityType.Outer)
            {
                Vector3 enteranceVelocity = playerRigidBody.velocity;
                playerRigidBody.AddForce(enteranceVelocity * 100f);
                Debug.Log("ENTRY FORCE APPLIED");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRigidBody == null)
                playerRigidBody = other.GetComponent<Rigidbody2D>();

            isCollidingWithPlayer = false;
            gravityDelegate();

            // If the gravity type is outer, set the player's drag back to normal.
            if (gravityType == GravityType.Outer)
            {
                playerRigidBody.drag = playerDrag;
                Debug.Log("SET rigidBody.drag = " + playerDrag + ": " + name);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRigidBody == null)
                playerRigidBody = other.GetComponent<Rigidbody2D>();

            if (applyingGravity)
            {
                Vector2 direction = (transform.position - other.transform.position).normalized * pull;
                float force = intensity * (other.GetComponent<Rigidbody2D>().velocity.magnitude * playerMagnituteForceMultiplier);
                Vector2 gravity = direction * force;
                playerRigidBody.AddForce(gravity);
            }
        }
    }
}