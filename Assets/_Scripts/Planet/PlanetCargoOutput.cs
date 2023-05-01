using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCargoOutput : MonoBehaviour
{
    private Planet parentPlanet;

    private void Awake()
    {
        parentPlanet = GetComponentInParent<Planet>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.parent.GetComponentInChildren<Player>();

            // Check if the player has any cargo to deliver to this planet.
            if (player.CargoCount > 0)
            {
                for (int i = 0; i < player.CargoList.Length; ++i)
                {
                    Debug.Log("I: " + i);

                    if (player.CargoList[i] != null)
                    {
                        Debug.Log("index " + i + " has cargo for planet: " + player.CargoList[i].destinationPlanetID);

                        // If the player has cargo to deliver to this planet, remove the cargo from the player's cargo list.
                        if (player.CargoList[i].destinationPlanetID == parentPlanet.planetID)
                        {
                            player.RemoveCargo(i);
                            Debug.Log("Cargo Delivered");
                        }
                    }
                }
            }

            // Generate a random cargo this is not from the current planet and add the cargo to the player's cargo list.
            player.AddCargo(CargoManager.Instance.GenerateRandomCargo(parentPlanet.planetID));
        }
    }
}