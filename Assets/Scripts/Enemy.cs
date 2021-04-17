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

    private float _rotAngleClamp = 10f;
    private float _rotAngle;
    private float _rotSpeed = 2f;

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
        //RotateEnemy();

        if (Time.time > _canFire && !_isDead)
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

    private void RotateEnemy()
    {
        _rotAngle = transform.rotation.z;

        _rotAngle += (_rotSpeed * Time.deltaTime);

        _rotAngle = Mathf.Clamp(_rotAngle, -_rotAngleClamp, _rotAngleClamp);

        //transform.Rotate(Vector3.forward * Time.deltaTime * _rotSpeed);
        //transform.eulerAngles = new Vector3(0, 0, _rotAngle);
        //Quaternion rQuat = Quaternion.AngleAxis(_rotAngle, Vector3.forward);

        transform.localRotation = new Quaternion(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, _rotAngle, 1);
        Debug.Log("transform rotation z: " + transform.localRotation.eulerAngles.z + "     rotangle" + _rotAngle);


        if (transform.localRotation.eulerAngles.z > _rotAngleClamp)
        {
            Debug.Log("in if statement");
            //_rotSpeed *= -1;
            //_rotAngle += (_rotSpeed);
            //transform.eulerAngles = new Vector3(0, 0, _rotAngle);

        }
        else if (transform.localRotation.eulerAngles.z < -_rotAngleClamp)
        {
            Debug.Log("in if-else");
            //_rotSpeed *= -1;
            //_rotAngle += (_rotSpeed);
            //transform.eulerAngles = new Vector3(0, 0, _rotAngle);
        }

    }
}
