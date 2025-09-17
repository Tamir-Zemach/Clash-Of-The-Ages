using BackEnd.Data__ScriptableOBj_;
using JetBrains.Annotations;
using Ui.Buttons.Deploy_Button;

namespace BackEnd.Structs
{
        public struct Deployment
        {
            public UnitData Unit;
            [CanBeNull] public Lane Lane;

            public Deployment(UnitData unit, Lane lane = null)
            {
                Unit = unit;
                Lane = lane;
            }
        }
}