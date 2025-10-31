using BackEnd.Base_Classes;
using units.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Animations
{
    
    [RequireComponent(typeof(UnitAnimationActions))]
    public class UnitAnimationController : InGameObject
    {
        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitHealthManager _unitHealthManager;
        private NavMeshAgent _agent;
        private Animator _animator;

        private void Awake()
        {
            _unitBaseBehaviour = GetComponentInParent<UnitBaseBehaviour>();
            _unitHealthManager = GetComponentInParent<UnitHealthManager>();
            _agent = GetComponentInParent<NavMeshAgent>();
            _animator = GetComponent<Animator>();

            if (_unitHealthManager != null)
                _unitHealthManager.OnDying += PlayDeathAnimation;

            if (_unitBaseBehaviour != null)
                _unitBaseBehaviour.OnInitialized += ConfigureAttackAnimation;
        }

        private void Update()
        {
            if (_animator == null || _unitBaseBehaviour == null) return;

            _animator.SetBool("isAttacking", _unitBaseBehaviour.IsAttacking);
            _animator.SetBool("isIdle", !_unitBaseBehaviour.IsAttacking && _agent.isStopped);
        }

        private void ConfigureAttackAnimation()
        {
            if (_animator == null || _unitBaseBehaviour?.Unit == null) return;

            float desiredDuration = _unitBaseBehaviour.Unit.InitialAttackDelay;
            float speedMultiplier = desiredDuration > 0 ? 1f / desiredDuration : 1f;

            _animator.SetFloat("AttackSpeedMultiplier", speedMultiplier);
        }

        private void PlayDeathAnimation()
        {
            _animator.SetTrigger("isDying");
        }

        public override void HandlePause()
        {
            if (_animator != null)
                _animator.speed = 0f;
        }

        public override void HandleResume()
        {
            if (_animator != null)
                _animator.speed = 1f;
        }

        public override void HandleGameEnd()
        {
            if (_animator == null) return;

            _animator.SetBool("isIdle", true);
            _animator.SetBool("isAttacking", false);
        }
    }
}