using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    private float _speed = 3.0f;
    private Player _playerObject;

    //ID for powerups
    //0 for triple shot
    //1 for speed
    //2 for shields
    [SerializeField]
    private int _powerupID;

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
            switch (_powerupID)
            {
                case 0:
                    _playerObject.EnableTripleShot();
                    break;
                case 1:
                    _playerObject.EnableSpeedBoost();
                    break;
                case 2:
                    _playerObject.EnableShield();
                    break;
                case 3:
                    _playerObject.RefillAmmo();
                    break;
                case 4:
                    _playerObject.AddHealth();
                    break;
                case 5:
                    _playerObject.EnableRailgun();
                    break;
            }
            
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerMagnet")
        {
            float step = 10 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, other.gameObject.GetComponent<Transform>().position, step);
        }
    }
}
