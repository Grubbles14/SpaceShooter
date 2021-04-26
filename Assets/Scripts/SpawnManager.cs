using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _bossPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _healthPrefab;
    [SerializeField]
    private GameObject _ammoPrefab;
    private float _ySpawn = 7.0f;
    private bool _stopSpawning = false;
    private bool _spawning = false;

    [SerializeField]
    private GameObject[] _powerupList;
    [SerializeField]
    private GameObject[] _negPickupList;
    [SerializeField]
    private GameObject[] _specialPowerupList;

    private int _currWave = 0;
    [SerializeField]
    private int[] _waveEnemyCounters = { 10, 15, 25 };
    private UIManager _uiManager;


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }
    }

    public void StartSpawning()
    {
        _currWave++;
        _stopSpawning = false;
        _uiManager.UpdateWaveCount(_currWave);
        StopAllCoroutines();
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnHealthAmmoRoutine());
        StartCoroutine(SpawnNegativePickupRoutine());
        _spawning = true;
    }


    IEnumerator SpawnEnemyRoutine()
    {
        int numEnemies = _waveEnemyCounters[_currWave - 1];
        yield return new WaitForSeconds(Random.Range(2,4));
        while (!_stopSpawning)
        {
            for (int i = 1; i <= numEnemies; i++)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9, 10), _ySpawn, 0), Quaternion.identity);
                Debug.Log("Spawning enemy number: " + i + " of wave: " + _currWave);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(2.5f);
            }
            _stopSpawning = true;
            _spawning = false;
        }
        if (_currWave == _waveEnemyCounters.Length - 1)
        {
            //logic to spawn boss goes here
            _uiManager.StartWaveTimer(10, true);
            yield return new WaitForSeconds(10);
            Instantiate(_bossPrefab);
        }
        else
        {
            _uiManager.StartWaveTimer(10, false);
            yield return new WaitForSeconds(10);
            StartSpawning();
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

    IEnumerator SpawnHealthAmmoRoutine()
    {
        int t = Random.Range(2, 6);
        yield return new WaitForSeconds(t);
        while (!_stopSpawning)
        {
            int d = Random.Range(1, 6);
            if (d == 2)
                Instantiate(_healthPrefab, new Vector3(Random.Range(-9, 10), _ySpawn, 0), Quaternion.identity);
            else
                Instantiate(_ammoPrefab, new Vector3(Random.Range(-9, 10), _ySpawn, 0), Quaternion.identity);
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
        _spawning = false;
        StopAllCoroutines();
    }

    public bool GetSpawnStatus()
    {
        return _spawning;
    }
}
