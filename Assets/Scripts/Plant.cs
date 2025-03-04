using UnityEngine;

// Enum para definir los tipos de plantas posibles
public enum PlantType { healthy, infected, vitalis }

public class Planta : MonoBehaviour
{
    public PlantType type;
    public GameObject plantPrefab; // Prefab de la planta
    public ParticleSystem infectedParticles; // Partículas para plantas infectadas
    public ParticleSystem vitalisParticles; // Partículas para plantas vitalis

    public static int level = 1; // Nivel del juego 
    private bool hasSpawned = false; // Evita múltiples clones

    void Start()
    {
        // Buscar automáticamente las partículas en los hijos 
        if (infectedParticles == null)
            infectedParticles = transform.Find("InfectedParticles")?.GetComponent<ParticleSystem>();

        if (vitalisParticles == null)
            vitalisParticles = transform.Find("VitalisParticles")?.GetComponent<ParticleSystem>();

        ApplyEffect(); // Aplica efectos según el tipo de planta 

        // Solo clona otra planta si es sana o infectada
        if ((type == PlantType.healthy || type == PlantType.infected) && !hasSpawned)
        {
            hasSpawned = true; // Evita múltiples clones
            SpawnAnotherPlant();
        }
    }

    void ApplyEffect()
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
        Vector3 spawnPosition = transform.position + new Vector3(1f, 0, 0);

        GameObject newPlant = Instantiate(plantPrefab, spawnPosition, Quaternion.identity);
        Planta plantScript = newPlant.GetComponent<Planta>();

        if (plantScript != null)
        {
            plantScript.type = GetRandomPlantType(); // Asigna tipo basado en probabilidades
            plantScript.ApplyEffect();
        }
    }

    PlantType GetRandomPlantType()
    {
        float randomValue = Random.value * 100; // Valor entre 0 y 100

        float healthyChance = 50;
        float vitalisChance = 30;
        float infectedChance = 20;

        if (randomValue < healthyChance)
            return PlantType.healthy;
        else if (randomValue < healthyChance + vitalisChance)
            return PlantType.vitalis;
        else
            return PlantType.infected;
    }
}
