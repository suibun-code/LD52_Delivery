using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderStay : MonoBehaviour
{
    [SerializeField] private Rigidbody2D followTarget;
    [SerializeField] private Transform lowestPoint;
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        Vector3 closest = followTarget.ClosestPoint(lowestPoint.position);

        transform.position = new Vector3(
            followTarget.position.x, followTarget.position.y + (closest.y - followTarget.position.y)) + offset;
    }
}