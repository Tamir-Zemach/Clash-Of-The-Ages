using System;
using System.Collections.Generic;
using BackEnd.Base_Classes;
using BackEnd.Economy;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bases
{
    public class EnemyBasesManager : SceneAwareMonoBehaviour<EnemyBasesManager>
    {
        private List<EnemyBaseHealth> _enemyBases = new List<EnemyBaseHealth>();
        
        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _enemyBases = new List<EnemyBaseHealth>(FindObjectsByType<EnemyBaseHealth>(FindObjectsSortMode.None));
            InitializeBasesLocalHealth();
        }

        private void InitializeBasesLocalHealth()
        {
            var enemyHealth = EnemyHealth.Instance.MaxHealth;
            foreach (var baseHealth in _enemyBases)
            {
                baseHealth.SetLocalHealth(enemyHealth / _enemyBases.Count);
            }
        }
        

        public bool MultipleBases()
        {
            return _enemyBases.Count > 1;
        } 
    }
}