using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the game manager oversees the other scripts and instanicantes all the objects
public class GameManager : MonoBehaviour
{
    //a refrecne to the node and spring prefab
    public GameObject nodeObj;
    public GameObject springPrefab;

    //the matrix of all the nodes stored like the fabric
    public GameObject[,] nodeMatrix;
    //a list of all springs and quads in the scene
    public List<GameObject> springs;
    public List<GameObject> quads;
    //the quads prefab
    public GameObject quadPrefab;

    //prefabs for collision boxes
    public GameObject wall;
    public GameObject Circle;
    //the number of rows and colloms in the fabric
    public int x;
    public int y;
    //the size and strength of each node
   public  float springSize;
   public  float springStrength;

    //info for diaagonal springs
    public bool diagonalEnabled;
    public List<GameObject> diagonalSprings;

    private void Start()
    {
        diagonalEnabled = true;
        //set the default values
        //these are good values for the simulation as some inputs can cause wierd movement
        x = 20;
        y = 10;
        springSize = 1.0f / 1.1f;
        springStrength = 10.5f;
        //loadScene();
    }

    //creates the fabric based off the inputs
    public void loadScene()
    {
        //create a new matrix of nodes based off x and y
        nodeMatrix = new GameObject[y, x];
        int l0 = nodeMatrix.GetLength(0);
        int l1 = nodeMatrix.GetLength(1);


        //nested loop to create the fabric
        for (int i = 0; i < l0; i++)
        {

            for (int j = 0; j < l1; j++)
            {
                float x = j - l1 / 2;
                float y = l0 / 2 - i;
                //create a new instance of a node at hte specific location name it and add the wall and circle to it
                GameObject tempObj = Instantiate(nodeObj, new Vector3(x / 2, y / 4+ 7, 0), Quaternion.identity);
                tempObj.name = "Node" + (i) + " " + (j);
                nodeMatrix[i, j] = tempObj;
                tempObj.GetComponent<NodeScrypt>().walls = wall;
                tempObj.GetComponent<NodeScrypt>().Cirlces = Circle;



            }
        }
        //loop through the matrix and attach a spring to each pair of nodes
        for (int i = 0; i < nodeMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < nodeMatrix.GetLength(1); j++)
            {
                //make the top row anchors for cloth
                if (i == 0)
                {
                    nodeMatrix[i, j].GetComponent<NodeScrypt>().isAnchor = true;
                }
                //attach a spring to the row abov it 
                else
                {
                    GameObject tempObj = Instantiate(springPrefab);
                    tempObj.GetComponent<SpringScript>().node1 = nodeMatrix[i, j];
                    tempObj.GetComponent<SpringScript>().node2 = nodeMatrix[i - 1, j];
                    tempObj.name = "spring: " + i + " " + j + " - " + (i - 1) + " " + (j);
                    tempObj.GetComponent<SpringScript>().startPlay(springSize, springStrength);
                    springs.Add(tempObj);
                }
                //attach a spring to every colum but the first
                if (j != 0)
                {
                    GameObject tempObj = Instantiate(springPrefab);
                    tempObj.GetComponent<SpringScript>().node1 = nodeMatrix[i, j];
                    tempObj.GetComponent<SpringScript>().node2 = nodeMatrix[i, j - 1];
                    tempObj.name = "spring: " + i + " " + j + " - " + i + " " + (j - 1);
                    tempObj.GetComponent<SpringScript>().startPlay(springSize, springStrength);
                    springs.Add(tempObj);
                    //attach a quad to the bottom right node & attach the two diaogonal springs
                    if (i != 0)
                    {
                        GameObject tempObj2 = Instantiate(springPrefab);
                        tempObj2.GetComponent<SpringScript>().node1 = nodeMatrix[i, j];
                        tempObj2.GetComponent<SpringScript>().node2 = nodeMatrix[i-1, j - 1];
                        tempObj2.name = "spring: " + i + " " + j + " - " + i + " " + (j - 1);
                        tempObj2.GetComponent<SpringScript>().startPlay(springSize, springStrength);
                        diagonalSprings.Add(tempObj2);
                        springs.Add(tempObj2);

                        GameObject tempQuad = Instantiate(quadPrefab, Vector3.zero, Quaternion.identity);
                        GameObject[] nodes = new GameObject[4];
                        nodes[0] = nodeMatrix[i, j-1 ];
                        nodes[1] = nodeMatrix[i, j];
                        nodes[2] = nodeMatrix[i-1, j-1];
                        nodes[3] = nodeMatrix[i-1 , j ];
                        tempQuad.GetComponent<ViewScript>().addNodes(nodes);
                        float r = ((float)j /(float) l1);
                        
                        float b = ((float)i / (float)l0);
                        tempQuad.GetComponent<ViewScript>().setColor(r, b);
                        quads.Add(tempQuad);
                    }
                    if( i != nodeMatrix.GetLength(0) -1)
                    {
                        GameObject tempObj2 = Instantiate(springPrefab);
                        tempObj2.GetComponent<SpringScript>().node1 = nodeMatrix[i, j];
                        tempObj2.GetComponent<SpringScript>().node2 = nodeMatrix[i + 1, j - 1];
                        tempObj2.name = "spring: " + i + " " + j + " - " + i + " " + (j - 1);
                        tempObj2.GetComponent<SpringScript>().startPlay(springSize, springStrength);
                        springs.Add(tempObj2);
                        diagonalSprings.Add(tempObj2);
                    }
                }
                


               
            }
        }
        //begin the sipmulation for each node
        for (int i = 0; i < nodeMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < nodeMatrix.GetLength(1); j++)
            {
                nodeMatrix[i, j].GetComponent<NodeScrypt>().beginSim();
            }
        }
        //disable the wall and circle
        wall.SetActive(false);
        Circle.SetActive(false);
    }

    //enable the diagonal springs
    public void enableDiagonal()
    {
        diagonalEnabled = !diagonalEnabled;
        for(int i = 0; i < diagonalSprings.Count; i++)
        {
            diagonalSprings[i].SetActive(diagonalEnabled);
        }
    }
    //draw the quads to the screen
    public void renderQuad(bool render)
    {
        for(int i =0; i < quads.Count; i++)
        {
            quads[i].SetActive(render);
        }
    }

    //draw the point controls to the screen
    public void renderNode(bool render)
    {
        for (int i = 0; i < nodeMatrix.GetLength(0); i++)
        {
            for(int j =0; j< nodeMatrix.GetLength(1); j++)
            {
                
                nodeMatrix[i, j].GetComponent<NodeScrypt>().drawNode();
            }
        }
    }
    //draw the spring to the screen
    public void renderSpring(bool render)
    {
        for(int i= 0; i < springs.Count; i++)
        {
            springs[i].GetComponent<SpringScript>().hasLines = !springs[i].GetComponent<SpringScript>().hasLines;
        }
    }
    //enable /disable gravity
    public void changeGravity(bool render)
    {
        
        for (int i = 0; i < nodeMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < nodeMatrix.GetLength(1); j++)
            {
                
                nodeMatrix[i, j].GetComponent<NodeScrypt>().toggleGravity(render);
            }
        }
    }
    //restart the sim
    public void resetSim()
    {
        for(int i = 0; i < quads.Count; i++)
        {
            Destroy(quads[i]);

        }
        quads = new List<GameObject>();
        for(int i =0; i < springs.Count; i++)
        {
            Destroy(springs[i]);
        }
        springs = new List<GameObject>();

        for(int i = 0; i < nodeMatrix.GetLength(0); i++)
        {
            for(int j = 0; j < nodeMatrix.GetLength(1); j++)
            {
                Destroy(nodeMatrix[i, j]);
            }
        }
       
        diagonalSprings = new List<GameObject>();
        nodeMatrix = new GameObject[0, 0];

        GetComponent<UIManager>().updateSettings();
        
    }
    

    // Update is called once per frame
    void Update()
    {

    }
}
