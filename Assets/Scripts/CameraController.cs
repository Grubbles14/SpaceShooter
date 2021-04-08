using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 originPos;

    [SerializeField]
    private float bounds = .3f;

    private void Start()
    {
        originPos = transform.position;
    }


    public void CameraShake()
    {

        StopCoroutine("ShakeRoutine");

        float[] xPosArray = new float[10];
        float[] yPosArray = new float[10];

        //fill arrays with random x and y positions
        for (int i = 0; i < 10; i++)
        {
            xPosArray[i] = Random.Range(transform.position.x - bounds, transform.position.x + bounds);
            yPosArray[i] = Random.Range(transform.position.y - bounds, transform.position.y + bounds);
        }

        StartCoroutine(ShakeRoutine(xPosArray, yPosArray));

    }

    IEnumerator ShakeRoutine(float[] xpos, float[] ypos)
    {
        for(int i = 0; i < xpos.Length; i++)
        {
            transform.position = new Vector3(xpos[i], ypos[i], transform.position.z);
            yield return new WaitForSeconds(.05f);
        }

        transform.position = originPos;
    }

}
