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

    [SerializeField]
    private GameObject[] _powerupList;

    
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    //spawn game objects every 3.5 seconds
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
        yield return new WaitForSeconds(4);
        while (!_stopSpawning)
        {
            int randomPowerup = Random.Range(0, _powerupList.Length);
            Instantiate(_powerupList[randomPowerup], new Vector3(Random.Range(-9, 10), _ySpawn, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6, 10));
        }
    }

    public void StopSpawn()
    {
        _stopSpawning = true;
    }
}
