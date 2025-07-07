
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;


namespace Assets.Scripts.units
{
    [CreateAssetMenu(fileName = "LevelUpData", menuName = "LevelUpData", order = 3)]
    public class UnitLevelUpData : ScriptableObject, IUpgradable<UnitType>
    {
        [Tooltip("The unit type:")]
        [SerializeField] private UnitType _unitType;

        [SerializeField] private bool _isFriendly;

        [Tooltip("What age this upgrade belongs to")]
        [SerializeField] private AgeStageType _ageStage;

        public UnitType Type => _unitType;

        public bool IsFriendly => _isFriendly;

        public int AgeStage => (int)_ageStage;

        [Tooltip("The prefab to instansiate when deplyed")]
        [SerializeField] private GameObject _unitPrefab;
        public GameObject Prefab => _unitPrefab;

        public void SetPrefab(GameObject prefab)
        {
            _unitPrefab = prefab;
        }
        public void SetType(UnitType unitType)
        {
            _unitType = unitType;
        }


        [Header("Enemy Unit Properties")]
        [Tooltip("How much money the player gains when this Unit is Destroyed")]
        public int _moneyWhenKilled;

        [Header("Unit Parameters")]
        [Tooltip("How far the unit detects opposite unit")]
        public int _range;
        [Tooltip("The speed Of the Unit")]
        public float _speed = 1;
        [Tooltip("The Health Of the Unit")]
        public int _health = 1;
        [Tooltip("How much every Hit will give damage")]
        public int _strength = 1;
        [Tooltip("Percentage reduction applied to initial attack delay after upgrade.\n" +
                 "For example, a value of 20 reduces the delay by 20%.")]
        public float AttackDelayReductionPercent = 1;

    }
}

