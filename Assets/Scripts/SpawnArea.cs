using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The area that needs to be cleard in order for deployment 
/// </summary>
public class SpawnArea : MonoBehaviour
{
    
    [Tooltip("The layer of the Friendly Units")]
    [SerializeField] private LayerMask _friendlyUnitLayer;
    private BoxCollider _boxCollider;
    public bool HasUnitInside {  get; private set; }
    
    [field: SerializeField] public bool IsFriendly{  get; private set; } 

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        HasUnitInside = Physics.CheckBox(transform.TransformPoint(_boxCollider.center), _boxCollider.size , _boxCollider.transform.rotation, _friendlyUnitLayer);
    }
    


}
