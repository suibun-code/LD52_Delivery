using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    [SerializeField] private PlanetManager planetManager;

    public static CargoManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public Cargo GenerateRandomCargo(int planetID)
    {
        Cargo cargo = ScriptableObject.CreateInstance<Cargo>();

        // Generate a random planet ID that is not the current planet.
        int randomPlanetID = Random.Range(0, PlanetManager.PlanetCount);

        while (randomPlanetID == planetID)
            randomPlanetID = Random.Range(0, PlanetManager.PlanetCount);

        cargo.destinationPlanetID = randomPlanetID;
        cargo.cargoColor = planetManager.Planets[randomPlanetID].PlanetCargoColor;

        return cargo;
    }
}
