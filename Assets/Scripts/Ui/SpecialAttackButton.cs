using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.BackEnd.Enems;
using Assets.Scripts.InterFaces;
using BackEnd.Data__ScriptableOBj_;
using Special_Attacks;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class SpecialAttackButton : MonoBehaviour, IImgeSwichable<SpecialAttackType>
    {
        [field: SerializeField] public SpecialAttackType Type {  get; private set; }

        [SerializeField] private int _cost;

        private SpecialAttackSpawnPos _specialAttackSpawnPos;

        private SpecialAttackData _specialAttack;

        private Image _image;


        private void Start()
        {
            GetData();
        }

        private void GetData()
        {
            _specialAttack = GameDataRepository.Instance.FriendlySpecialAttacks.GetData(Type);
            _image = GetComponent<Image>();
            LevelLoader.Instance.OnSceneChanged += GetSpawnPos;
            GameManager.Instance.OnAgeUpgrade += UpdateSprite;
            GetSpawnPos(); 
        }

        private void GetSpawnPos()
        {
            _specialAttackSpawnPos = FindAnyObjectByType<SpecialAttackSpawnPos>(); 
        }

        private void UpdateSprite(List<LevelUpDataBase> upgradeDataList)
        {
            foreach (var data in upgradeDataList)
            {
                if (data is SpritesLevelUpData levelUpData)
                {
                    _image.sprite = levelUpData.GetSpriteFromList(Type, levelUpData.SpecialAttackSpriteMap);
                }
            }
        }

        public void PerformSpecialAttack()
        {
            if (!PlayerCurrency.Instance.HasEnoughMoney(_cost) || _specialAttackSpawnPos.IsSpecialAttackAccruing) return;
            PlayerCurrency.Instance.SubtractMoney(_cost);
            _specialAttackSpawnPos.IsSpecialAttackAccruing = true;
            SpawnSpecialAttack();
        }

        private void SpawnSpecialAttack()
        {
            var specialAttack = Instantiate(_specialAttack.Prefab, _specialAttackSpawnPos.transform.position, _specialAttackSpawnPos.transform.rotation);
            var behaviour = specialAttack.GetComponent<SpecialAttackBaseBehavior>();

            if (behaviour != null)
            {
                behaviour.Initialize(_specialAttack, _specialAttackSpawnPos);
            }
            else
            {
                Debug.LogWarning("SpecialAttackBaseBehaviour not found on spawned enemy prefab.");
            }

        }

    
    
    }
}
