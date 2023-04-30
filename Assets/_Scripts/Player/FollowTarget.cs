using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private SpriteRenderer followTarget;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            followTarget.transform.position.x + originalPosition.x, 
            followTarget.transform.position.y + (followTarget.bounds.min.y - followTarget.transform.position.y) + originalPosition.y,
            transform.position.z + originalPosition.z);
    }
}
