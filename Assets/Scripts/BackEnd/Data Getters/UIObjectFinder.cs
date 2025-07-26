using Assets.Scripts.InterFaces;
using BackEnd.InterFaces;
using Ui;
using UnityEngine;

namespace BackEnd.Data_Getters
{
    public static class UIObjectFinder 
    {
        public static TButton GetButton<TButton, TType>()
            where TButton : Component, IImageSwitchable<TType> , ICostable
        {
            return Object.FindFirstObjectByType<TButton>();
        }
        public static TButtons[] GetButtons<TButtons, TType>()
            where TButtons : Component, IImageSwitchable<TType>, ICostable
        {
            return Object.FindObjectsByType<TButtons>(FindObjectsSortMode.None);
        }
        
        
        public static TButton[] GetButtonsInParent<TButton, TType>(Transform parent)
            where TButton : Component,IImageSwitchable<TType>, ICostable
        {
            return parent.GetComponentsInChildren<TButton>(includeInactive: true);
        }

        public static Canvas GetInGameCanvas()
        {
            return Object.FindAnyObjectByType<UIRootManager>().GetComponent<Canvas>();
        }
        
    }
}