using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A script that draw's the quads that are used for the texture in the game
public class ViewScript : MonoBehaviour {
    public List<GameObject> nodes;
    int[] tri;
	// Use this for initialization
	void Start () {
        //a list of points to draw the triangles right hand notiion
        tri = new int[12] { 0, 2, 1, 2, 3, 1 , 1, 2, 0, 1, 3, 2};
        


    }
    //sets the rgb color of the node
    public void setColor(float r, float b)
    {
        Renderer rend = GetComponent<Renderer>();
       

        rend.material.color = new Color(r, .4f, b, .5f);
    }
	
	// Update is called once per frame
	void Update () {
      
        //if their are nodes(after start, but before addnodes is called) update the points of the quad
		if(nodes.Count > 0)
        {
           
            List<Vector3> nodePos = new List<Vector3>();
            for(int i = 0; i < 4; i++)
            {
                nodePos.Add(nodes[i].transform.position);
               
            }
           
           //set the mesh's corners to the nodes, and set the triangles to draw
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.SetVertices(nodePos);
            mesh.triangles = tri;
        }
	}

    //adds ndoes to the object
    public void addNodes(GameObject[] newNodes)
    {
        for(int i = 0; i < newNodes.Length; i++)
        {
            nodes.Add(newNodes[i]);
            
        }
        
       
    }
}
