using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float _lowerYBound = 3;
    private int _bossHealth = 15;
    private bool _isBossAlive = false;
    private bool _isMoving = false;
    private float _bossSpeed = 2f;
    private float _alphaDrainRate = .4f;

    [SerializeField]
    private Sprite[] _healthBars;
    [SerializeField]
    private SpriteRenderer _bossHealthSprite;
    [SerializeField]
    private GameObject _bossShield;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    //private Vector3[] _firePoints = {new Vector3(-0.41f, -.5f, 0f), new Vector3(0.41f, -.5f, 0f), new Vector3(0f, -.5f, 0f) };

    void Start()
    {

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }

        transform.position = new Vector3(0, 10, 1);
        transform.GetComponentInParent<BoxCollider2D>().enabled = false;
        _bossShield.SetActive(false);
        _bossHealthSprite.enabled = false;
        StartCoroutine(DramaticEntry());      
    }

    private void Update()
    {
        if (_isBossAlive)
            FireLaser();
    }

    IEnumerator DramaticEntry()
    {
        while(transform.position.y >= _lowerYBound)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 2);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3);
        transform.GetComponentInParent<BoxCollider2D>().enabled = true;
        _isBossAlive = true;
        _bossHealthSprite.enabled = true;
        StartCoroutine(BossMovementRoutine());
    }

    void UpdateHealth(int currentHealth)
    {
        _bossHealthSprite.sprite = _healthBars[Mathf.Clamp(currentHealth, 0, 15)];

        if (currentHealth == 0)
        {
            //GameOverSequence();
            //Create logic to end game once boss is destroyed
            _isBossAlive = false;
            StopCoroutine("BossMovementRoutine");
            _uiManager.GameOverSequence(true);
            StartCoroutine(BossDestroySequence());
        }
    }

    IEnumerator BossMovementRoutine()
    {
        while (_isBossAlive)
        {
            float nextXPos = Random.Range(-6.5f, 6.5f);
            Vector3 target = new Vector3(nextXPos, transform.position.y, transform.position.z);
            _isMoving = true;
            _bossShield.SetActive(true);
            while (Vector3.Distance(transform.position, target) > 0.001f)
            {
                float step = _bossSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target, step);
                yield return new WaitForEndOfFrame();
            }
            _isMoving = false;
            _bossShield.SetActive(false);
            yield return new WaitForSeconds(3);
        }
    }

    void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(.75f, 1.5f);
            _canFire = Time.time + _fireRate;
            int loc = Random.Range(-1, 2);
            Vector3 laserLoc = new Vector3(transform.position.x + (2f * loc), transform.position.y - 2.3f, transform.position.z);
            GameObject _laser = Instantiate(_laserPrefab, laserLoc, Quaternion.identity);
            _laser.GetComponent<Laser>().AssignEnemy();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser") && !_bossShield.activeSelf && !other.GetComponent<Laser>().IsEnemy())
        {
            _bossHealth--;
            UpdateHealth(_bossHealth);
            Destroy(other.gameObject);
        }
    }

   IEnumerator BossDestroySequence()
    {
        _bossHealthSprite.enabled = false;
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        while (c.a > .1)
        {
            c.a -= (_alphaDrainRate * Time.deltaTime);
            gameObject.GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
