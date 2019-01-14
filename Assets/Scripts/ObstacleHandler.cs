using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    
    float obstaclewidth = 0;
    float endLine = 0;
    float HeightClamp = 0;
    float lasty = 0;

    //
    public bool isPlaying = false;
    
    public List<MovingObstacle> obstacles;

    [Range(2,10)]
    public int spaceBetweenObstacles = 1;   

    public void Reset()
    {
        lasty = 0;
        StartCoroutine(StartPlay());
    }

    public IEnumerator StartPlay()
    {
        //setup and randomize first n obstacles has in the pool

        //get moving obstacle list
        obstacles = GetComponentsInChildren<MovingObstacle>(true).ToList();
        
        if (obstacles != null && obstacles.Count > 0)
        {
            //assign obstacle width in this case i'm using only width cause sprite it's a square
            if(obstaclewidth<=0)
            obstaclewidth = obstacles[0].bottom.size.x;

            //Setup a height clamp for the range in within the gap between two obstacle should fall
            HeightClamp = (GameManager._Instance.FrustumHeight * 0.5f) - (obstaclewidth * 2);

            //First x position just outside the screen
            float startX = (GameManager._Instance.FrustumWidth / 2) + (obstaclewidth / 2);
            //end line is the x position just outside the left bound of the screen
            endLine = -startX;
            for (int i = 0; i < obstacles.Count; i++)
            {
                //Activate
                obstacles[i].gameObject.SetActive(true);
                //Search new y pos based on lasty if i is 0 then lasty will be 0
                float newyPos = Mathf.Clamp(lasty + Random.Range(-obstaclewidth*spaceBetweenObstacles, obstaclewidth * spaceBetweenObstacles),-HeightClamp,HeightClamp);
                //assign position to obstacle
                obstacles[i].transform.position = new Vector3(startX + ((i * obstaclewidth) * spaceBetweenObstacles),newyPos , 0);
                //new y is now last y
                lasty = newyPos;
                yield return new WaitForSeconds(0.1f);
            }
            //this make the obstacle start moving
            isPlaying = true;
        }

        yield break;
    }
    
    // Update is called once per frame
    void Update()
    {
        //if there are no obstacle in the list or list uninitialized then return
        if (obstacles == null || obstacles.Count == 0)
        {
            return;
        }

        //check if the obstacle with lower x finished is movement
        if (obstacles[0].transform.position.x<endLine)
        {
            //assign a new y 
            float newyPos = Mathf.Clamp(lasty + Random.Range(-obstaclewidth * spaceBetweenObstacles, obstaclewidth * spaceBetweenObstacles), -HeightClamp, HeightClamp);
            //update obstacle position
            obstacles[0].transform.position=new Vector3(obstacles[obstacles.Count-1].transform.position.x + ((obstaclewidth) * spaceBetweenObstacles), newyPos, 0);
            //assign new y to last y
            lasty = newyPos;
            //sort array based on x
            obstacles.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        }
    }
}
