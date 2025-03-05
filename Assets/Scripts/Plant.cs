using UnityEngine;
using System.Collections.Generic;

public enum PlantType { healthy, infected, vitalis }

public class Planta : MonoBehaviour
{
    public PlantType type;
    public GameObject plantPrefab; // Prefab de la planta
    public ParticleSystem infectedParticles;
    public ParticleSystem vitalisParticles;
    private int plantLayer;

    public int level = 2; // Nivel del juego
    private bool hasSpawned = false; // Evita múltiples clones al mismo tiempo
    public static int totalClones = 0; // Lleva la cuenta total de clones
    public static int maxClones = 1; // Máximo de plantas permitidas


    void Start()
    {
        plantLayer = LayerMask.GetMask("Plant");
        // Buscar automáticamente las partículas en los hijos
        if (infectedParticles == null)
            infectedParticles = transform.Find("InfectedParticles")?.GetComponent<ParticleSystem>();

        if (vitalisParticles == null)
            vitalisParticles = transform.Find("VitalisParticles")?.GetComponent<ParticleSystem>();

        ApplyEffect(); // Aplica efectos según el tipo de planta 

        // Solo clona otra planta si es sana o infectada
        if (type == PlantType.infected || type == PlantType.healthy)
        {
            float delay = Random.Range(2f, 5f); // Retraso aleatorio para naturalidad
            Invoke("SpawnAnotherPlant", delay);
        }
    }

    public void ApplyEffect()
    {
        if (infectedParticles != null) infectedParticles.Stop();
        if (vitalisParticles != null) vitalisParticles.Stop();

        switch (type)
        {
            case PlantType.infected:
                infectedParticles?.Play();
                Debug.Log("Planta infectada activada en " + gameObject.name);
                break;

            case PlantType.vitalis:
                vitalisParticles?.Play();
                Debug.Log("Planta vitalis activada en " + gameObject.name);
                break;
        }
    }

    void SpawnAnotherPlant()
    {
        {
            if (hasSpawned || totalClones >= maxClones) return; // Evita demasiados clones

            Vector3 newPosition = transform.position + new Vector3(2f, 0, 0);
            Debug.DrawRay(newPosition, Vector3.up * 10, Color.red, 10f);
            Collider[] colliders = Physics.OverlapSphere(newPosition, 0.2f, plantLayer);
            if (colliders.Length == 0) // Verifica espacio libre
            {
                GameObject newPlant = Instantiate(plantPrefab, newPosition, Quaternion.identity);
                Planta newPlantScript = newPlant.GetComponent<Planta>();
                hasSpawned = true;
                totalClones++;
                Debug.Log($"Planta clonada. Total: {totalClones}");
            }
            else
            {
                Debug.Log("No se clonó porque el espacio está ocupado.");
            }
        }

    }

}
