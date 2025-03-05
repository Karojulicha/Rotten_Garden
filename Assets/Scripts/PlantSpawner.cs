using UnityEngine;
public class PlantSpawner : MonoBehaviour
{
    public GameObject healthyPlantPrefab; // Prefab para todas las plantas 

    private bool hasPlant = false; // Para evitar colocar más de una planta en la misma casilla

    void OnMouseDown()
    {
        if (!hasPlant) // Solo permite activar una planta por casilla
        {
            PlantType type = GetRandomPlantType(); // Determina el tipo de planta
            GameObject plant = Instantiate(healthyPlantPrefab, transform.position, Quaternion.identity);
            hasPlant = true;

            Planta plantScript = plant.GetComponent<Planta>();
            if (plantScript != null)
            {
                plantScript.type = type;
                plantScript.ApplyEffect();
            }
        }
    }

    PlantType GetRandomPlantType() // Cantidad de plantas segun el tipo.
    {
        float randomValue = Random.value * 100;

        if (randomValue < 50) return PlantType.healthy;
        else if (randomValue < 60) return PlantType.vitalis;
        else return PlantType.infected;
    }
}
