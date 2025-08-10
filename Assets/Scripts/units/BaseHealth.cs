using BackEnd.Economy;
using BackEnd.InterFaces;
using UnityEngine;

namespace units
{
    public class BaseHealth : MonoBehaviour, IDamageable
    {
        public bool isFriendly;


        public void GetHurt(int damage)
        {
            if (isFriendly)
            {
                PlayerHealth.Instance.SubtractHealth(damage);
                return;
            }
            else
            {
                EnemyHealth.Instance.SubtractHealth(damage);
            }

        }

    }
}
