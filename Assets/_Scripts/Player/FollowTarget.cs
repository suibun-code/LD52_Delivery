using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    enum KeepToSideOf
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }

    [SerializeField] private KeepToSideOf keepToSideOf;
    [SerializeField] private SpriteRenderer followTarget;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = GetComponent<RectTransform>().position;
    }

    void Update()
    {
        if (keepToSideOf == KeepToSideOf.BOTTOM)
        {
            transform.position = new Vector3(
                followTarget.transform.position.x + originalPosition.x,
                followTarget.transform.position.y + (followTarget.bounds.min.y - followTarget.transform.position.y) + originalPosition.y,
                transform.position.z + originalPosition.z);
        }
        else if (keepToSideOf == KeepToSideOf.TOP)
        {
            transform.position = new Vector3(
                followTarget.transform.position.x + originalPosition.x,
                followTarget.transform.position.y + (followTarget.bounds.max.y - followTarget.transform.position.y) + originalPosition.y,
                transform.position.z + originalPosition.z);
        }
        else if (keepToSideOf == KeepToSideOf.LEFT)
        {
            transform.position = new Vector3(
                followTarget.transform.position.x + (followTarget.bounds.min.x - followTarget.transform.position.x) + originalPosition.x,
                followTarget.transform.position.y + originalPosition.y,
                transform.position.z + originalPosition.z);
        }
        else if (keepToSideOf == KeepToSideOf.RIGHT)
        {
            transform.position = new Vector3(
                followTarget.transform.position.x + (followTarget.bounds.max.x - followTarget.transform.position.x) + originalPosition.x,
                followTarget.transform.position.y + originalPosition.y,
                transform.position.z + originalPosition.z);
        }
    }
}
