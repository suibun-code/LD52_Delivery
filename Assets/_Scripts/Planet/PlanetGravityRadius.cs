using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravityRadius : MonoBehaviour
{
    [SerializeField] float intensity;
    [SerializeField] float playerMagnituteForceMultiplier;
    [SerializeField] float pull;

    private static float playerDrag;
    public bool isCollidingWithPlayer = false;

    public delegate void planetGravityDelegate();
    public planetGravityDelegate gravityDelegate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            gravityDelegate();

            playerDrag = other.GetComponent<Rigidbody2D>().drag;
            other.GetComponent<Rigidbody2D>().drag = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
            gravityDelegate();

            other.GetComponent<Rigidbody2D>().drag = playerDrag;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector2 direction = (transform.position - other.transform.position).normalized * pull;
            float force = intensity * (other.GetComponent<Rigidbody2D>().velocity.magnitude * playerMagnituteForceMultiplier);
            Vector2 gravity = direction * force;
            other.GetComponent<Rigidbody2D>().AddForce(gravity);
        }
    }
}