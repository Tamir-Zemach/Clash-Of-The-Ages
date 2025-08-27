using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Enums;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using Managers;
using turrets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static BackEnd.Utilities.SpriteKeys; 

namespace Ui.Buttons.Turret_Button
{
    public partial class TurretButton : ButtonWithCost , IImageSwitchable<TurretType>
    {
        [Tooltip("The type of action this button triggers.")]
        [SerializeField] private TurretButtonType _turretButtonType;

        [Tooltip("The type of action this button triggers.")]
        [SerializeField] private TurretType _turretType;
        


        [FormerlySerializedAs("_overLay")] [SerializeField] private CanvasGroup _canvasGroupToFade;

        private List<TurretSpawnPoint> _turretSpawnPoints = new();

        private TurretData _turret;
        private Image _image;
        private GameObject _playerBase;

        private Dictionary<TurretButtonType, Func<TurretSpawnPoint, bool>> _conditions;

        public TurretType Type => _turretType;
        
        
        private void Awake()
        {
            _conditions = new Dictionary<TurretButtonType, Func<TurretSpawnPoint, bool>>
            {
               { TurretButtonType.DeployTurret, spawnPoint => ! spawnPoint.HasTurret && spawnPoint.IsUnlocked },
               { TurretButtonType.AddSlot, spawnPoint => !spawnPoint.IsUnlocked },
               { TurretButtonType.SellTurret, spawnPoint => spawnPoint.HasTurret}
            };

        }

        private void Start()
        {
            GetData();
        }
        private void GetData()
        {
            _turret = GameDataRepository.Instance.FriendlyTurrets.GetData(_turretType);
            _image = GetComponent<Image>();
            LevelLoader.Instance.OnSceneChanged += GetAllFriendlyTurretSpawnPoints;
            UiAgeUpgrade.Instance.OnUiRefreshTurrets += UpdateSprite;
            GetAllFriendlyTurretSpawnPoints();
            if (_turretButtonType is TurretButtonType.AddSlot or TurretButtonType.SellTurret) return;
            _playerBase = GameObject.FindGameObjectWithTag(_turret.FriendlyBase);
        }

        private void UpdateSprite(List<SpriteEntries.SpriteEntry<TurretKey>> spriteMap)
        {
            foreach (var s in spriteMap)
            {
                var key = s.GetKey();

                if (key.ButtonType == _turretButtonType && key.TurretType == _turretType)
                {
                    var newSprite = s.GetSprite();
                    if (_image != null && newSprite != null)
                    {
                        _image.sprite = newSprite;
                    }
                    break; 
                }
            }
        }
        
        

        private void GetAllFriendlyTurretSpawnPoints()
        {
            _turretSpawnPoints = FindObjectsByType<TurretSpawnPoint>(FindObjectsSortMode.None)
                                    .Where(spawnPoint => spawnPoint.IsFriendly)
                                    .ToList();
        }
        
#if UNITY_EDITOR
        public static class FieldNames
        {
            public const string TurretButtonType = nameof(_turretButtonType);
            public const string TurretType = nameof(_turretType);
            public const string OverLay = nameof(_canvasGroupToFade);
        }
#endif
        
    }
}
