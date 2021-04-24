using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed;
    private float _baseSpeed = 6f;
    private float _boostedSpeed = 10f;

    [SerializeField]
    private float _horizontalAxis;

    [SerializeField]
    private float _verticalAxis;

    private float _yUpperBound = 0;
    private float _yLowerBound = -4;
    private float _xUpperBound = 10;
    private float _xLowerBound = -10;

    //Laser prefab to be instantiated
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject tripleShotPrefab;

    //Space offset for laser position
    private float _laserOffset = 1.05f;
    private float _railgunOffset = 6f;

    //Firerate variables
    [SerializeField]
    private float _fireRate = 0.10f;
    private float _nextFire = 0.0f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private SpawnManager _spawnObject;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isShieldEnabled = false;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private GameObject _playerMagnet;

    private int _score = 0;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _fireErrorSound;
    [SerializeField]
    private AudioClip _railgunSound;
    [SerializeField]
    private AudioSource _audioSource;

    //Boost factor is the factor to multiply player speed by. Can be modified in editor.
    [SerializeField]
    private float _boostFactor = 1.5f;
    //Boost multiplier holds current boost. Modified and referenced in movement function
    private float _boostMultiplier = 1f;
    [SerializeField]
    private float _boostRemaining = 100;
    [SerializeField]
    private float _boostDrain = 15f;
    [SerializeField]
    private bool _canBoost = true;
    private bool _boosting = false;

    private int _shieldStrength = 0;

    //MaxAmmo is a reference to the max allowable ammo count. Only used to refill ammo.
    [SerializeField]
    private int _maxAmmo = 15;
    private int _currentAmmo = 15;

    [SerializeField]
    private GameObject _cameraObject;

    [SerializeField]
    private GameObject _railgunPrefab;
    [SerializeField]
    private bool _isRailgunActive = false;

    



    void Start()
    {

        _playerSpeed = _baseSpeed;
        _playerShield.SetActive(false);

        transform.position = new Vector3(0, 0, 0);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio source on player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }

        _spawnObject = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnObject == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }

        _cameraObject = GameObject.Find("Main Camera");

        if (_cameraObject == null)
        {
            Debug.LogError("Camera is NULL");
        }

        _playerMagnet.SetActive(false);


        _uiManager.UpdateAmmo(_currentAmmo, _maxAmmo);
    }

    void Update()
    {

        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _currentAmmo > 0)
        {
            ShootLaser();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _currentAmmo <= 0)
        {
            _audioSource.PlayOneShot(_fireErrorSound);
        }

        if (Input.GetKey(KeyCode.C))
        {
            _playerMagnet.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            _playerMagnet.SetActive(false);
        }

    }

    void ShootLaser()
    {
        _nextFire = Time.time + _fireRate;


        if (_isRailgunActive)
        {
            Instantiate(_railgunPrefab, new Vector3(transform.position.x, transform.position.y + _railgunOffset, transform.position.z), Quaternion.identity);
            _audioSource.PlayOneShot(_railgunSound);
        }
        else if (_isTripleShotActive)
        {
            Instantiate(tripleShotPrefab, new Vector3(transform.position.x, transform.position.y + _laserOffset, transform.position.z), Quaternion.identity);
            _audioSource.PlayOneShot(_laserSound);
        }
        else
        {
            Instantiate(laserPrefab, new Vector3(transform.position.x, transform.position.y + _laserOffset, transform.position.z), Quaternion.identity);
            _audioSource.PlayOneShot(_laserSound);
        }
        

        //Only reduce ammo count if spawning has started
        if(_spawnObject.GetSpawnStatus())
            _currentAmmo--;

        _uiManager.UpdateAmmo(_currentAmmo, _maxAmmo);
    }

    public void RefillAmmo()
    {
        _currentAmmo = _maxAmmo;
        _uiManager.UpdateAmmo(_currentAmmo, _maxAmmo);
    }


    void CalculateMovement()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        _verticalAxis = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && _canBoost)
        {
            _boosting = true;
            StopCoroutine("BoostCooldownRoutine");
            _boostMultiplier = _boostFactor;
            _boostRemaining -= (_boostDrain * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _boosting = false;
            _boostMultiplier = 1f;
            StopCoroutine("BoostCooldownRoutine");
            StartCoroutine(BoostCooldownRoutine());
        }

        _uiManager.UpdateBoosterBar(_boostRemaining);

        //boost multiplier is added inline with movement calculation
        transform.Translate(Vector3.right * _playerSpeed * _boostMultiplier * _horizontalAxis * Time.deltaTime);

        transform.Translate(Vector3.up * _playerSpeed * _boostMultiplier * _verticalAxis * Time.deltaTime);


        if (transform.position.y >= _yUpperBound)
        {
            transform.position = new Vector3(transform.position.x, _yUpperBound, transform.position.z);
        }
        else if (transform.position.y <= _yLowerBound)
        {
            transform.position = new Vector3(transform.position.x, _yLowerBound, transform.position.z);
        }


        if (transform.position.x >= _xUpperBound)
        {
            transform.position = new Vector3(_xLowerBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= _xLowerBound)
        {
            transform.position = new Vector3(_xUpperBound, transform.position.y, transform.position.z);
        }

    }

    private IEnumerator BoostCooldownRoutine()
    {
        _canBoost = false;
        yield return new WaitForSeconds(1.5f);
        _canBoost = true;
        while (_boostRemaining < 100 && !_boosting)
        {
            _boostRemaining += (_boostDrain * 2f * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    public float GetBoostLevel()
    {
        return Mathf.Clamp(_boostRemaining, 0, 100);
    }

    public void Damage()
    {

        if (_isShieldEnabled && _shieldStrength > 0)
        {
            _shieldStrength--;
            if (_shieldStrength <= 0)
            {
                //Shield needs to be turned off after shield strength reaches 0
                _isShieldEnabled = false;
                _playerShield.SetActive(false);
            }

            //Call method in UImanager to update shields
            _uiManager.UpdateShields(_shieldStrength);
            return;
        }

        else
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" && other.GetComponent<Laser>().IsEnemy())
        {
                Destroy(other.gameObject);
                Damage();
        }
    }

    public void EnableTripleShot()
    {
        //Make change to disable any current coroutines
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void EnableSpeedBoost()
    {
        _playerSpeed = _boostedSpeed;
        StartCoroutine(SpeedBoostRoutine());
    }

    IEnumerator SpeedBoostRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _playerSpeed = _baseSpeed;
    }

    public void EnableShield()
    {
        _isShieldEnabled = true;
        _shieldStrength = 3;
        _playerShield.SetActive(true);
        _uiManager.UpdateShields(3);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void PlaySound(AudioClip sound){
        _audioSource.PlayOneShot(sound);
        }

    public void AddHealth()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
        }
    }

    public void EnableRailgun()
    {
        _isRailgunActive = true;
        RefillAmmo();
        StartCoroutine(RailgunPowerDownRoutine());
    }

    IEnumerator RailgunPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isRailgunActive = false;
    }

    public void RemoveHealth()
    {
        TakeDamage();
    }

    private void TakeDamage()
    {
        _lives--;
        _uiManager.UpdateLives(_lives);
        _cameraObject.GetComponent<CameraController>().CameraShake();
        switch (_lives)
        {
            case 2:
                _rightEngine.SetActive(true);
                break;
            case 1:
                _leftEngine.SetActive(true);
                break;
            case 0:
                Destroy(gameObject);
                if (_spawnObject != null)
                    _spawnObject.StopSpawn();
                break;
        }
    }

    
}
