
using Assets.Scripts.Backend.Data;
using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.turrets;
using Assets.Scripts.Ui.TurretButton;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Manages upgrade-related data and behavior for units, turrets, and special attacks.
/// Handles UI sprites, prefabs, upgrade costs, and the player's current special attack selection.
///</summary>
/// <remarks> 
/// To Debug: Tools => Upgrade State Manager Debugger in runtime. 
/// </remarks>

public class GameStateManager : SceneAwareMonoBehaviour<GameStateManager>
{
    #region LOCAL PARAMETERS

    private TypedObjectCache<UnitType, UnitData> _friendlyUnits = new();
    private TypedObjectCache<UnitType, UnitData> _enemyUnits = new();

    private TypedObjectCache<TurretType, TurretData> _friendlyTurrets = new();
    private TypedObjectCache<TurretType, TurretData> _enemyTurrets = new();

    private SpecialAttackData _friendlySpecialAttackData;
    private SpritesData _spritesData;


    // -- UI SPRITE STATE --

    private TypedObjectCache<UnitType, Sprite> _unitSprites = new();
    private TypedObjectCache<AgeStageType, Sprite> _specialAttackSprites = new();
    private TypedObjectCache<(TurretType, TurretButtonType), Sprite> _turretSprites = new();
    private TypedObjectCache<(UnitType, StatType), Sprite> _unitUpgradebuttonSprites = new();

    // -- COST UPGRADES --
    private TypedObjectCache<(UnitType, StatType), int> _unitStatUpgradeCost = new();
    private TypedObjectCache<UpgradeType, int> _playerStatUpgradeCost = new();
    private TypedObjectCache<UnitType, int> _deployUnitCost = new();
    private TypedObjectCache<AgeStageType, int> _specialAttackCost = new();
    private TypedObjectCache<TurretType, int> _turretCost = new();

    // -- PREFAB UPGRADES --
    private TypedObjectCache<UnitType, GameObject> _unitPrefabs = new();
    private TypedObjectCache<AgeStageType, GameObject> _specialAttackPrefabs = new();
    private TypedObjectCache<TurretType, GameObject> _turretPrefabs = new();

    #endregion


    #region Initilize

    protected override void Awake()
    {
        base.Awake();
        InisilizeDataAtAwake();
    }

    private void InisilizeDataAtAwake()
    {
        SetUnitsAtInitilize();
        SetTurretAtinitilize();
        SetSpecialAttackAtinitilize();
        SetSpritesAtInitilize();
    }
    protected override void InitializeOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        var unitDeployButtons = UIObjectFinder.GetButtons<UnitDeployButton, UnitType>();
        var unitUpgradeButtons = UIObjectFinder.GetButtons<UnitUpgradeButton, UnitType>();
        var specialAttackButton = UIObjectFinder.GetButton<SpecialAttackButton, AgeStageType>();
        var turretButton = UIObjectFinder.GetButton<TurretButton, TurretType>();

        foreach (var button in unitUpgradeButtons)
        {
            SetStatUpgradeCost(button.Type, button.StatType, button.Cost);
        }

        foreach (var button in unitDeployButtons)
        {
            var unit = GetFriendlyUnit(button.Type);
            if (unit != null)
            {
                SetDeployUnitCost(button.Type, unit.Cost);
            }
        }

        if (specialAttackButton != null && GetFriendlySpecialAttackData() != null)
        {
            SetSpecialAttackCost(specialAttackButton.Type, GetFriendlySpecialAttackData().Cost);
        }

