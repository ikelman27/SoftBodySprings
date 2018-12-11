using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Mangaes the ui that changes when the simulation
public class UIManager : MonoBehaviour
{

    //sliders & input fields
    public Slider springSize;
    public Slider springStrength;
    public Slider verticalNodes;
    public Slider horizontalNodes;

    public Text sizeField;
    public Text strengthField;
    public Text vertField;
    public Text horizField;
    public Text fps;
    public GameObject inGameField;
    float deltaTime = 0;
    public Camera camera;


    public float[] previousSettings;
    // Use this for initialization
    void Start()
    {
        //settings to determine which ui elemetn was changed
        camera = Camera.main;
        previousSettings = new float[4];
        previousSettings[0] = springSize.value;
        previousSettings[1] = springStrength.value;
        previousSettings[2] = verticalNodes.value;
        previousSettings[3] = horizontalNodes.value;


    }

    // Update is called once per frame
    void Update()
    {
       
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fpsF = 1.0f / deltaTime;
        fps.text = Mathf.Ceil(fpsF).ToString();
        //move the camera based off of input
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            camera.orthographicSize++;
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            if (camera.orthographicSize > 1)
            {
                camera.orthographicSize--;
            }
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            var pos = camera.transform.position;
            var scale = camera.orthographicSize / 8;
            pos = new Vector3(pos.x, pos.y + (.02f * scale * Time.deltaTime * 100), pos.z);
            camera.transform.position = pos;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            var pos = camera.transform.position;
            var scale = camera.orthographicSize / 8;
            pos = new Vector3(pos.x, pos.y - (.03f * scale * Time.deltaTime * 100), pos.z);
            camera.transform.position = pos;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            var pos = camera.transform.position;
            var scale = camera.orthographicSize / 8;
            pos = new Vector3(pos.x - (.03f * scale * Time.deltaTime * 100), pos.y, pos.z);
            camera.transform.position = pos;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            var pos = camera.transform.position;
            var scale = camera.orthographicSize / 8;
            pos = new Vector3(pos.x + (.03f * scale * Time.deltaTime * 100), pos.y, pos.z);
            camera.transform.position = pos;

        }

    }

    //resets the game with new paramiters
    public void updateSettings()
    {
        GetComponent<GameManager>().x = (int)horizontalNodes.value;
        GetComponent<GameManager>().y = (int)verticalNodes.value;
        GetComponent<GameManager>().springSize = springSize.value;
        GetComponent<GameManager>().springStrength = springStrength.value;
        //reset the toggles on the right side of the screen
        for (int i = 0; i < inGameField.transform.childCount; i++)
        {
            if (i <= 3)
            {
                inGameField.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
            }
            else
            {
                inGameField.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
            }
        }

        GetComponent<GameManager>().loadScene();
    }


    //check which of the sliders has been changed and update the view by that ammount
    //limmint each variable by .1 or 1 respectively
    public void changeSetting(GameObject obj)
    {
        if (springSize.value != previousSettings[0])
        {
            float value = springSize.value;
            value = Mathf.Round(value / .1f) * .1f;
            springSize.value = value;
            sizeField.text = springSize.value.ToString("n1");
            previousSettings[0] = springSize.value;
            return;
        }
        else if (springStrength.value != previousSettings[1])
        {
            float value = springStrength.value;
            value = Mathf.Round(value / .1f) * .1f;
            springStrength.value = value;
            strengthField.text = springStrength.value.ToString("n1");
            previousSettings[1] = springStrength.value;
            return;
        }
        else if (verticalNodes.value != previousSettings[2])
        {
            vertField.text = verticalNodes.value.ToString();
            previousSettings[2] = verticalNodes.value;
            return;
        }
        else if (horizontalNodes.value != previousSettings[3])
        {
            horizField.text = horizontalNodes.value.ToString();
            previousSettings[3] = horizontalNodes.value;
            return;
        }

    }


}
