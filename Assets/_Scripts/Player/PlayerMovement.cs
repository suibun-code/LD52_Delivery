using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float boostPower;
    [SerializeField] private Transform planetTransform;
    private Rigidbody2D rb;
    [SerializeField] private bool inBoost;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputValue value)
    {
        rb.AddForce(value.Get<Vector2>() * speed, ForceMode2D.Impulse);
    }

    private void OnBoost(InputValue value)
    {
        inBoost = true;

        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * boostPower, ForceMode2D.Impulse);

        StartCoroutine(BoostedTimer());
    }

    public void AddPlanetGravity(Vector2 force, Vector2 direction)
    {
        if (!inBoost)
        {
            rb.AddTorque(force.magnitude);
            rb.AddForce(force);
            //rb.AddForce((force * 0.75f) * transform.up);
        }
    }

    IEnumerator BoostedTimer()
    {
        yield return new WaitForSeconds(0.35f);

        inBoost = false;
    }
}
