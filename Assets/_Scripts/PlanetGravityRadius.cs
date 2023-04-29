using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravityRadius : MonoBehaviour
{
    [SerializeField] float intensity;
    [SerializeField] float playerMagnituteForceMultiplier;
    [SerializeField] float pull;

    public bool isCollidingWithPlayer = false;

    public delegate void myDelegate();
    public myDelegate myDelegateInstance;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            myDelegateInstance();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
            myDelegateInstance();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector2 direction = (transform.position - other.transform.position).normalized * pull;

            float force = intensity * (other.GetComponent<Rigidbody2D>().velocity.magnitude * playerMagnituteForceMultiplier);

            Vector2 gravity = direction * force;

            other.GetComponent<PlayerMovement>().AddPlanetGravity(gravity, direction);
        }
    }
}