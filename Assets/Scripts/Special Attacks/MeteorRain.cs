using System.Collections;
using UnityEngine;

public class MeteorRain : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab of the meteor object to be spawned during the attack.")]
    private GameObject _meteorPrefab;

    [SerializeField, Tooltip("The defined area within which meteors will randomly spawn.")]
    private Collider _specialAttackArea;

    [SerializeField, Tooltip("Strength For Meteor")]
    private int _meteorSterngth;

    [SerializeField, Tooltip("Total duration (in seconds) that the meteor shower will last. Can be desimal number.")]
    private float _timeOfTheSpecialAttack;

    [SerializeField, Tooltip("Minimum delay (in seconds) between consecutive meteor spawns. Can be desimal number.")]
    private float _minSpawnTime;

    [SerializeField, Tooltip("Maximum delay (in seconds) between consecutive meteor spawns. Can be desimal number.")]
    private float _maxSpawnTime;

    private float _timer;

    private SpecialAttackSpawnPos _specialAttackSpawnPos;

    private void Awake()
    {
        _specialAttackSpawnPos = FindAnyObjectByType<SpecialAttackSpawnPos>();
         StartCoroutine(SpawnCycle());
    }

    private void Update()
    {
        MeteorRainDuretion();
    }

    private Vector3 RandomSpawnPoint()
    {
        Vector3 areaMin = _specialAttackArea.bounds.min;
        Vector3 areaMax = _specialAttackArea.bounds.max;

        Vector3 spawnPos = new Vector3(
            Random.Range(areaMin.x, areaMax.x),
            Random.Range(areaMin.y, areaMax.y),
            Random.Range(areaMin.z, areaMax.z)
        );
        return spawnPos;
    }
    private float RandomSpawnTime(float minSpawnTime, float maxSpawntime)
    {
        float randomSpawnTime;
        return randomSpawnTime = Random.Range(minSpawnTime, maxSpawntime);
    }

    private IEnumerator SpawnCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(RandomSpawnTime(_minSpawnTime, _maxSpawnTime));
            SpawnMeteorAtRandomPos();
        }
    }
    private void SpawnMeteorAtRandomPos()
    {
        GameObject meteorReference = Instantiate(_meteorPrefab, RandomSpawnPoint(), Quaternion.identity);

        Meteor meteorScript = meteorReference.GetComponent<Meteor>();
        if (meteorScript != null)
        {
            meteorScript.SetStrength(_meteorSterngth);
        }
    }


    private void MeteorRainDuretion()
    {
        _timer += Time.deltaTime;
        if (_timer >= _timeOfTheSpecialAttack )
        {
            StopAllCoroutines();
            _specialAttackSpawnPos.IsSpecialAttackAccruing = false;
            Destroy(gameObject);
        }
    }

}


