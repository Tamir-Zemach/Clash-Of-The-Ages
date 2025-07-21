using Assets.Scripts.InterFaces;
using UnityEngine;

namespace BackEnd.Data_Getters
{
    public static class UIObjectFinder 
    {
        public static TButton GetButton<TButton, TType>()
            where TButton : Component, IImgeSwichable<TType>
        {
            return Object.FindFirstObjectByType<TButton>();
        }
        public static TButtons[] GetButtons<TButtons, TType>()
            where TButtons : Component, IImgeSwichable<TType>
        {
            return Object.FindObjectsByType<TButtons>(FindObjectsSortMode.None);
        }

    }
}