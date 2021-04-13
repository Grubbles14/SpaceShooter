using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private float _ySpawn = 7.0f;
    private bool _stopSpawning = false;
    private bool _spawning = false;

    [SerializeField]
    private GameObject[] _powerupList;
    [SerializeField]
    private GameObject[] _negPickupList;
    [SerializeField]
    private GameObject[] _specialPowerupList;


    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnNegativePickupRoutine());
        _spawning = true;
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2);
        while (!_stopSpawning)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9, 10), _ySpawn, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        int t = Random.Range(2, 6);
        yield return new WaitForSeconds(t);
        while (!_stopSpawning)
        {
            int d = Random.Range(1, 11);
            Debug.Log(d.ToString());
            if (d == 3)
            {
                //spawn random special powerup. 1 in 10 chance of spawning special.
                int randomPowerup = Random.Range(0, _specialPowerupList.Length);
                Instantiate(_specialPowerupList[randomPowerup], new Vector3(Random.Range(-9, 10), _ySpawn, 0), Quaternion.identity);
            }
            else
            {
                int randomPowerup = Random.Range(0, _powerupList.Length);
                Instantiate(_powerupList[randomPowerup], new Vector3(Random.Range(-9, 10), _ySpawn, 0), Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(6, 10));
        }
    }

        IEnumerator SpawnNegativePickupRoutine()
    {
        int t = Random.Range(2, 6);
        yield return new WaitForSeconds(t);
        while (!_stopSpawning)
        {
            int randomPowerup = Random.Range(0, _negPickupList.Length);
            Instantiate(_negPickupList[randomPowerup], new Vector3(Random.Range(-9, 10), _ySpawn, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(9, 13));
        }
    }

    public void StopSpawn()
    {
        _stopSpawning = true;
    }

    public bool GetSpawnStatus()
    {
        return _spawning;
    }
}
