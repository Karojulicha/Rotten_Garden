using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public void ChangeScene(string nameScene)
   {
    SceneManager.LoadScene(nameScene);
   }
}
