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

   public int maxInfectadas = 5; // Cantidad de plantas infectadas necesarias para perder
   public int minSanasParaGanar = 5; // Cantidad de plantas sanas necesarias para ganar

   private void Start()
   {
      ActualizarListaPlantas(); // Carga la lista de plantas al iniciar el juego
      StartCoroutine(VerificarEstadoJuego()); // Inicia la verificación del estado del juego cada ciertos segundos
      AudioManager.Instance.PlayMusic();
   }

   public void ChangeScene(string nameScene)
   {
      SceneManager.LoadScene(nameScene); // Cambia la escena cuando se llama a esta función
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
      AudioManager.Instance.StopMusic();
   }

   public void Reanudar()
   {
      GamePause = false; // Desactiva la pausa
      Time.timeScale = 1f; // Restaura el tiempo normal
      BotonPausa.SetActive(true); // Muestra el botón de pausa
      PausaPanel.SetActive(false); // Oculta el panel de pausa
      AudioManager.Instance.PlayMusic();
   }

   public void Reiniciar()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual para reiniciar el juego
   }

   private void ActualizarListaPlantas()
   {
      plantasEnJuego.Clear(); // Vacía la lista antes de actualizar
      plantasEnJuego.AddRange(FindObjectsOfType<Planta>()); // Encuentra todas las plantas en la escena y las agrega a la lista
   }

   private IEnumerator VerificarEstadoJuego()
   {
      while (true)
      {
         yield return new WaitForSeconds(2f); // Espera 2 segundos antes de cada verificación

         ActualizarListaPlantas(); // Obtiene el estado actual de las plantas en el juego

         int sanas = 0, infectadas = 0; // Contadores de plantas sanas e infectadas

         // Recorre todas las plantas en el juego y cuenta cuántas hay de cada tipo
         foreach (Planta planta in plantasEnJuego)
         {
            if (planta.type == PlantType.healthy)
               sanas++;
            else if (planta.type == PlantType.infected)
               infectadas++;
         }

         // Si el número de plantas infectadas supera el límite, se pierde el juego
         if (infectadas >= maxInfectadas)
         {
            MostrarDerrota();
            yield break; // Detiene la verificación del estado del juego
         }

         // Si el número de plantas sanas alcanza el mínimo necesario, se gana el juego
         if (sanas >= minSanasParaGanar)
         {
            MostrarVictoria();
            yield break; // Detiene la verificación del estado del juego
         }
      }
   }

   private void MostrarVictoria()
   {
      Time.timeScale = 0f; // Detiene el juego
      VictoriaPanel.SetActive(true); // Muestra el panel de victoria
      Debug.Log("¡Has ganado!"); // Mensaje en la consola para depuración
   }

   private void MostrarDerrota()
   {
      Time.timeScale = 0f; // Detiene el juego
      DerrotaPanel.SetActive(true); // Muestra el panel de derrota
      Debug.Log("Has perdido."); // Mensaje en la consola para depuración
   }
}
