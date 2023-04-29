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
            child.GetComponent<PlanetGravityRadius>().myDelegateInstance += Yo;
        }
    }

    private void Yo()
    {
        for (int i = 0; i < gravityRadiusList.Count; ++i)
        {
            if (gravityRadiusList[i].activeInHierarchy == false)
            {
                gravityRadiusList[i].SetActive(true);
                Debug.Log("ENABLED " + gravityRadiusList[i].name);
            }

            if (gravityRadiusList[i].GetComponent<PlanetGravityRadius>().isCollidingWithPlayer)
            {
                if (i != gravityRadiusList.Count - 1)
                {
                    for (int j = i + 1; j < gravityRadiusList.Count; ++j)
                    {
                        gravityRadiusList[j].SetActive(false);
                        Debug.Log("DISABLED " + gravityRadiusList[j].name);
                    }
                }

                break;
            }
        }
    }
}