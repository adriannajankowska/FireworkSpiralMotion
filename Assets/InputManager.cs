using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public bool shouldMove = false; //flag whether the object should start/stop movement

    public Button startButton; //GUI element on canvas

    void Start()
    {
        startButton.onClick.AddListener(TaskOnClick);
    }

    void Update()
    {
        //listening to Space down push
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldMove = false;
        }
    }
    //setting the movement flag to true 
    void TaskOnClick()
    {
        shouldMove = true;
        Debug.Log("shouldMove = true");
        startButton.enabled = false;
    }
}
