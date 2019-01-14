using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed=2;
    public float maxSpeed = 10;
    
    public BoxCollider2D top, bottom;

    public bool move = false;

    public bool dispensedScore = false;

    // Start is called before the first frame update
    void Start()
    {
        dispensedScore = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if game is not playing return
        if(!GameManager._Instance.obstacleHandler.isPlaying)
        {
            return;
        }

        //update position obstacle
        this.transform.position += -Vector3.right * (speed * Time.deltaTime);

        //Check if already dispensed score
        if (!dispensedScore)
        {
            //if player surpassed this obstacle on x increase score
            if (this.transform.position.x + top.size.x * 0.5f <= GameManager._Instance.pController.transform.position.x)
            {
                GameManager._Instance.increaseScore();
                dispensedScore = true;
            }
        }
    }
}
