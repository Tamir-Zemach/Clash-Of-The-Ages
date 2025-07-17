using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [Tooltip("The layer of the Friendly Units")]
    [SerializeField] private LayerMask _friendlyUnitLayer;
    private BoxCollider _boxCollider;
    public bool _hasUnitInside {  get; private set; }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        _hasUnitInside = Physics.CheckBox(transform.TransformPoint(_boxCollider.center), _boxCollider.size , _boxCollider.transform.rotation, _friendlyUnitLayer);
    }


}
