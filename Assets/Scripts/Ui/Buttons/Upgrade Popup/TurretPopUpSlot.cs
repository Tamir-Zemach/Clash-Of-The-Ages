using System;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Enums;
using BackEnd.Utilities.EffectsUtil;
using turrets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui.Buttons.Upgrade_Popup
{
    public class TurretPopUpSlot : UpgradeSlotBase
    {
        public override SlotType SlotType => SlotType.TurretUpgrade;
        
        private TurretSpawnPoint _spawnPoint;

        private TurretData _turret;

        private GameObject _playerBase;
        private TurretType _turretType => TurretType.FastTurret;
        
        public TurretData TurretData => _turret;
        

        private void Awake()
        {
            _turret = GameDataRepository.Instance.FriendlyTurrets.GetData(_turretType);
            _playerBase = GameObject.FindGameObjectWithTag(_turret.FriendlyBase);

            FindValidSpawnPoint();
        }

        public void AddTurretToBase()
        {
            _spawnPoint.HasTurret = true;

            var turret = Instantiate(_turret.Prefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
            UpgradeDataStorage.Instance.RegisterTurretUpgrade();
            turret.transform.parent = _spawnPoint.transform;
            var behaviour = turret.GetComponent<TurretBaseBehavior>();

            if (behaviour)
            {
                behaviour.Initialize(_turret, _playerBase.transform);
            }
            else
            {
                Debug.LogWarning("TurretBaseBehaviour not found on spawned enemy prefab.");
            }

            FinalizeUpgrade();
        }

        private void FinalizeUpgrade()
        {
            UpgradePopup.Instance.BlockRaycasts(false);

            UIEffects.ShrinkAndDestroy(transform, 1.2f, 0, () =>
            {
                UpgradePopup.Instance.HidePopup();
            });
        }

        private void FindValidSpawnPoint()
        {
            var spawnPoints = _playerBase.GetComponentsInChildren<TurretSpawnPoint>()
                .Where(sp => !sp.HasTurret)
                .ToList();

            _spawnPoint = spawnPoints.First();
        }
        
    }
}