using System;
using UnityEngine;

namespace Special_Attacks
{
    public class MeteorRainSpawnPos : MonoBehaviour
    {
        public static event Action OnMeteorRainAccruing;
        public static event Action OnMeteorRainEnding;

        public void ApplySpecialAttack()
        {
            OnMeteorRainAccruing?.Invoke();
        }

        public void ClearSpecialAttack()
        {
            OnMeteorRainEnding?.Invoke();
        }


    }
}
