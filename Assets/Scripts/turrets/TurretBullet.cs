using Assets.Scripts.Enems;
using Assets.Scripts.turrets;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class TurretBullet : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField, TagSelector] private string _groundTag;
    [SerializeField] private float _destroyTime;
    [SerializeField] TurretType _turretType;
    private bool _hasHit;
    private float _timer;
    private TurretData _turretData;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _turretData = GameStateManager.Instance.GetFriendlyTurret(_turretType);
    }
    private void Start()
    {
        ApplyForceAtStart();
    }
    private void ApplyForceAtStart()
    {
        _rb.AddForce(transform.forward * _turretData.BulletSpeed, ForceMode.Impulse);
        _rb.AddForce(transform.up * 2, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        //check for hit one time
        if (_hasHit) return;

        if (other.gameObject.CompareTag(_turretData.OppositeUnitTag))
        {
            _hasHit = true;
            GiveDamage(other.gameObject);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag(_groundTag))
        {
            Destroy(gameObject);
        }
    }
    private void GiveDamage(GameObject target)
    {
        UnitHealthManager targetHealth = target.GetComponent<UnitHealthManager>();
        if (targetHealth != null)
        {
            targetHealth.GetHurt(_turretData.BulletStrength);
        }
    }
    private void Update()
    {
        DestroyAfterCertainTime();
    }
    private void DestroyAfterCertainTime()
    {
        _timer += Time.deltaTime;
        if (_timer >= _destroyTime)
        {
            Destroy(gameObject);
        }
    }

}
