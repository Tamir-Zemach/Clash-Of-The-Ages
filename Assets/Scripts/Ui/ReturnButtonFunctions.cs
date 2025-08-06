using BackEnd.Utilities;
using Managers;
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
        }
        
        
        
    }
}
