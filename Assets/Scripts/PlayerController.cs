using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //physics var
    public float MaxJumpHeight = 10;
    public float verticalAcceleration = 1f;
    public float Gravity = 0.981f;

    //physics var
    float currentSpeed = 0;
    BoxCollider2D bcollider;


    //gameplay vars
    bool isDead = false;
    bool isPlaying;

    public bool IsPlaying
    {
        get
        {
            return isPlaying;
        }

        set
        {
            isPlaying = value;                
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize
        isDead = false;
        bcollider = GetComponent<BoxCollider2D>();
    }

    //death function
    void Die()
    {
        isDead = true;
        GameManager._Instance.OnDeath();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player is not playing or is dead 
        if (isDead || !isPlaying)
            return;

        //if colliding with bottom or an obstacle then die
        if (collision.tag ==  "Obstacle" || collision.tag == "Bottom")
        {
            Die();
            return;
        }      
    }

    public void ResetAndPlay()
    {
        //Resety all vars and play
        this.transform.position = Vector3.zero;
        isDead = false;
        isPlaying = false;
        currentSpeed = 0;
        //to add a little delay on start game i've used a coroutine
        StartCoroutine(StartGame());
        
    }

    IEnumerator StartGame()
    {
        //reset Obstaclehandler to generate a new scene
        GameManager._Instance.obstacleHandler.Reset();

        yield return new WaitForSeconds(0.75f);
        //game start here
        isPlaying=true;
    }

    //if player reached max height on screen return true
    public bool OnRoof()
    {
       return this.transform.position.y >= (GameManager._Instance.FrustumHeight * 0.5f) - (bcollider.size.y * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (bcollider == null)
            return;

        if (!isPlaying)
            return;

        //mouse input button works for mobile first touch
        if (Input.GetMouseButtonDown(0) && !OnRoof() && !isDead)
        {
            //if falling then reset speed to 0
            if (currentSpeed < 0)
                currentSpeed = 0;

            //add current speed vertical acceleration to a max value
            currentSpeed = Mathf.Clamp(currentSpeed + verticalAcceleration * Time.deltaTime, 0, MaxJumpHeight);
        }
        else
        {
            //this var is used to make a tweakable jump like mario bross
            float accelmult = 1;

            //if player is not holding down and player is jumping then the jump force should consume earlier
            if (currentSpeed > 0 && !Input.GetMouseButton(0))
                accelmult = 1.5f;
       
            //apply gravity
            currentSpeed -= Gravity * accelmult * Time.deltaTime;
        }

        //calculate new position
        Vector3 newposition = this.transform.position + (Vector3.up * currentSpeed);
        
        //clamp newposition y to the bounds of the screen
        newposition.y = Mathf.Clamp(newposition.y, (-GameManager._Instance.FrustumHeight * 0.5f)+(bcollider.size.y*0.5f), GameManager._Instance.FrustumHeight * 0.5f-(bcollider.size.y*0.5f));

        //apply new position
        this.transform.position= newposition;



    }
}
