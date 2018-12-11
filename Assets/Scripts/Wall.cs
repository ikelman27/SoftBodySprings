using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A script to contian info for the wall that the fabric collides with as well as info for collision detection with the wall
public class Wall : MonoBehaviour
{
    
 
    //list of the rectangles points in worldspace
    public List<Vector2> worldPoints;
    //the normals to the point
    public List<Vector2> normals;
    // a box collider used to get the points of the object in world space
    public PolygonCollider2D collider;
    //position scale and rotation are used to store the previous frames 
    Vector3 scale;
    Vector3 position;
    Quaternion rotation;
    bool isMoved;
    public Camera camera;
    // Use this for initialization
    void Start()
    {
        //set objects
        camera = Camera.main;
        collider = gameObject.GetComponent<PolygonCollider2D>();
        worldPoints = new List<Vector2>();
        //set the objects vertices in the world
        setWorldPoints();
        //set variables for info for previeous frame for opptomization
        position = gameObject.transform.position;
        scale = gameObject.transform.localScale;
        rotation = gameObject.transform.rotation;

    }

    //sets teh objects points & normals in the world
    public void setWorldPoints()
    {
        //clear the previous array of poiints & normals
        worldPoints.Clear();
        normals.Clear();
        //get the world points from the collider
        worldPoints.Add(collider.transform.TransformPoint(collider.points[0]));
        //loop through each point and make the normals
        for (int i = 1; i < collider.points.Length; i++)
        {
            worldPoints.Add(collider.transform.TransformPoint(collider.points[i]));
            normals.Add(worldPoints[i - 1] - worldPoints[i]);
        }

        normals.Add(worldPoints[collider.points.Length - 1] - worldPoints[0]);


        //calculate the perpindicular to the line calulated to get the true normal
        for (int i = 0; i < normals.Count; i++)
        {

           normals[i] = new Vector2(normals[i].y, -normals[i].x);

       }
    }

    //checks if the point is inside the object using SAT
    public Vector3 checkCollision(GameObject node)
    {
        //get the smallest axis for each normal and determine the overlap
        Vector2 smallestAxis = Vector2.zero;
        float overlap = float.MaxValue;
        float pointValue;
        for (int i = 0; i < normals.Count; i++)
        {
            float min1 = Vector2.Dot(normals[i], worldPoints[0]);
            float max1 = min1;

            for (int j = 0; j < normals.Count; j++)
            {
                float c = Vector2.Dot(normals[i], worldPoints[j]);
                if (c < min1) { min1 = c; }
                if (c > max1) { max1 = c; }
            }


            pointValue = Vector2.Dot(normals[i], node.transform.position);
            //if the point is overlaping the line at the value continue otherwise return with zero
            if (pointValue >= min1 && pointValue <= max1)
            {
                //check if the overlap is in the smallest axis
                float o = pointValue - min1;
                if (o < overlap)
                {
                    overlap = o;
                    smallestAxis = normals[i];
                }
            }
            else
            {
                
                return Vector3.zero;
            }
        }
        // calulate the mtv based off the smallest angle of overlap
        float valX = -smallestAxis.x * overlap / (smallestAxis.magnitude * smallestAxis.magnitude + Mathf.Epsilon);
        float valY = -smallestAxis.y * overlap / (smallestAxis.magnitude * smallestAxis.magnitude + Mathf.Epsilon);
        Vector2 mtv = new Vector2(valX, valY);
        //Debug.DrawRay(Vector3.zero, mtv, Color.red);
        return mtv;
        

    }

    //if the wall is clicked move it
    private void OnMouseDown()
    {

       
        isMoved = true;
        gameObject.transform.position = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y, 0);
        

    }

    //stop moving the object
    private void OnMouseUp()
    {
        isMoved = false;
    }


    // Update is called once per frame
    void Update()
    {
        //if the object is clicked set its position to the mouse's position
        if (isMoved)
        {
            gameObject.transform.position = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y, 0);

        }

        //if position scale or rotation have changed update the world points & set the new ones
        if (position != gameObject.transform.position || scale != gameObject.transform.localScale || rotation != gameObject.transform.rotation)
        {

            setWorldPoints();
            position = gameObject.transform.position;
            scale = gameObject.transform.localScale;
            rotation = gameObject.transform.rotation;
        }
        
       
        
        
      
    }
}
