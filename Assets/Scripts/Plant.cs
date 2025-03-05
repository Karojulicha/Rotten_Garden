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

    public int level = 1; // Nivel del juego
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
                if (!vitalisParticles.isPlaying)
                    vitalisParticles?.Play();
                Debug.Log("Planta vitalis activada en " + gameObject.name);
                PurifyNearbyPlants(); // Llama la particula
                break;

            case PlantType.healthy:
                Debug.Log("Planta sana, sin partículas.");
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
    void PurifyNearbyPlants()
    {
        float purifyRadius = 3f; // Define el radio de purificación
        Collider[] nearbyPlants = Physics.OverlapSphere(transform.position, purifyRadius);
        bool hasPurified = false; // se sabe si purificó alguna planta

        foreach (Collider collider in nearbyPlants)
        {
            Planta otherPlant = collider.GetComponent<Planta>();
            if (otherPlant != null && otherPlant.type == PlantType.infected)
            {
                otherPlant.type = PlantType.healthy; // Convierte en planta sana
                otherPlant.ApplyEffect();
                Debug.Log($"Planta {otherPlant.gameObject.name} purificada.");
                hasPurified = true;
            }
        }

        if (hasPurified)
        {
            // Esperar un breve momento antes de detener las partículas 
            StartCoroutine(StopVitalisParticles());
            Debug.Log($"{gameObject.name} ha purificado y ahora es una planta sana.");
        }
    }

    private System.Collections.IEnumerator StopVitalisParticles()
    {
        yield return new WaitForSeconds(1f); // Espera 1 segundo antes de detener partículas
        if (type == PlantType.vitalis)
        {
            type = PlantType.healthy; // Convertir en planta sana después de purificar
            vitalisParticles?.Stop();
            ApplyEffect();
        }
    }
}