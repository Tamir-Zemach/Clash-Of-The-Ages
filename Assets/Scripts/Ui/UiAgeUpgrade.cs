
using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Utilities;
using Managers;
using static BackEnd.Utilities.SpriteKeys; 

namespace Ui
{
    public class UiAgeUpgrade : SceneAwareMonoBehaviour<UiAgeUpgrade>
    {

        public delegate void OnUiRefreshDelegate<TType>(List<SpriteEntries.SpriteEntry<TType>> spriteMap);
        public event OnUiRefreshDelegate<UnitType> OnUiRefreshDeployUnits;
        public event OnUiRefreshDelegate<UnitUpgradeButtonKey> OnUiRefreshUpgradeUnits;
        public event OnUiRefreshDelegate<TurretKey> OnUiRefreshTurrets;
        public event OnUiRefreshDelegate<SpecialAttackType> OnUiRefreshSpecialAttack;

        private SpritesLevelUpData _spritesLevelUpData;

        private List<SpriteEntries.SpriteEntry<UnitType>> UnitSpriteMap { get; set; }

        private List<SpriteEntries.SpriteEntry<UnitUpgradeButtonKey>> UnitUpgradeButtonSpriteMap { get; set; }

        private List<SpriteEntries.SpriteEntry<SpecialAttackType>> SpecialAttackSpriteMap { get; set; }

        private List<SpriteEntries.SpriteEntry<TurretKey>> TurretSpriteMap { get; set; }


        private void Start()
        {
            GameManager.Instance.OnAgeUpgrade += UpdateSprites;
        }

        private void UpdateSprites(List<LevelUpDataBase> upgradeDataList)
        {
            foreach (var data in upgradeDataList)
            {
                if (data is SpritesLevelUpData levelUpData)
                {
                    _spritesLevelUpData  = levelUpData;
                    UnitSpriteMap = _spritesLevelUpData.UnitSpriteMap;
                    SpecialAttackSpriteMap = _spritesLevelUpData.SpecialAttackSpriteMap;
                    UnitUpgradeButtonSpriteMap = _spritesLevelUpData.UnitUpgradeButtonSpriteMap;
                    TurretSpriteMap = _spritesLevelUpData.TurretSpriteMap;
                        
                    OnUiRefreshDeployUnits?.Invoke(UnitSpriteMap);
                    OnUiRefreshUpgradeUnits?.Invoke(UnitUpgradeButtonSpriteMap);
                    OnUiRefreshSpecialAttack?.Invoke(SpecialAttackSpriteMap);
                    OnUiRefreshTurrets?.Invoke(TurretSpriteMap);

                    break;
                }
            }
        }

        protected override void InitializeOnSceneLoad()
        {
            
        }
    }
}