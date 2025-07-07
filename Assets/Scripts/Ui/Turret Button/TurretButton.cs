using Assets.Scripts.Enems;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.InterFaces;
using UnityEngine.UI;
using Assets.Scripts.turrets;
using Assets.Scripts.Backend.Data;

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

        [Tooltip("Parent GameObject that contains all TurretSpawnPoint children.")]
        [SerializeField] private GameObject _turretSpawnPointsParent;

        [HideInInspector] private List<TurretSpawnPoint> _turretSpawnPoints = new();

        private Transform _turretSpawnPos;
        private bool _isWaitingForClick;
        private Sprite _sprite;
        private Image _image;
        private TurretData _turret;

        private Dictionary<TurretButtonType, Func<TurretSpawnPoint, bool>> _conditions;

        public TurretType Type => _turretType;
        public TurretButtonType ButtonType => _turretButtonType;

        public Sprite Sprite => _sprite;

        public Image Image => _image;

        public int Cost => _cost;

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
            GetAllFriendlyTurretSpawnPoints();
        }

        private void Start()
        {
            GetData();
        }
        private void GetData()
        {
            _turret = GameStateManager.Instance.GetFriendlyTurret(_turretType);
            _sprite = GameStateManager.Instance.GetTurretSprite((_turretType, _turretButtonType));
            _image = GetComponent<Image>();
            _image.sprite = _sprite;
        }


        private void GetAllFriendlyTurretSpawnPoints()
        {
            _turretSpawnPoints.Clear();

            if (_turretSpawnPointsParent == null)
            {
                Debug.LogWarning($"{nameof(TurretButton)}: Turret spawn points parent is not assigned.");
                return;
            }

            _turretSpawnPoints.AddRange(_turretSpawnPointsParent.GetComponentsInChildren<TurretSpawnPoint>());
        }


#if UNITY_EDITOR
        public static class FieldNames
        {
            public const string TurretButtonType = nameof(_turretButtonType);
            public const string TurretType = nameof(_turretType);
            public const string Cost = nameof(_cost);
            public const string Refund = nameof(_moneyToGiveBack);
            public const string SpawnPointParent = nameof(_turretSpawnPointsParent);
        }
#endif
    }
}
