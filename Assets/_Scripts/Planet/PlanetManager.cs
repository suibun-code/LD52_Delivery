using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    // List of all planets.
    [SerializeField] private List<Planet> planets;

    public List<Planet> Planets { get { return planets; } private set { planets = value; } }

    private static int planetCount = 0;

    public static int PlanetCount { get {  return planetCount; } private set { planetCount = value; } }

    private void Awake()
    {
        // Reset planet count.
        planetCount = 0;

        // Create a new list of planets.
        planets = new List<Planet>();

        // Add all planets to the list. They are children of this object.
        for (int i = 0; i < transform.childCount; ++i)
        {
            Planet planet = transform.GetChild(i).gameObject.GetComponent<Planet>();
            planet.planetID = planetCount;
            planets.Add(planet);
            ++planetCount;
        }
    }
}