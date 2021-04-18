using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float _lowerYBound = 3;
    private int _bossHealth = 15;
    private bool _isBossAlive = false;
    private float _bossSpeed = 2f;

    [SerializeField]
    private Sprite[] _healthBars;
    [SerializeField]
    private SpriteRenderer _bossHealthSprite;

    void Start()
    {
        transform.position = new Vector3(0, 10, 1);
        transform.GetComponentInParent<BoxCollider2D>().enabled = false;
        StartCoroutine(DramaticEntry());      
    }

    IEnumerator DramaticEntry()
    {
        while(transform.position.y >= _lowerYBound)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 2);
            yield return new WaitForEndOfFrame();
        }
        transform.GetComponentInParent<BoxCollider2D>().enabled = true;
        _isBossAlive = true;
        StartCoroutine(BossMovementRoutine());
        Debug.Log("Boss is alive, starting movement routine");
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
        }
    }

    IEnumerator BossMovementRoutine()
    {
        while (_isBossAlive)
        {
            float nextXPos = Random.Range(-6.5f, 6.5f);
            Debug.Log("Picking new xpos: " + nextXPos);
            Vector3 target = new Vector3(nextXPos, transform.position.y, transform.position.z);
            while (Vector3.Distance(transform.position, target) > 0.001f)
            {
                float step = _bossSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target, step);
                Debug.Log("Moving towards: " + target.ToString());
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(3);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            _bossHealth--;
            UpdateHealth(_bossHealth);
        }
    }
}
