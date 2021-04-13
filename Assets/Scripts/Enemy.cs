using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float _enemySpeed = 3.5f;
    private float _ySpawn = 7.0f;

    private Player playerObj;

    //handle to animator component
    private Animator _anim;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    //Create a bool isFireEnabled so I can stop firing after enemy is destroyed

    // Start is called before the first frame update
    void Start()
    {
        //assign animator component
        _anim = this.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Enemy animator is null");
        }

        playerObj = GameObject.Find("Player").GetComponent<Player>();

        _audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject _laser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] _lasers = _laser.GetComponentsInChildren<Laser>();
            foreach (Laser l in _lasers)
            {
                l.AssignEnemy();
            }
        }

    }

    private void CalculateMovement(){

        transform.Translate(Vector3.down * Time.deltaTime * _enemySpeed);

        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(playerObj != null)
                playerObj.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            this.GetComponent<BoxCollider2D>().enabled = false;
            _audioSource.Play();
            Destroy(gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            //Need to put a check here to ignore laser if it's from an enemy
            Destroy(other.gameObject);
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            this.GetComponent<BoxCollider2D>().enabled = false;
            _audioSource.Play();
            if (playerObj != null)
                playerObj.AddScore(10);
            Destroy(gameObject, 2.8f);
        }

        if (other.tag == "Railgun")
        {
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            this.GetComponent<BoxCollider2D>().enabled = false;
            _audioSource.Play();
            if (playerObj != null)
                playerObj.AddScore(10);
            Destroy(gameObject, 2.8f);
        }
    }
}
