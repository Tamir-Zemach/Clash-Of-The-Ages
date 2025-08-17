using BackEnd.Economy;
using BackEnd.InterFaces;
using UnityEngine;

namespace Bases
{
    public class FriendlyBaseHealth : MonoBehaviour, IDamageable
    {
        public void GetHurt(int damage)
        {
            PlayerHealth.Instance.SubtractHealth(damage);
        }

    }
}