        if (turretButton != null)
        {
            SetTurretCost(turretButton.Type, turretButton.Cost);
        }
    }

    private void SetSpritesAtInitilize()
    {
        _spritesData = InitialDataLoader.Instance.GetSpritesData();

        // -- Unit Sprites --
        foreach (var entry in _spritesData.unitSpriteMap)
        {
            var key = entry.GetKey();
            if (_unitSprites.HasObject(key))
            {
                Debug.LogWarning($"Duplicate unit sprite for {key}");
            }
            _unitSprites.SetObject(key, entry.GetSprite());
        }

        // -- Special Attack Sprites --
        foreach (var entry in _spritesData.specialAttackSpriteMap)
        {
            var key = entry.GetKey();
            if (_specialAttackSprites.HasObject(key))
            {
                Debug.LogWarning($"Duplicate special attack sprite for {entry.GetKey()}");
            }
            _specialAttackSprites.SetObject(entry.GetKey(), entry.GetSprite());
        }

        // -- Turret Sprites --
        foreach (var entry in _spritesData.turretSpriteMap)
        {
            var key = entry.GetKey();

            if (entry.Sprite == null)
            {
                Debug.LogWarning($"Skipping turret sprite for key {key} because sprite is null");
                continue;
            }

            if (_turretSprites.HasObject(key))
            {
                Debug.LogWarning($"Duplicate turret sprite for {key}. Skipping.");
                continue;
            }

            _turretSprites.SetObject(key, entry.GetSprite());
        }

        // -- Upgrade Button Sprites --
        foreach (var entry in _spritesData.unitUpgradeButtonSpriteMap)
        {
            var key = entry.GetKey();
            if (_unitUpgradebuttonSprites.HasObject(key))
            {
                Debug.LogWarning($"Duplicate upgrade button sprite for {entry.GetKey()}");
            }
            _unitUpgradebuttonSprites.SetObject(entry.GetKey(), entry.GetSprite());
        }
    }
    private void SetUnitsAtInitilize()
    {
        foreach (var unit in InitialDataLoader.Instance.GetAllFriendlyUnits())
        {
            _friendlyUnits.SetObject(unit.Type, unit);

            SetDeployUnitCost(unit.Type, unit.Cost);

            if (unit.Prefab != null)
            {
                SetUnitPrefab(unit.Type, unit.Prefab);
            }


        }

        foreach (var unit in InitialDataLoader.Instance.GetAllEnemyUnits())
        {
            _enemyUnits.SetObject(unit.Type, unit);

            if (unit.Prefab != null)
                SetUnitPrefab(unit.Type, unit.Prefab);
        }
    }
    private void SetTurretAtinitilize()
    {
        foreach (var turret in InitialDataLoader.Instance.GetAllFriendlyTurrets())
        {
            _friendlyTurrets.SetObject(turret.Type, turret);

            if (turret.Prefab != null)
            {
                SetTurretPrefab(turret.Type, turret.Prefab);
            }

        }

        foreach (var turret in InitialDataLoader.Instance.GetAllEnemyTurretsTurrets())
        {
            _enemyTurrets.SetObject(turret.Type, turret);

            if (turret.Prefab != null)
            {
                SetTurretPrefab(turret.Type, turret.Prefab);
            }
        }



    }
    private void SetSpecialAttackAtinitilize()
    {
        _friendlySpecialAttackData = InitialDataLoader.Instance.GetFriendlySpecialAttackData();
        SetSpecialAttackPrefab(_friendlySpecialAttackData.Type, _friendlySpecialAttackData.Prefab);
    }

    #endregion


    #region SETTERS

    // -- UI --

    public void SetUnitSprite(UnitType type, Sprite sprite) => _unitSprites.SetObject(type, sprite);
    public void SetSpecialAttackSprite(AgeStageType type, Sprite sprite) => _specialAttackSprites.SetObject(type, sprite);
    public void SetTurretSprite((TurretType, TurretButtonType) type, Sprite sprite) => _turretSprites.SetObject(type, sprite);
    public void SetUnitUpgradeButtonSprite((UnitType, StatType) type, Sprite sprite) => _unitUpgradebuttonSprites.SetObject(type, sprite);

    // -- COST --
    public void SetStatUpgradeCost(UnitType unitType, StatType statType, int cost) => _unitStatUpgradeCost.SetObject((unitType, statType), cost);
    public void SetPlayerStatUpgradeCost(UpgradeType type, int cost) => _playerStatUpgradeCost.SetObject(type, cost);
    public void SetDeployUnitCost(UnitType type, int cost) => _deployUnitCost.SetObject(type, cost);
    public void SetSpecialAttackCost(AgeStageType type, int cost) => _specialAttackCost.SetObject(type, cost);
    public void SetTurretCost(TurretType type, int cost) => _turretCost.SetObject(type, cost);

    // -- PREFABS --
    public void SetUnitPrefab(UnitType type, GameObject prefab) => _unitPrefabs.SetObject(type, prefab);
    public void SetSpecialAttackPrefab(AgeStageType type, GameObject prefab) => _specialAttackPrefabs.SetObject(type, prefab);
    public void SetTurretPrefab(TurretType type, GameObject prefab) => _turretPrefabs.SetObject(type, prefab);

    #endregion


    #region GETTERS


    public UnitData GetFriendlyUnit(UnitType type) => _friendlyUnits.TryGetObject(type, out var data) ? data : null;
    public UnitData GetEnemyUnit(UnitType type) => _enemyUnits.TryGetObject(type, out var data) ? data : null;

    public TurretData GetFriendlyTurret(TurretType type) => _friendlyTurrets.TryGetObject(type, out var data) ? data : null;
    public TurretData GetEnemyTurret(TurretType type) => _friendlyTurrets.TryGetObject(type, out var data) ? data : null;


    public List<UnitData> GetAllFriendlyUnits() => _friendlyUnits.GetAll().Values.ToList();
    public List<UnitData> GetAllEnemyUnits() => _enemyUnits.GetAll().Values.ToList();

    public List<TurretData> GetAllFriendlyTurrets() => _friendlyTurrets.GetAll().Values.ToList();
    public List<TurretData> GetAllEnemyTurrets() => _enemyTurrets.GetAll().Values.ToList();

    public SpecialAttackData GetFriendlySpecialAttackData() => _friendlySpecialAttackData;


    // -- UI --
    public Sprite GetUnitSprite(UnitType type) => _unitSprites.GetObject(type);   
    public Sprite GetSpecialAttackSprite(AgeStageType type) => _specialAttackSprites.GetObject(type);
    public Sprite GetTurretSprite((TurretType, TurretButtonType) type) => _turretSprites.GetObject(type);
    public Sprite GetUnitUpgradebuttonSpritesSprite((UnitType, StatType) type) => _unitUpgradebuttonSprites.GetObject(type);

    // -- COST --
    public int GetStatUpgradeCost(UnitType unitType, StatType statType) => _unitStatUpgradeCost.GetObject((unitType, statType));
    public int GetPlayerStatCost(UpgradeType type) => _playerStatUpgradeCost.GetObject(type);

    // -- PREFABS --
    public GameObject GetUnitPrefab(UnitType type) => _unitPrefabs.GetObject(type);
    public GameObject GetTurretPrefab(TurretType type) => _turretPrefabs.GetObject(type);
    public GameObject GetSpecialAttackPrefab() => _specialAttackPrefabs.GetObject(_friendlySpecialAttackData.Type);

    // -- DEBUG GETTERS -- 
    public TypedObjectCache<(UnitType, StatType), int> GetAllUnitStatUpgradeCosts() => _unitStatUpgradeCost;
    public TypedObjectCache<UpgradeType, int> GetAllPlayerUpgradeCosts() => _playerStatUpgradeCost;
    public TypedObjectCache<UnitType, GameObject> GetAllUnitPrefabs() => _unitPrefabs;
    public TypedObjectCache<AgeStageType, GameObject> GetAllSpecialAttackPrefabs() => _specialAttackPrefabs;
    public TypedObjectCache<TurretType, GameObject> GetAllTurretPrefabs() => _turretPrefabs;

    #endregion
}