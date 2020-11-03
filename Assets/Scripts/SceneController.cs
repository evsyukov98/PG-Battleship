using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleShip
{
    
    public class SceneController : MonoBehaviour
    {

        public static IEnumerator RestartGame(int timer)
        {
            yield return new WaitForSeconds(timer);

            SceneManager.LoadScene(0);
        }

        public static void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}