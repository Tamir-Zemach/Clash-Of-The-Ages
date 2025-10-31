using System.Collections.Generic;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using Managers.Loaders;
using Managers.Spawners;
using Ui.Buttons.Deploy_Button;
using units.Behavior;
using UnityEngine;

namespace Managers
{
    public class LaneManager : SceneAwareMonoBehaviour<LaneManager>
    {
        private List<Lane> _lanes = new List<Lane>();
        
        private Dictionary<Lane, List<UnitBaseBehaviour>> _unitsOnLane = new();
        
        
        public List<Lane> Lanes => _lanes;
        protected override void Awake()
        {
            base.Awake();
            UnitSpawner.OnUnitDeployedOnLane += (lane, unit) => RegisterUnitToLane(unit, lane);
        }

        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _lanes.Clear();
            _lanes.AddRange(FindObjectsByType<Lane>(FindObjectsSortMode.None)); 
            SubscribeToLanes();
        }

        private void RegisterUnitToLane(UnitBaseBehaviour unit , Lane lane = null)
        {
            // Fallback to single lane if lane is null
            if (lane == null && _lanes.Count == 1)
            {
                var defaultLane = _lanes[0];
                lane = defaultLane;
            }
            
            if (lane == null)
            {
                return;
            }
            
            // If the lane is not yet tracked in the dictionary, initialize its entry
            if (lane != null && !_unitsOnLane.ContainsKey(lane))
            {
                _unitsOnLane[lane] = new List<UnitBaseBehaviour>();
            }

            // Add the unit to the list of units associated with this lane
            _unitsOnLane[lane].Add(unit);

            // Subscribe to the unit's OnDying event using a named local function
            unit.OnDying += OnUnitDied;

            // Local function to handle unit death
            // It removes the unit from the lane and unsubscribes itself from the event
            void OnUnitDied()
            {
                UnregisterUnitFromLane(lane, unit);     // Remove unit from tracking
                unit.OnDying -= OnUnitDied;             // Unsubscribe to prevent memory leaks
            }
            
        }

        private void UnregisterUnitFromLane(Lane lane, UnitBaseBehaviour unit)
        {
            if (_unitsOnLane.ContainsKey(lane))
            {
                _unitsOnLane[lane].Remove(unit);
            }
        }

        private void SubscribeToLanes()
        {
            foreach (var lane in _lanes)
            {
                lane.OnLaneDestroyed += WipeUnitsOnLane;
            }
        }

        private void WipeUnitsOnLane(Lane lane)
        {
            if (!_unitsOnLane.ContainsKey(lane)) return;

            var units = _unitsOnLane[lane];
            foreach (var unit in units)
            {
                if (unit != null)
                {
                    
                    Destroy(unit.gameObject);
                }
            }

            _unitsOnLane.Remove(lane); // Clean up the dictionary
        }

        public void StartFlashingAllLanes()
        {
            foreach (var lane in _lanes)
            {
                lane.StartFlashing(0.2f);
            }
        }
        public void StopFlashingAllLanes()
        {
            foreach (var lane in _lanes)
            {
                lane.StopFlashing();
            }
        }
    }
}