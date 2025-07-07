
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class DeployManager : SceneAwareMonoBehaviour<DeployManager>
{
    public static event Action OnQueueChanged;

    public readonly Queue<UnitData> _unitQueue = new Queue<UnitData>();
    private bool isDeploying = false;


    [Tooltip("The spawn area where the friendly unit will not spawn if another one is already present.")]
    [SerializeField, TagSelector] private string _spawnArea;

    [Tooltip("The player base Tag:")]
    [SerializeField, TagSelector] private string _baseTag;

    private SpawnArea SpawnArea;
    private Transform _unitSpawnPoint;
    private UnitData nextCharacter;
    private GameObject unitReference;

    private float timer;

    protected override void Awake()
    {
        base.Awake();
    }

    private void ResetQueueState()
    {
        nextCharacter = null;
        isDeploying = false;
        timer = 0;
        _unitQueue.Clear();
    }
    protected override void InitializeOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        ResetQueueState();
        GameObject areaGO = GameObject.FindGameObjectWithTag(_spawnArea);
        if (areaGO == null)
        {
            Debug.LogWarning($"[DeployManager] No GameObject with tag '{_spawnArea}' found in scene.");
            return;
        }

        SpawnArea = areaGO.GetComponent<SpawnArea>();
        if (SpawnArea == null)
        {
            Debug.LogWarning($"[DeployManager] GameObject tagged '{_spawnArea}' is missing SpawnArea component.");
            return;
        }

        _unitSpawnPoint = SpawnArea.GetComponentInParent<Transform>();
        if (_unitSpawnPoint == null)
        {
            Debug.LogWarning("[DeployManager] Failed to locate parent transform for spawn point.");
        }

    }

    private void Update()
    {
        if (nextCharacter != null)
            HandleDelayedDeployment();
    }



    /// <summary>
    /// Adds the specified unit to the deployment queue.
    /// If no deployment is currently in progress, starts deploying immediately.
    /// </summary>
    /// <param name="unit">The UnitData to queue for deployment.</param>
    public void AddUnitToDeploymentQueue(UnitData unit)
    {
        _unitQueue.Enqueue(unit);

        //Event invoke for Ui purpses 
        OnQueueChanged?.Invoke();

        // If no deployment is currently happening, start the queue process
        if (!isDeploying)
        {
            ProcessNextUnitInQueue();
        }
    }

    /// <summary>
    /// Retrieves the next unit from the queue and marks deployment as in progress.
    /// If the queue is empty, resets deployment state.
    /// </summary>

    private void ProcessNextUnitInQueue()
    {
        if (_unitQueue.Count > 0)
        {
            //remove unit from queue
            nextCharacter = _unitQueue.Dequeue();

            //Event invoke for Ui purpses 
            OnQueueChanged?.Invoke();

            isDeploying = true;
        }
        else
        {
            nextCharacter = null;
            isDeploying = false;
        }
    }



    /// <summary>
    /// Handles unit instantiation after a delay.
    /// Waits for sufficient time and an available spawn area before deploying.
    /// Resets timer and triggers the next deployment if available.
    /// </summary>

    private void HandleDelayedDeployment()
    {
        timer += Time.deltaTime;
        if (timer >= nextCharacter.DeployDelayTime && !SpawnArea._hasUnitInside)
        {
            unitReference = Instantiate(nextCharacter.Prefab, _unitSpawnPoint.position, _unitSpawnPoint.rotation);

            if (unitReference.TryGetComponent(out UnitBaseBehaviour behaviour))
            {
                behaviour.Initialize(nextCharacter);
            }

            timer = 0;
            isDeploying = false;
            ProcessNextUnitInQueue();
        }
    }


}




