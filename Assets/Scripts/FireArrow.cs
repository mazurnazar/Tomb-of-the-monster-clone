using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : MonoBehaviour
{
    [SerializeField] private GameObject arrowPref;
    [SerializeField] private GameObject player;
    [SerializeField] private int fireRate;
    private void Start()
    {
        fireRate = 5;
        StartCoroutine(Fire());
    }
    private void Update()
    {
        Aim();
    }
    // instantiate arrow every fireRate seconds
    IEnumerator Fire()
    {
        if (player.GetComponent<PlayerMovement>().IsMoving)
        {
            yield return new WaitForSeconds(fireRate);
            var arrow = Instantiate(arrowPref, transform.position, Quaternion.identity);

            arrow.transform.parent = this.transform;
            arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
            arrow.transform.localPosition = new Vector3(0.2f, 0.02f, 0);
            StartCoroutine(Fire());
        }
    }
    // bow rotates to look at player
    void Aim()
    {
        if (player == null) return;
        Vector3 dir = player.transform.position - transform.position;
        dir = player.transform.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle );
    }
}
