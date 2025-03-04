using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject BotonPausa;
   [SerializeField] private GameObject PausaPanel;
   private bool GamePause = false;

   public void ChangeScene(string nameScene)
   {
    SceneManager.LoadScene(nameScene);
   }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
         if(GamePause){
            Reanudar();
         }else{
            Pausa();
         }
        }
    }
    public void Pausa()
   {
      GamePause = true;
      Time.timeScale = 0f;
      BotonPausa.SetActive(false);
      PausaPanel.SetActive(true);
   }

   public void Reanudar()
   {
      GamePause = false;
      Time.timeScale = 1f;
      BotonPausa.SetActive(true);
      PausaPanel.SetActive(false);
   }

   public void Reiniciar()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }
   
}
