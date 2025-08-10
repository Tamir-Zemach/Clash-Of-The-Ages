using System.Collections.Generic;
using System.Linq;
using BackEnd.Data_Getters;
using BackEnd.Enums;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace BackEnd.Base_Classes
{
    public abstract class DataObject<TType> : ScriptableObject, IUpgradable<TType>
    {

        
        [FormerlySerializedAs("ageStage")]
        [Header("Identity")]
        [Tooltip("The age stage during which this Data becomes available.")]
        [SerializeField] private AgeStageType _ageStage;
        public AgeStageType AgeStage
        {
            get => _ageStage;
            set => _ageStage = value;
        }

        [FormerlySerializedAs("type")]
        [Tooltip("Specifies the type of The Data.")]
        [SerializeField] private TType _type;
        public TType Type => _type;

        [FormerlySerializedAs("isFriendly")]
        [Tooltip("Indicates whether this Data belongs to the friendly faction.")]
        [SerializeField] private bool _isFriendly;
        public bool IsFriendly => _isFriendly;

        [Header("Audio Settings")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private List<SfxEntry> _sfx;

        [FormerlySerializedAs("prefab")]
        [Header("Deployment Settings")]
        [Tooltip("Prefab to instantiate when deploying.")]
        [SerializeField] private GameObject _prefab;
        public GameObject Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }


        private void Awake()
        {
            if (GameDataRepository.Instance != null)
            {
                GameDataRepository.Instance.OnInitialized += Initialize;
            }
        }

        private void Initialize()
        {
            if (IsFriendly)
            {
                GameManager.Instance.OnAgeUpgrade += UpgradeAge;
            }
            else
            {
                EnemyAgeManager.Instance.OnAgeUpgrade += UpgradeAge;
            }
        }

        public abstract void UpgradeAge(List<LevelUpDataBase> upgradeDataList);

        public AudioClip GetSfx(SfxType type) =>
        _sfx.FirstOrDefault(entry => entry.Type == type)?.AudioClip;

        public float GetClipVolume(SfxType type) =>
        _sfx.FirstOrDefault(entry => entry.Type == type)!.Volume;

        
    }
}