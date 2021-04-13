using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativePickup : MonoBehaviour
{

    private float _speed = 2.0f;
    private Player _playerObject;

    [SerializeField]
    private int _negPowerupID;

    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        //move down at speed of 3
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        //when leave screen, destroy
        if (transform.position.y < -4.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            _playerObject = other.transform.GetComponent<Player>();
            _playerObject.PlaySound(_audioSource.clip);
            switch (_negPowerupID)
            {
                case 0:
                    _playerObject.RemoveHealth();
                    break;
            }

            Destroy(gameObject);
        }
    }
}
