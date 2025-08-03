using System.Collections.Generic;
using BackEnd.Base_Classes;
using units.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Animations
{
    [RequireComponent(typeof(Billboard))]
    [RequireComponent(typeof(UnitAnimationActions))]

    public class UnitAnimationController : InGameObject
    {
        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitHealthManager _unitHealthManager;
        private NavMeshAgent _agent;

        private Animator _animator;


        [FormerlySerializedAs("overrideControllerTemplate")] [SerializeField] private AnimatorOverrideController _overrideControllerTemplate;

        private static AnimationClip _cachedIdle;
        private static AnimationClip _cachedWalk;
        private static AnimationClip _cachedAttack;
        private static AnimationClip _cachedDying;

        private AnimationClip _idleClipPlaceHolder;
        private AnimationClip _walkClipPlaceHolder;
        private AnimationClip _attackClipPlaceHolder;
        private AnimationClip _deathClipPlaceHolder;

        // These are the ones unique per unit

        [FormerlySerializedAs("customIdleClip")] [SerializeField] private AnimationClip _customIdleClip;
        [FormerlySerializedAs("customWalkClip")] [SerializeField] private AnimationClip _customWalkClip;
        [FormerlySerializedAs("customAttackClip")] [SerializeField] private AnimationClip _customAttackClip;
        [FormerlySerializedAs("customDeathClip")] [SerializeField] private AnimationClip _customDeathClip;


        private void Awake()
        {
            GetAllRelevantComponents();
            GetAllAnimationsPlaceHolders();
            ApplyAnimationOverrides();
            if (_unitHealthManager != null)
            {
                _unitHealthManager.OnDying += PlayDeathAnimation;
            }
            if (_unitBaseBehaviour != null)
            {
                _unitBaseBehaviour.OnInitialized += ConfigureAttackAnimation;
            }
        }

        private void GetAllRelevantComponents()
        {
            _unitBaseBehaviour = GetComponentInParent<UnitBaseBehaviour>();
            _unitHealthManager = GetComponentInParent<UnitHealthManager>();
            _agent = GetComponentInParent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }
        /// <summary>
        /// Configures animation-related logic, such as syncing attack speed to unit stats.
        /// This should be called after animation overrides have been applied.
        /// </summary>
        private void ConfigureAttackAnimation()
        {
            if (_animator == null || _customAttackClip == null || _unitBaseBehaviour?.Unit == null)
            {
                Debug.LogWarning("Missing references for animation setup.");
                return;
            }

            // Use the custom attack clip length to calculate animation speed
            float attackDuration = _customAttackClip.length;
            float desiredDuration = _unitBaseBehaviour.Unit.InitialAttackDelay;

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
            var newOverride = new AnimatorOverrideController(_overrideControllerTemplate);

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
                    overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(_attackClipPlaceHolder, _customAttackClip);
                }
                else if (overrides[i].Key == _walkClipPlaceHolder)
                {
                    // Replace the default 'Walk' clip with this unit’s unique walk animation
                    overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(_walkClipPlaceHolder, _customWalkClip);
                }
                else if (overrides[i].Key == _deathClipPlaceHolder)
                {
                    // Replace the default 'Walk' clip with this unit’s unique walk animation
                    overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(_deathClipPlaceHolder, _customDeathClip);
                }
                else if (overrides[i].Key == _idleClipPlaceHolder)
                {
                    // Replace the default 'idle' clip with this unit’s unique walk animation
                    overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(_idleClipPlaceHolder, _customIdleClip);
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
            _animator.SetBool("isAttacking", _unitBaseBehaviour.IsAttacking);
            _animator.SetBool("isIdle", IdleState());
        
        }

        private void PlayDeathAnimation()
        {
            _animator.SetTrigger("isDying");
        }

        private bool IdleState()
        {
            return !_unitBaseBehaviour.IsAttacking && _agent.isStopped;
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
        
        


        protected override void HandlePause()
        {
            _animator.speed = 0f;
        }

        protected override void HandleResume()
        {
            _animator.speed = 1f;
        }

        protected override void HandleGameEnd()
        {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isAttacking", false);
        }

        protected override void HandleGameReset()
        {
            _animator.speed = 1f;
            _animator.ResetTrigger("isDying");
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isAttacking", false);
        }
    }
}
