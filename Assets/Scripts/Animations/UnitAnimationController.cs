
using System.Collections.Generic;
using System.ComponentModel;
using Animations;
using Assets.Scripts;
using units.Behavior;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Billboard))]
[RequireComponent(typeof(UnitAnimationActions))]

public class UnitAnimationController : MonoBehaviour
{
    private UnitBaseBehaviour UnitBaseBehaviour;
    private UnitHealthManager UnitHealthManager;
    private NavMeshAgent _agent;

    private Animator _animator;


    [SerializeField] private AnimatorOverrideController overrideControllerTemplate;

    private static AnimationClip _cachedIdle;
    private static AnimationClip _cachedWalk;
    private static AnimationClip _cachedAttack;
    private static AnimationClip _cachedDying;

    private AnimationClip _idleClipPlaceHolder;
    private AnimationClip _walkClipPlaceHolder;
    private AnimationClip _attackClipPlaceHolder;
    private AnimationClip _deathClipPlaceHolder;

    // These are the ones unique per unit

    [SerializeField] private AnimationClip customIdleClip;
    [SerializeField] private AnimationClip customWalkClip;
    [SerializeField] private AnimationClip customAttackClip;
    [SerializeField] private AnimationClip customDeathClip;


    private void Awake()
    {
        GetAllReleventComponents();
        GetAllAnimationsPlaceHolders();
        ApplyAnimationOverrides();
        if (UnitHealthManager != null)
        {
            UnitHealthManager.OnDying += PlayDeathAnimation;
        }
        if (UnitBaseBehaviour != null)
        {
            UnitBaseBehaviour.OnInitialized += ConfigureAttackAnimation;
        }
    }

    private void GetAllReleventComponents()
    {
        UnitBaseBehaviour = GetComponentInParent<UnitBaseBehaviour>();
        UnitHealthManager = GetComponentInParent<UnitHealthManager>();
        _agent = GetComponentInParent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    /// <summary>
    /// Configures animation-related logic, such as syncing attack speed to unit stats.
    /// This should be called after animation overrides have been applied.
    /// </summary>
    private void ConfigureAttackAnimation()
    {
        if (_animator == null || customAttackClip == null || UnitBaseBehaviour?.Unit == null)
        {
            Debug.LogWarning("Missing references for animation setup.");
            return;
        }

        // Use the custom attack clip length to calculate animation speed
        float attackDuration = customAttackClip.length;
        float desiredDuration = UnitBaseBehaviour.Unit.InitialAttackDelay;

        // Speed = how many times the animation needs to play per second to match desired timing
        float speedMultiplier = attackDuration > 0 ? attackDuration / desiredDuration : 1f;

        // Assign the calculated speed multiplier to the Animator parameter
        _animator.SetFloat("AttackSpeedMultiplier", speedMultiplier);
    }
    /// <summary>
    /// Applies unit-specific animation clips by overriding the default clips defined 
    /// in the Animator Override Controller. This allows different units to share the same 
    /// Animator Controller structure while using their own animations.
    /// </summary>
    private void ApplyAnimationOverrides()
    {
        // Create a fresh instance of the override controller so we don't affect the shared template
        var newOverride = new AnimatorOverrideController(overrideControllerTemplate);

        // Create a list to store override mappings (original clip → replacement clip)
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        // Populate the list with the current overrides from the template
        newOverride.GetOverrides(overrides);

        // Loop through all base clips and swap in the custom ones for this unit
        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key == _attackClipPlaceHolder)
            {
                // Replace the default 'Attack' clip with this unit’s unique attack animation
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(_attackClipPlaceHolder, customAttackClip);
            }
            else if (overrides[i].Key == _walkClipPlaceHolder)
            {
                // Replace the default 'Walk' clip with this unit’s unique walk animation
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(_walkClipPlaceHolder, customWalkClip);
            }
            else if (overrides[i].Key == _deathClipPlaceHolder)
            {
                // Replace the default 'Walk' clip with this unit’s unique walk animation
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(_deathClipPlaceHolder, customDeathClip);
            }
            else if (overrides[i].Key == _idleClipPlaceHolder)
            {
                // Replace the default 'idle' clip with this unit’s unique walk animation
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(_idleClipPlaceHolder, customIdleClip);
            }
        }
        // Apply the updated overrides to the controller
        newOverride.ApplyOverrides(overrides);

        // Assign the customized override controller to the unit's animator
        _animator.runtimeAnimatorController = newOverride;
    }
    private void Update()
    {
        Animations();
    }
    private void Animations()
    {
        _animator.SetBool("isAttacking", UnitBaseBehaviour.IsAttacking);
        _animator.SetBool("isIdle", IdleState());
        
    }

    private void PlayDeathAnimation()
    {
        _animator.SetTrigger("isDying");
    }

    private bool IdleState()
    {
        return !UnitBaseBehaviour.IsAttacking && _agent.isStopped;
    }

    private void GetAllAnimationsPlaceHolders()
    {
        if (_cachedIdle == null)
               _cachedIdle = Resources.Load<AnimationClip>("animations/Idle_PlaceHolder");

        if (_cachedWalk == null)
            _cachedWalk = Resources.Load<AnimationClip>("animations/Walk_PlaceHolder");

        if (_cachedAttack == null)
            _cachedAttack = Resources.Load<AnimationClip>("animations/Attack_PlaceHolder");

        if (_cachedDying == null)
            _cachedDying = Resources.Load<AnimationClip>("animations/Death_PlaceHolder");

        _idleClipPlaceHolder = _cachedIdle;
        _walkClipPlaceHolder = _cachedWalk;
        _attackClipPlaceHolder = _cachedAttack;
        _deathClipPlaceHolder = _cachedDying;

        if (_idleClipPlaceHolder == null)
            Debug.LogWarning("Missing 'idle_PlaceHolder' animation in Resources.");

        if (_walkClipPlaceHolder == null)
            Debug.LogWarning("Missing 'Walk_PlaceHolder' animation in Resources.");

        if (_attackClipPlaceHolder == null)
            Debug.LogWarning("Missing 'Attack_PlaceHolder' animation in Resources.");

        if (_deathClipPlaceHolder == null)
            Debug.LogWarning("Missing 'Death_PlaceHolder' animation in Resources.");
    }

}
