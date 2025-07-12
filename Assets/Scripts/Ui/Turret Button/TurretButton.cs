
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using Assets.Scripts.turrets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static SpritesLevelUpData;
using static UnityEditor.U2D.ScriptablePacker;

namespace Assets.Scripts.Ui.TurretButton
{
    public partial class TurretButton : MonoBehaviour , IImgeSwichable<TurretType>
    {
        [Tooltip("The type of action this button triggers.")]
        [SerializeField] private TurretButtonType _turretButtonType;

        [Tooltip("The type of action this button triggers.")]
        [SerializeField] private TurretType _turretType;

        [Tooltip("Cost for triggering this action.")]
        [SerializeField] private int _cost;

        [Tooltip("Refund granted when selling a turret.")]
        [SerializeField] private int _moneyToGiveBack;

        [HideInInspector] private List<TurretSpawnPoint> _turretSpawnPoints = new();

        private TurretData _turret;
        private Image _image;
        private bool _isWaitingForClick;

        private Dictionary<TurretButtonType, Func<TurretSpawnPoint, bool>> _conditions;

        public TurretType Type => _turretType;
        public TurretButtonType ButtonType => _turretButtonType;

        public int Cost => _cost;

        public Image Image => _image;

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }


        private void Awake()
        {
            _conditions = new Dictionary<TurretButtonType, Func<TurretSpawnPoint, bool>>
            {
               { TurretButtonType.DeployTurret, spawnPoint => !spawnPoint.HasTurret && spawnPoint.IsUnlocked },
               { TurretButtonType.AddSlot, spawnPoint => !spawnPoint.IsUnlocked },
               { TurretButtonType.SellTurret, spawnPoint => spawnPoint.HasTurret }
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
            UIRootManager.Instance.OnSceneChanged += GetAllSpawnPoints;
            GameManager.Instance.OnAgeUpgrade += UpdateSprite;
            GetAllSpawnPoints();
        }



        public void UpdateSprite(List<LevelUpDataBase> upgradeDataList)
        {
            foreach (var data in upgradeDataList)
            {
                if (data is SpritesLevelUpData levelUpData)
                {
                    _image.sprite = levelUpData.GetSpriteFromList(new TurretKey 
                    { 
                         ButtonType = _turretButtonType , TurretType = _turretType
                    }, 
                    levelUpData.turretSpriteMap);
                }
            }
        }


        private void GetAllSpawnPoints()
        {
            _turretSpawnPoints = FindObjectsByType<TurretSpawnPoint>(FindObjectsSortMode.None).ToList();
        }


#if UNITY_EDITOR
        public static class FieldNames
        {
            public const string TurretButtonType = nameof(_turretButtonType);
            public const string TurretType = nameof(_turretType);
            public const string Cost = nameof(_cost);
            public const string Refund = nameof(_moneyToGiveBack);
        }
#endif
    }
}
