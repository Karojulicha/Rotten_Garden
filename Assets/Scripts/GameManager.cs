using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject BotonPausa; // Botón de pausa en la interfaz
    [SerializeField] private GameObject PausaPanel; // Panel que aparece al pausar el juego
    [SerializeField] private GameObject VictoriaPanel; // Panel de victoria
    [SerializeField] private GameObject DerrotaPanel; // Panel de derrota

    private bool GamePause = false; // Indica si el juego está pausado
    private List<Planta> plantasEnJuego = new List<Planta>(); // Lista de todas las plantas activas en la escena

    private GridManager gridManager;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        StartCoroutine(VerificarEstadoJuego()); // Inicia la verificación del estado del juego cada ciertos segundos
    }

    public void ChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene); // Cambia la escena cuando se llama a esta función
        Time.timeScale = 1;
    }

    private void Update()
    {
        // Verifica si se ha presionado la tecla "Escape" para pausar o reanudar el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePause)
                Reanudar();
            else
                Pausa();
        }
    }

    public void Pausa()
    {
        GamePause = true; // Activa la pausa
        Time.timeScale = 0f; // Detiene el tiempo en el juego
        BotonPausa.SetActive(false); // Oculta el botón de pausa
        PausaPanel.SetActive(true); // Muestra el panel de pausa
    }

    public void Reanudar()
    {
        GamePause = false; // Desactiva la pausa
        Time.timeScale = 1f; // Restaura el tiempo normal
        BotonPausa.SetActive(true); // Muestra el botón de pausa
        PausaPanel.SetActive(false); // Oculta el panel de pausa 
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f; // Restaura la velocidad del tiempo a la normalidad
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
    }


    private IEnumerator VerificarEstadoJuego()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            plantasEnJuego.Clear();
            plantasEnJuego.AddRange(FindObjectsOfType<Planta>());

            if (EsGridCompleta())
            {
                int sanas = 0, infectadas = 0;

                foreach (Planta planta in plantasEnJuego)
                {
                    if (planta.type == PlantType.healthy || planta.type == PlantType.vitalis)
                        sanas++;
                    else if (planta.type == PlantType.infected)
                        infectadas++;
                }

                if (sanas > infectadas)
                    MostrarVictoria();
                else if (infectadas > sanas)
                    MostrarDerrota();
            }
        }
    }

    private bool EsGridCompleta()
    {
        int totalCeldas = gridManager.gridSize * gridManager.gridSize;
        return plantasEnJuego.Count >= totalCeldas;
    }

    private void MostrarVictoria()
    {
        Time.timeScale = 0f; // Detiene el juego
        VictoriaPanel.SetActive(true); // Muestra el panel de victoria
    }

    private void MostrarDerrota()
    {
        Time.timeScale = 0f; // Detiene el juego
        DerrotaPanel.SetActive(true); // Muestra el panel de derrota
    }

    public void SiguienteNivel()
{
    Time.timeScale = 1f; // Asegurar que el juego siga funcionando
    int siguienteEscenaIndex = SceneManager.GetActiveScene().buildIndex + 1;

    // Verificar si hay más niveles disponibles
    if (siguienteEscenaIndex < SceneManager.sceneCountInBuildSettings)
    {
        SceneManager.LoadScene(siguienteEscenaIndex); // Cargar el siguiente nivel
    }
    else
    {
        Debug.Log("No hay más niveles disponibles."); // Mensaje si es el último nivel
    }
}
}
