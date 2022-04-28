using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    [SerializeField] private Tilemap backgroundTiles;
    [SerializeField] private Tilemap obstacleTiles;

    private Vector3Int location;
    private float countdown;
    private int explodeRange;

    private void Start()
    {

        backgroundTiles = GameObject.Find("Background").GetComponent<Tilemap>();
        obstacleTiles = GameObject.Find("Obstacles").GetComponent<Tilemap>();
        countdown = 3f;
        explodeRange = 10;
        StartCoroutine(Countdown());
    }
    // wait countdown seconds and that explode
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(countdown);
        GetT();
        Handheld.Vibrate();
        Destroy(gameObject);
    }
    // check all directions where can explode
    void GetT()
    {
        Vector3 mp = transform.position;
        location = backgroundTiles.WorldToCell(mp);
        for (int i = 0; i <explodeRange; i++)
        {
            if (backgroundTiles.GetTile(location))
            {
                if (obstacleTiles.GetTile(location) == null)
                {
                    var boom = Instantiate(explosion, transform.position + new Vector3(0, i), Quaternion.identity);
                }
                else
                {
                    obstacleTiles.SetTile(location, null);
                    break;
                }
            }
            else break;
            location.y++;
        }
        location = backgroundTiles.WorldToCell(mp);
        for (int i = 0; i > -explodeRange; i--)
        {
            location.y--;
            if (backgroundTiles.GetTile(location))
            {
                if (obstacleTiles.GetTile(location) == null)
                {
                    var boom = Instantiate(explosion, transform.position + new Vector3(0, i), Quaternion.identity);
                }
                else
                {
                    obstacleTiles.SetTile(location, null);
                    break;
                }
            }
            else break;
        }
        location = backgroundTiles.WorldToCell(mp);
        for (int i = 0; i < explodeRange; i++)
        {
            if (backgroundTiles.GetTile(location))
            {
                if (obstacleTiles.GetTile(location) == null)
                {
                    var boom = Instantiate(explosion, transform.position + new Vector3(i, 0), Quaternion.identity);
                }
                else
                {
                    var boom = Instantiate(explosion, transform.position + new Vector3(i, 0), Quaternion.identity);
                    obstacleTiles.SetTile(location, null);
                    break;
                }
            }
            else break;
            location.x++;
        }
        location = backgroundTiles.WorldToCell(mp);
        for (int i = 0; i > - explodeRange; i--)
        {
            location.x--;
            if (backgroundTiles.GetTile(location))
            {
                if (obstacleTiles.GetTile(location) == null)
                {
                    var boom = Instantiate(explosion, transform.position + new Vector3(i, 0), Quaternion.identity);
                }
                else
                {
                    obstacleTiles.SetTile(location, null);
                    break;
                }
            }
            else break;
        }
    }

}
