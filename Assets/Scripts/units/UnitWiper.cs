using System;
using System.Collections.Generic;
using units.Behavior;
using static UnityEngine.Object;


namespace units
{
    public static class UnitWiper
    {
        [Obsolete("Obsolete")]
        public static void WipeAllUnits()
        {
            var units = new List<UnitBaseBehaviour>(FindObjectsOfType<UnitBaseBehaviour>());
            foreach (UnitBaseBehaviour unit in units)
            {
                Destroy(unit.gameObject);
            }
        }
        [Obsolete("Obsolete")]
        public static void WipeUnitsAligned(bool isFriendly)
        {
            var units = new List<UnitBaseBehaviour>(FindObjectsOfType<UnitBaseBehaviour>());
            foreach (UnitBaseBehaviour unit in units)
            {
                if (unit.Unit.IsFriendly == isFriendly)
                {
                    Destroy(unit.gameObject);
                }
            }
        }

    }
}