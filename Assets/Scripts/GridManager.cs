using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject floorPrefabTwo;

    public GameObject grassFloorPrefab;
    private Vector3 floorSize;
    public int gridSize;
    private List<GameObject> floorTerrain = new List<GameObject>();
    void Start()
    {
        floorSize = floorPrefab.GetComponent<Renderer>().bounds.size;
        GenerationGridTerrain();
    }

    void GenerationGridTerrain()
    {
        foreach (GameObject floor in floorTerrain)
            floor.SetActive(false);


        // create the terrain grid, with matrix in x y z
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // interpolates the prefab selection
                GameObject selectedPrefab = (x + z) % 2 == 0 ? floorPrefab : floorPrefabTwo;
                // instance the floor
                GameObject floor = GetInstantiateFloor(selectedPrefab);
                // adjust the position according to the size
                floor.transform.position = new Vector3(x * floorSize.x, 0, z * floorSize.z);

                floor.SetActive(true);

                if (floor.GetComponent<PlantSpawner>() == null)
                {
                    floor.AddComponent<PlantSpawner>();
                }


                // Instance the border floor, checking which zone is on the edge
                if (x == 0 || x == gridSize - 1 || z == 0 || z == gridSize - 1)
                {
                    GameObject borderFloor = GetInstantiateFloor(grassFloorPrefab);
                    borderFloor.transform.position = new Vector3(x * floorSize.x, 0, z * floorSize.z);
                    borderFloor.SetActive(true);
                }

            }
        }
    }

    GameObject GetInstantiateFloor(GameObject prefabFloor)
    {
        foreach (GameObject floor in floorTerrain)
        {
            if (!floor.activeInHierarchy && floor.name == prefabFloor.name)
            {
                return floor;
            }
        }
        GameObject newFloor = Instantiate(prefabFloor);
        newFloor.name = prefabFloor.name;
        floorTerrain.Add(newFloor);
        return newFloor;
    }
}
