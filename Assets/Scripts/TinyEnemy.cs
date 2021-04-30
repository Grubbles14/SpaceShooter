using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyEnemy : MonoBehaviour
{

    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = .5f;
    private float _canFire = .5f;
    private bool _isDead = false;


    void Update()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(.2f, .9f);
            _canFire = Time.time + _fireRate;
            GameObject _laserObj = Instantiate(_laserPrefab, transform.position, Quaternion.identity, this.gameObject.transform);
            _laserObj.transform.localScale = _laserObj.transform.localScale * 2.5f;
            Laser laser = _laserObj.GetComponent<Laser>();
            laser.AssignEnemy();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" && !other.GetComponent<Laser>().IsEnemy())
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
