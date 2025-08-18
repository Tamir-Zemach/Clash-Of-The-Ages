using System;
using BackEnd.Enums;
using BackEnd.Data__ScriptableOBj_;
using Managers;
using UnityEngine;

namespace Special_Attacks
{
    public class SpecialAttackBaseBehavior  : MonoBehaviour
    {
        [SerializeField, Tooltip("Total duration (in seconds) that the meteor shower will last. Can be desimal number.")]
        private float _timeOfTheSpecialAttack;
        
        private MeteorRainSpawnPos _meteorRainSpawnPos;
        private float _timer;

        public SpecialAttackData SpecialAttack { get; private set; }

        public void Initialize(SpecialAttackData specialAttackData, MeteorRainSpawnPos spawnPos)
        {
            SpecialAttack = specialAttackData;
            _meteorRainSpawnPos  = spawnPos;
            _timer = 0;
        }
        
        private void Update()
        {
            if (!GameStates.Instance.GameIsPlaying) return;
            SpecialAttackDuration();
        }
        
        private void SpecialAttackDuration()
        {
            _timer += Time.deltaTime;
            if (!(_timer >= _timeOfTheSpecialAttack)) return;
            StopAllCoroutines();
            _meteorRainSpawnPos.ClearSpecialAttack();
            Destroy(gameObject);
        }
        
        
    }
}