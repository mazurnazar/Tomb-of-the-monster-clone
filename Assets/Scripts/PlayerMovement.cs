using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject boots, armor;

    private Rigidbody2D rigidbody2D;
    private Vector3 firstClick;
    private float timeClick;

    [SerializeField] private float speed = 10f;
    private bool isMoving = true;
    private bool lavaResistance, bulletProof;
    private bool win;
    public bool Win { get => win; private set { } }

    private Animator animator;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        cam = GetComponentInChildren<Camera>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        CheckDirection();
    }
    // moves player up/down/right/left
    void Movement(string direction)
    {
        switch(direction)
        {
            case "up":
                rigidbody2D.AddForce(transform.up * speed, ForceMode2D.Impulse);
                break;
            case "down":
                rigidbody2D.AddForce(-transform.up * speed, ForceMode2D.Impulse);
                break;
            case "left":
                rigidbody2D.AddForce(-transform.right * speed, ForceMode2D.Impulse);
                break;
            case "right":
                rigidbody2D.AddForce(transform.right * speed, ForceMode2D.Impulse);
                break;
        }
    }

    // check swipe direction and then call movement method
    void CheckDirection()
    {
        if (!isMoving) return;
        if (Input.GetMouseButtonDown(0))
        {
            firstClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            timeClick = Time.time;
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            if (Time.time - timeClick > 0.2f)
            {
                Vector3 secondClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 current = new Vector3(secondClick.x - firstClick.x, secondClick.y - firstClick.y);
                current.Normalize();
                if (Mathf.Abs(current.y) < 0.5f)
                {
                    if (current.x > 0)
                    {
                        Movement("right");
                    }
                    else if (current.x < 0)
                        Movement("left");
                }
                if (Mathf.Abs(current.x) < 0.5f)
                {
                    if (current.y > 0)
                    {
                        Movement("up");
                    }
                    else if (current.y < 0)
                        Movement("down");
                }
            }
            else
            {
                GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            }
        }
    }
    // if collides with arrow with tag 'Death', activates skeleton, and destroys player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Death" && !bulletProof)
        {
            cam.gameObject.transform.parent = null;
            skeleton.transform.parent = null;
            skeleton.SetActive(true);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if collides with explosion or lava, but if wearing boots player can walk on lava
        if (collision.tag == "explosion"|| (collision.tag == "lava"&& !lavaResistance) )
        {
            Handheld.Vibrate();
            cam.gameObject.transform.parent = null;
            skeleton.transform.parent = null;
            skeleton.SetActive(true);
            Destroy(gameObject);
        }
        // win condition
        if (collision.tag == "exit")
        {
            win = true;
            isMoving = false;
            animator.StopPlayback();
        }
        // if collides with inventory, player can walk on lava or becomes bulletproof
        if(collision.tag == "inventory")
        {
            if (collision.name == "Boots")
            {
                boots.GetComponent<SpriteRenderer>().color = Color.green;
                lavaResistance = true;
            }
            if (collision.name == "Armor")
            {
                armor.GetComponent<SpriteRenderer>().color = Color.green;
                bulletProof = true;
            }
            Destroy(collision.gameObject);
        }
    }
    
}
