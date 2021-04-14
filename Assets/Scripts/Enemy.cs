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
    private bool _isDead = false;

    private bool _isShieldedEnemy = false;
    private bool _shieldUp = false;
    [SerializeField]
    private GameObject _enemyShield;

    void Awake()
    {
        int d = Random.Range(1, 11);
        switch (d)
        {
            case 1:
                _isShieldedEnemy = true;
                break;
            default:
                break;
        }
    }

    void Start()
    {

        if (_isShieldedEnemy)
        {
            _shieldUp = true;
            _enemyShield.SetActive(true);
        }

        _anim = this.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Enemy animator is null");
        }

        playerObj = GameObject.Find("Player").GetComponent<Player>();

        _audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {

        CalculateMovement();

        if(Time.time > _canFire && !_isDead)
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
            EnemyDestroy();
        }

        if (other.tag == "Laser" && !other.GetComponent<Laser>().IsEnemy())
        {
            //I'm only checking for shields active when hit with laser. Colliding with player should destroy enemy completely. Railgun goes through shields.
            if (_shieldUp)
            {
                _shieldUp = false;
                _enemyShield.SetActive(false);
            }
            else
            {
                Destroy(other.gameObject);
                EnemyDestroy();
                if (playerObj != null)
                    playerObj.AddScore(10);
            }
        }

        if (other.tag == "Railgun")
        {
            EnemyDestroy();
            if (playerObj != null)
                playerObj.AddScore(10);
        }
    }

    private void EnemyDestroy()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _enemySpeed = 0;
        this.GetComponent<BoxCollider2D>().enabled = false;
        _isDead = true;
        _enemyShield.SetActive(false);
        _audioSource.Play();
        Destroy(gameObject, 2.8f);
    }
}
