using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _rotSpeed = 10f;

    [SerializeField]
    private GameObject _explosionObject;
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private GameObject _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            GameObject expl = Instantiate(_explosionObject, transform.position, Quaternion.identity);
            Destroy(expl, 3f); //destroy explosion object
            Destroy(other.gameObject); //destroy laser
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.2f); //destroy asteroid
        }
    }
}
