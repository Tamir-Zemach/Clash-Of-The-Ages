
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;


namespace Assets.Scripts.units
{
    [CreateAssetMenu(fileName = "LevelUpData", menuName = "LevelUpData", order = 3)]
    public class UnitLevelUpData : LevelUpDataBase
    {
        [Header("Unit Identity")]
        [Tooltip("Specifies the type of unit.")]
        [field: SerializeField] public UnitType Type { get; private set; }

        [Tooltip("Indicates whether this unit belongs to the friendly faction.")]
        [field: SerializeField] public bool IsFriendly { get; private set; }


        [Header("Deployment Settings")]
        [Tooltip("Prefab to instantiate when deploying this unit.")]
        [field: SerializeField] public GameObject Prefab { get; private set; }

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

