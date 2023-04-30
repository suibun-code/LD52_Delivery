using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private List<GameObject> gravityRadiusList;

    private void Awake()
    {
        gravityRadiusList = new List<GameObject>();

        foreach (Transform child in transform)
        {
            gravityRadiusList.Add(child.gameObject);
            child.GetComponent<PlanetGravityRadius>().gravityDelegate += DisableOuterGravityFields;
        }
    }

    private void DisableOuterGravityFields()
    {
        for (int i = 0; i < gravityRadiusList.Count; ++i)
        {
            // If the gravity radius is not active, activate it.
            if (gravityRadiusList[i].activeInHierarchy == false)
                gravityRadiusList[i].SetActive(true);

            // If the gravity radius is colliding with the player, deactivate all gravity radii after it.
            if (gravityRadiusList[i].GetComponent<PlanetGravityRadius>().isCollidingWithPlayer)
            {
                // Ensure the radius is not the last one in the list.
                if (i != gravityRadiusList.Count - 1)
                    for (int j = i + 1; j < gravityRadiusList.Count; ++j)
                        gravityRadiusList[j].SetActive(false);

                break;
            }
        }
    }
}