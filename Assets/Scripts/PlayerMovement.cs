using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject boots, armor;

    private new Rigidbody2D rigidbody2D;
    private Camera cam;
    private Vector2 firstTouchPos;
    private Vector2 secondTouchPos;
    private Vector2 currentSwipe;
    private float minSwipeLength = 200f;

    private float timeClick;

    [SerializeField] private float speed = 10f;
    private bool isMoving = true;
    public bool IsMoving { get => isMoving; private set { } }

    private bool lavaResistance, bulletProof;
    private bool win;
    public bool Win { get => win; private set { } }

    private new Sound audio;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        cam = GetComponentInChildren<Camera>();
        audio = GameObject.Find("Manager").GetComponent<Sound>();
    }
    void Update()
    {
        Swipe();
    }
    // moves player up/down/right/left
   void Movement(string direction)
    {
        Vector2 move = Vector2.zero;
        switch(direction)
        {
            case "up":
                move = transform.up;
                break;
            case "down":
                move = -transform.up;
                break;
            case "left":
                move = -transform.right;
                break;
            case "right":
                move = transform.right;
                break;
        }
        rigidbody2D.AddForce(move*speed, ForceMode2D.Impulse);
        audio.PlaySound("move");
    }

    // check swipe direction and then call movement method
    void Swipe()
    {
        if (!isMoving) return;

        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstTouchPos = new Vector2(t.position.x, t.position.y);
                timeClick = Time.time;

            }
            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondTouchPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                currentSwipe = new Vector3(secondTouchPos.x - firstTouchPos.x, secondTouchPos.y - firstTouchPos.y);

                if (Time.time - timeClick > 0.1f && currentSwipe.magnitude > minSwipeLength) 
                {
                    //normalize the 2d vector
                    currentSwipe.Normalize();

                    if (Mathf.Abs(currentSwipe.x) < 0.5f)
                    {
                        if (currentSwipe.y > 0) Movement("up");
                        else if (currentSwipe.y < 0) Movement("down");
                    }
                    if (Mathf.Abs(currentSwipe.y) < 0.5f)
                    {
                        if (currentSwipe.x > 0) Movement("right");
                        else if (currentSwipe.x < 0) Movement("left");
                    }
                }
                else { GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity); }
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if collides with explosion or lava or arrow dies, but if wearing boots player can walk on lava, same with armor and arrows
        if (collision.tag == "explosion"|| 
            (collision.tag == "lava"&& !lavaResistance) || 
            collision.gameObject.tag == "Death" && !bulletProof)
        {
            Handheld.Vibrate();
            cam.gameObject.transform.parent = null;
            skeleton.transform.parent = null;
            skeleton.SetActive(true);
            audio.PlaySound("death");
            Destroy(gameObject);

        }
        // win condition
        if (collision.tag == "exit")
        {
            win = true;
            isMoving = false;
        }
        // if collides with inventory, player can take it, and walk on lava or becomes bulletproof
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
            audio.PlaySound("collect");
        }
    }
    
}
