using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    private bool ismoving;
    private const float arrowSpeed = 0.3f;
    void Start()
    {
        ismoving = true;
    }

    void Update()
    {
        if(ismoving)
        transform.position +=  transform.right * arrowSpeed;
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ismoving = false;
    }
}
