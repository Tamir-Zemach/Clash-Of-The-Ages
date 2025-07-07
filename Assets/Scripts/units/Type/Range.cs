using UnityEngine;

[RequireComponent(typeof(UnitBaseBehaviour))]
public class Range : MonoBehaviour
{
    private UnitBaseBehaviour UnitBaseBehaviour;
    private UnitData unit;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;

    private void Awake()
    {
        UnitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
        if (UnitBaseBehaviour != null)
        {
            UnitBaseBehaviour.OnAttack += Attack;
        }
    }
    private void Start()
    {
        unit = UnitBaseBehaviour.Unit;
    }


    public void FireProjectile(GameObject target)
    {
        if (target == null) return;

        var instance = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        var bulletScript = instance.GetComponent<RangeBullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(target.transform, unit.Strength);
        }

        target = null; // clear after shot
    }

    private void Attack(GameObject target)
    {
        var instance = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        var bulletScript = instance.GetComponent<RangeBullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(target.transform, unit.Strength);
        }
    }

}
