using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private List<PlanetGravityRadius> gravityRadiusList;

    private void Awake()
    {
        gravityRadiusList = new List<PlanetGravityRadius>();

        foreach (Transform child in transform)
        {
            PlanetGravityRadius gravityRadius = child.gameObject.GetComponent<PlanetGravityRadius>();
            gravityRadiusList.Add(gravityRadius);
            gravityRadius.gravityDelegate += DisableOuterGravityFields;
        }
    }

    private void OnDestroy()
    {
        foreach (PlanetGravityRadius gravityRadius in gravityRadiusList)
            gravityRadius.GetComponent<PlanetGravityRadius>().gravityDelegate -= DisableOuterGravityFields;
    }

    private void DisableOuterGravityFields()
    {
        for (int i = 0; i < gravityRadiusList.Count; ++i)
        {
            // If the gravity radius is not active, activate it.
            if (gravityRadiusList[i].applyingGravity == false)
            {
                gravityRadiusList[i].applyingGravity = true;

                //FOR DEBUGGING
                gravityRadiusList[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }

            // If the gravity radius is colliding with the player, deactivate all gravity radii after it.
            if (gravityRadiusList[i].isCollidingWithPlayer)
            {
                // Ensure the radius is not the last one in the list.
                if (i != gravityRadiusList.Count - 1)
                    for (int j = i + 1; j < gravityRadiusList.Count; ++j)
                    {
                        gravityRadiusList[j].applyingGravity = false;

                        //FOR DEBUGGING
                        gravityRadiusList[j].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }

                break;
            }
        }
    }
}