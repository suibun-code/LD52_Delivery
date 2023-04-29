using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");

        if (collision.CompareTag("Player"))
        {
            Debug.Log("collision player");
            Vector2 direction = transform.position - collision.transform.position;
            float force = Vector2.Distance(transform.position, collision.transform.position) * 3f;
            Vector2 gravity = direction * force;

            collision.GetComponent<PlayerMovement>().AddPlanetGravity(gravity);

        }
    }
}
