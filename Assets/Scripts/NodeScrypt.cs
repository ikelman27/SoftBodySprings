using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//a script contianing all the info about each point in world space
public class NodeScrypt : MonoBehaviour
{

    //basic movement info
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    //objects mass
    public float mass;

    //an anchor object is locked in place and cant be moved
    public bool isAnchor;
    //the wall in the scene
    public GameObject walls;
    //the circle in the scene
    public GameObject Cirlces;
    //camera for mouse movement
    public Camera camera;
    //if the node is drawn
    public bool draw;
    //mouse movement
    public bool isMoved;

    //limit speed and force
    float maxSpeed;
    public float maxForce;

    //vector for gravity
    public Vector3 gravity;



    // Use this for initialization
    void Start()
    {
        //set all the basic default values
        maxForce = 100000;
        draw = true;
        maxSpeed = 100f;
        camera = Camera.main;

        //if the object is an anchor set its color to blue
        SpriteRenderer mySpirite = GetComponent<SpriteRenderer>();
        if (isAnchor)
        {
            mySpirite.color = Color.blue;

        }
        else
        {
            mySpirite.color = Color.white;
        }

    }

    public void beginSim()
    {
        //get poistion and make sure mass > 0
        position = gameObject.transform.position;
        if (mass <= 0)
        {
            mass = 1.0f;
        }
        gravity = new Vector3(0, -9.8f * mass / 4, 0);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //wait for 1 second so every node in the fabric can be loaded in
        if (Time.time >= 1)
        {
            //set postion to the actual poistion
            position = gameObject.transform.position;

            //if the mouse is moving the object ingonre the rest of the movement
            if (isMoved)
            {
                gameObject.transform.position = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y, 0);
            }
            //if the object is an anchor ignore the rest of movement
            else if (!isAnchor)
            {

                //basic euiler ingegration for movement
                acceleration = force / mass;
                acceleration = Vector3.ClampMagnitude(acceleration, maxSpeed * 10);
                velocity += acceleration * Time.deltaTime;

                velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
                //apply friction
                velocity *= .99f;// - ((velocity.magnitude / maxSpeed) * .05f);
                position += velocity * Time.deltaTime;


                gameObject.transform.position = position;
                //if the wall is enableled calculate the collision
                if (walls.activeInHierarchy)
                {
                    Vector3 mtv = walls.GetComponent<Wall>().checkCollision(this.gameObject);
                    //update position to remove penitration
                    position += mtv;
                    //if the penitration isn't zero apply frinction
                    if (mtv != Vector3.zero) { velocity *= .7f; }

                    gameObject.transform.position = position;
                }
                //chec if the circle is active in the heirarchy and resolve collsion
                if (Cirlces.activeInHierarchy)
                {
                    Cirlces.GetComponent<CircleCollision>().checkCollision(this.gameObject);

                }
                //reset the force
                force = gravity;

            }

        }
    }

    //mouse info for movement
    private void OnMouseDown()
    {

        force = Vector3.zero;
        isMoved = true;
        gameObject.transform.position = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y, 0);
        //Debug.Log(camera.ScreenToWorldPoint(Input.mousePosition));

    }

    //if the object is right clicked toggle weather its an anchor or not
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            changeAnchor();
        }
    }

    //mouse info for movement
    private void OnMouseUp()
    {
        isMoved = false;
        force = Vector3.zero;
    }

    //set the object as an anchor & change the sprite color
    void changeAnchor()
    {
        isAnchor = !isAnchor;
        SpriteRenderer mySpirite = GetComponent<SpriteRenderer>();
        if (isAnchor)
        {
            mySpirite.color = Color.blue;

        }
        else
        {
            mySpirite.color = Color.white;
        }
    }

    //swap if the object is drawn if its not disable thje sprite
    public void drawNode()
    {
        draw = !draw;
        if (!draw)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    //turn gravity on and off
    public void toggleGravity(bool change)
    {

        if (change)
        {
            gravity = new Vector3(0, -9.8f * mass / 4, 0);

        }
        else
        {
            gravity = Vector3.zero;
        }
    }


    // increase the force by an ammount but clamp the magnitude
    public void applyForce(Vector3 newForce)
    {
        force += newForce;
        force = Vector3.ClampMagnitude(force, maxForce);
    }
}
