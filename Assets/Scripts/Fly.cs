using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    private bool ismoving;
    void Start()
    {
        ismoving = true;
    }

    void Update()
    {
        if(ismoving)
        transform.position +=  0.5f*transform.right;
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
