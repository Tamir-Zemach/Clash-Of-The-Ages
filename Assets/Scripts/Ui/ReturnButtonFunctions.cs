using BackEnd.Utilities;
using Managers;
using Managers.Loaders;
using UnityEditor;
using UnityEngine;

namespace Ui
{
    public class ReturnButtonFunctions : MonoBehaviour
    {
        public void CloseGame()
        {
            Application.Quit();
        }
        
        
        public void BackToStartMenu()
        {
            LevelLoader.Instance.LoadMainMenu();
            GameStates.Instance.ResetGameState();
        }
        
        
        
    }
}
