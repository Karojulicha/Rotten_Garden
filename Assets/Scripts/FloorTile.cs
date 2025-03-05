using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public GameObject borderPrefab; 
    private float tileSize; 

    void Start()
    {
        // bounds genera un cuadro imaginario 
        // calcula el ancho del componente 
        tileSize = GetComponentInChildren<Renderer>().bounds.size.x;
        CheckAndPlaceBorders();
    }

    void CheckAndPlaceBorders()
    {
        // Direcciones de los vectores (izquierda, derecha, arriba, abajo)
        Vector3[] directions = {
            Vector3.left,  // (-x)
            Vector3.right, // (+x)
            Vector3.forward, // (+z)
            Vector3.back // (-z)
        };

        foreach (Vector3 direction in directions)
        {
            Vector3 checkPosition = transform.position + (direction.normalized * tileSize);

            // Verificar si hay otro tile en esa posición
            Collider[] colliders = Physics.OverlapSphere(checkPosition, 0.1f);
            bool hasNeighbor = false;

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Floor")) // Si tiene un vecino con el tag "Floor"
                {
                    hasNeighbor = true;
                    break;
                }
            }

            // Si NO hay vecino, instancia un borde en esa dirección
            if (!hasNeighbor)
            {
                Instantiate(borderPrefab, checkPosition, Quaternion.identity);
            }
        }
    }
}
