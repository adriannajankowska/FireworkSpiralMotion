using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIManager : MonoBehaviour
{
    public GameObject distanceValue;
    private TMP_Text textmeshPro; //GUI element on canvas

    private void Start()
    {
        textmeshPro = distanceValue.GetComponent<TMP_Text>();
    }

    //setting the GUI text content
    public void SetText(string text)
    {
        textmeshPro.text = text;
    }
}
