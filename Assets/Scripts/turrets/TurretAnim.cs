using System;
using UnityEngine;

namespace turrets
{
    public class TurretAnim : MonoBehaviour
    {
        public AnimationClip AttackAnim;
        
        private TurretBaseBehavior  _turretBaseBehavior;
        private Animator _animator;
        private bool _seeingEnemy;
        private void Awake()
        {
            _turretBaseBehavior = GetComponentInParent<TurretBaseBehavior>();
            _animator = GetComponent<Animator>();
            
        }

        private void Start()
        {
            _animator.SetBool("SeeingEnemy", _seeingEnemy);
            _turretBaseBehavior.OnSeeingEnemy += SeeingEnemy;
            _turretBaseBehavior.OnSeeingNothing += SeeingNothing;
        }
        
        private void OnDestroy()
        {
            _turretBaseBehavior.OnSeeingEnemy -= SeeingEnemy;
            _turretBaseBehavior.OnSeeingNothing -= SeeingNothing;
        }


        private void SeeingNothing()
        {
            _seeingEnemy = false;
            _animator.SetBool("SeeingEnemy", false);
        }

  
        private void SeeingEnemy(Quaternion rotation)
        {
            _seeingEnemy = true;

            float baseAnimDuration = AttackAnim.length;
            float desiredDelay = _turretBaseBehavior.Turret.InitialAttackDelay;

            float speedMultiplier = baseAnimDuration / desiredDelay;
            _animator.SetFloat("AttackSpeed", speedMultiplier);
            _animator.SetBool("SeeingEnemy", true);
        }
        
        
    }
}