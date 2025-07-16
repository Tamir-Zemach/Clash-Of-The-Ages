
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class UnitBaseBehaviour : MonoBehaviour
{
    public delegate void AttackDelegate(GameObject target);
    public event AttackDelegate OnAttack;

    public event Action OnInitialized;

    public delegate void Spawned(Renderer renderer);
    public static event Spawned OnSpawned;

    private NavMeshAgent _agent;
    private UnitHealthManager _healthManager;
    private GameObject _enemyBase;
    private GameObject _currentTarget;
    private Animator _animator;
    private Coroutine _currentCoroutine;
    private Collider _col;
    private Renderer _renderer;

    private bool _isAttacking = false;
    private bool _isDying;


    public UnitData Unit { get; private set; }

    public bool IsAttacking => _isAttacking;

    public GameObject GetAttackTarget() => _currentTarget;

    public void Initialize(UnitData unitData)
    {
        Unit = unitData;
        _agent = GetComponent<NavMeshAgent>();
        _healthManager = GetComponent<UnitHealthManager>();
        _healthManager.OnDying += Dying;
        _enemyBase = GameObject.FindGameObjectWithTag(Unit.OppositeBaseTag);
        _agent.destination = _enemyBase.transform.position;
        _agent.speed = Unit.Speed;
        _col = GetComponent<Collider>();
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<Renderer>();
        OnInitialized?.Invoke();
        if (!Unit.IsFriendly)
        {
            OnSpawned?.Invoke(_renderer);
        }

    }

    private void Update()
    {
        if (!_isDying)
        {
            CheckForEnemyUnit();
            CheckForFriendlyUnit();
        }
    }


    private void CheckForEnemyUnit()
    {
        _agent.isStopped = false; // Default state

        if (Physics.BoxCast(transform.position,
                                Unit.boxSize,
                                transform.forward,
                                out var hitInfo,
                                Quaternion.identity,
                                Unit.Range,
                                Unit.OppositeUnitMask))
        {
            GameObject obj = hitInfo.transform.gameObject;

            if (obj.CompareTag(Unit.OppositeUnitTag) || obj.CompareTag(Unit.OppositeBaseTag))
            {
                HandleEnemyDetection(obj);
            }
        }

            ResetAttackStateIfNeeded();
    }
    private void HandleEnemyDetection(GameObject target)
    {
        _agent.isStopped = true;
        if (!_isAttacking)
        {
            _isAttacking = true;
                Attack(target);
        }
    }

    private void CheckForFriendlyUnit()
    {
        if (Physics.BoxCast(transform.position,
                            Unit.boxSize, transform.forward,
                            out var hitInfo,
                            Quaternion.identity,
                            Unit.RayLengthForFriendlyUnit))
        {
            GameObject obj = hitInfo.transform.gameObject;
            if (obj.CompareTag(Unit.FriendlyUnitTag))
            {
                _agent.isStopped = true;
            }
        }
    }


    private void ResetAttackStateIfNeeded()
    {
        if (_currentCoroutine != null && !_agent.isStopped)
        {
            StopAllCoroutines();
            _currentCoroutine = null;
            _isAttacking = false;
        }
    }
    private void Attack(GameObject target)
    {
        _currentTarget = target;
        _currentCoroutine = StartCoroutine(AttackAction(Unit.InitialAttackDelay, target));
    }

    IEnumerator AttackAction(float initialAttackDelay, GameObject target)
    {
        yield return new WaitForSeconds(initialAttackDelay);
        if (_animator == null)
        {
            OnAttack?.Invoke(target);
        }
        _isAttacking = false;
    }

    private void Dying()
    {
        _isDying = true;
        _agent.isStopped = true;
        _col.enabled = false;

    }

    private void OnDrawGizmos()
    {
        if (Unit == null) return;
        Gizmos.color = Unit.boxColor;

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        // Perform the BoxCast
        if (Physics.BoxCast(origin,Unit.boxSize , direction, out RaycastHit hitInfo, Quaternion.identity, Unit.Range))
        {
            // Draw the hit box
            Gizmos.DrawWireCube(hitInfo.point, Unit.boxSize);
        }

        // Draw the initial box
        Gizmos.DrawWireCube(origin, Unit.boxSize);

        // Draw the movement path
        Gizmos.DrawLine(origin, origin + direction * Unit.Range);

    }




}



