using Assets.Scripts.Backend.Data;
using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.turrets;
using Assets.Scripts.units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InitialDataLoader : PersistentMonoBehaviour<InitialDataLoader>
{
    private Dictionary<UnitType, UnitData> _friendlyUnits;
    private Dictionary<UnitType, UnitData> _enemyUnits;

    private Dictionary<TurretType, TurretData> _friendlyTurrets = new();
    private Dictionary<TurretType, TurretData> _enemyTurrets = new();

    private List<UnitLevelUpData> _unitLevelUpData;
    private List<TurretLevelUpData> _turretLevelUpData;

    private SpecialAttackData _friendlySpecialAttack;
    private SpritesData _spriteData;



    private SpecialAttackLevelUpData _specialAttackLevelUpData;
    private SpritesLevelUpData _spritesLevelUpData;

    protected override void Awake()
    {
        base.Awake();
        LoadAll();
    }

    public void LoadAll( )
    {
        LoadData();
        LoadLevelUpData();
    }

    private void LoadData()
    {
        // Instantiate special attack
        var specialRaw = Resources.LoadAll<SpecialAttackData>("").FirstOrDefault();
        if (specialRaw != null) _friendlySpecialAttack = Instantiate(specialRaw);

        // Instantiate sprites data
        var spriteRaw = Resources.LoadAll<SpritesData>("").FirstOrDefault();
        if (spriteRaw != null) _spriteData = Instantiate(spriteRaw);

        LoadTurretData();
        LoadUnitData(); 
    }
    private void LoadUnitData()
    {
        _friendlyUnits = new Dictionary<UnitType, UnitData>();
        _enemyUnits = new Dictionary<UnitType, UnitData>();

        foreach (var unit in Resources.LoadAll<UnitData>(""))
        {
            var clone = Instantiate(unit);
            if (unit.IsFriendly)
                _friendlyUnits[unit.Type] = clone;
            else
                _enemyUnits[unit.Type] = clone;
        }
    }
    private void LoadTurretData()
    {
        _friendlyTurrets = new Dictionary<TurretType, TurretData>();
        _enemyTurrets = new Dictionary<TurretType, TurretData>();

        foreach (var turret in Resources.LoadAll<TurretData>(""))
        {
            var clone = Instantiate(turret);
            if (turret.IsFriendly)
                _friendlyTurrets[turret.Type] = clone;
            else
                _enemyTurrets[turret.Type] = clone;
        }
    }


    private void LoadLevelUpData()
    {
        _unitLevelUpData = Resources.LoadAll<UnitLevelUpData>("").ToList();

        _specialAttackLevelUpData = Resources.LoadAll<SpecialAttackLevelUpData>("").FirstOrDefault();

        _turretLevelUpData = Resources.LoadAll<TurretLevelUpData>("").ToList();

        _spritesLevelUpData = Resources.LoadAll<SpritesLevelUpData>("").FirstOrDefault();
    }


    public List<UnitData> GetAllFriendlyUnits() => _friendlyUnits?.Values.ToList();
    public List<UnitData> GetAllEnemyUnits() => _enemyUnits?.Values.ToList();

    public List<TurretData> GetAllFriendlyTurrets() => _friendlyTurrets?.Values.ToList();
    public List<TurretData> GetAllEnemyTurretsTurrets() => _enemyTurrets?.Values.ToList();

    public List<UnitLevelUpData> GetUnitLevelUpData() => _unitLevelUpData;
    public List<TurretLevelUpData> GetTurretLevelUpData() => _turretLevelUpData;
    public SpecialAttackLevelUpData GetSpecialAttackLevelUpData() => _specialAttackLevelUpData;
    public SpritesLevelUpData GetSpritesLevelUpData() => _spritesLevelUpData;


    public SpecialAttackData GetFriendlySpecialAttackData() => _friendlySpecialAttack;
    public SpritesData GetSpritesData() => _spriteData;

}