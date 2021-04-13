using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{

    private float _timeToLive = 2f;
    private float _alphaDrainRate = .02f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeRoutine());
    }

    //create coroutine to fade away, then destroy
    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(.5f);
        while (gameObject.GetComponent<SpriteRenderer>().color.a > .15)
        {
            Color c = gameObject.GetComponent<SpriteRenderer>().color;
            c.a -= _alphaDrainRate;
            gameObject.GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
            Destroy(other.gameObject);
    }
}
