using System;
using BackEnd.Base_Classes;
using UnityEngine;

namespace turrets
{
    public class TurretAnim : InGameObject
    {
        public AnimationClip AttackAnim;
        
        private TurretBaseBehavior  _turretBaseBehavior;
        private Animator _animator;
        private bool _seeingEnemy;
        private float _defaultAnimSpeed;
        private void Awake()
        {
            _turretBaseBehavior = GetComponentInParent<TurretBaseBehavior>();
            _animator = GetComponent<Animator>();
            _defaultAnimSpeed = _animator.speed;

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

        #region GameLifeCycle

        public override void HandlePause()
        {
          _animator.speed = 0;
        }

        public override void HandleResume()
        {
            _animator.speed = _defaultAnimSpeed;
        }

        public override void HandleGameEnd()
        {
        }

        #endregion

    }
}