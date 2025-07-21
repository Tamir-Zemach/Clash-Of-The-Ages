using System;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Data__ScriptableOBj_;
using UnityEngine;

namespace Special_Attacks
{
    public class SpecialAttackBaseBehavior  : MonoBehaviour
    {
        [SerializeField, Tooltip("Total duration (in seconds) that the meteor shower will last. Can be desimal number.")]
        private float _timeOfTheSpecialAttack;
        
        private SpecialAttackSpawnPos _specialAttackSpawnPos;
        private float _timer;

        public SpecialAttackData SpecialAttack { get; private set; }

        public void Initialize(SpecialAttackData specialAttackData, SpecialAttackSpawnPos spawnPos)
        {
            SpecialAttack = specialAttackData;
            _specialAttackSpawnPos  = spawnPos;
            _timer = 0;
        }
        
        private void Update()
        {
            SpecialAttackDuration();
        }
        
        private void SpecialAttackDuration()
        {
            _timer += Time.deltaTime;
            if (!(_timer >= _timeOfTheSpecialAttack)) return;
            StopAllCoroutines();
            _specialAttackSpawnPos.IsSpecialAttackAccruing = false;
            Destroy(gameObject);
        }
        
        
    }
}