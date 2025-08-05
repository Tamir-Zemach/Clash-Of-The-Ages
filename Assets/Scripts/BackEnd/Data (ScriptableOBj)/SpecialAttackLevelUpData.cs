using BackEnd.Enums;
using Assets.Scripts.InterFaces;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "SpecialAttackLevelUpData", menuName = "SpecialAttackLevelUpData", order = 5)]
    public class SpecialAttackLevelUpData : LevelUpDataBase
    {

        [Header("Unit Identity")]
        [Tooltip("Specifies the type of unit.")]
        [field: SerializeField] public SpecialAttackType Type { get; private set; }

        [Tooltip("Indicates whether this unit belongs to the friendly faction.")]
        [field: SerializeField] public bool IsFriendly { get; private set; }


        [Header("Deployment Settings")]
        [Tooltip("Prefab to instantiate when deploying this unit.")]
        [field: SerializeField] public GameObject Prefab { get; private set; }

    }
}