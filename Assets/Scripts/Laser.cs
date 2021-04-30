using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _projectileSpeed = 8.0f;
    private bool _isEnemyLaser = false;


    void Start()
    {
        if (transform.parent == null || transform.parent.tag == "Enemy")
            Destroy(gameObject, 3f);
        else if (transform.parent != null && transform.parent.tag != "Enemy")
            Destroy(transform.parent.gameObject, 3f);
    }

    void Update()
    {

        if (!_isEnemyLaser)
            MoveUp();

        else
            MoveDown();
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _projectileSpeed);
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _projectileSpeed);
    }
    public void AssignEnemy()
    {
        _isEnemyLaser = true;
    }
    public bool IsEnemy()
    {
        return _isEnemyLaser;
    }
}
