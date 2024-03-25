using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MotionController : MonoBehaviour
{
    public GameObject InputManager; //reference to the globally used Input Manager
    private bool shouldMove; //local copy of the flag from Input Manager

    public GameObject GUIManager; //reference to the globally used GUI Manager
    private GameObject distanceValueGameObject;

    public GameObject fireworksParticle; //reference to the fireworks particle system game object

    public Transform target; //point in space which the object will revolve around

    private float currentSpeed = 0; //Object's current currentSpeed, 0 at the very start
    public float accelerationRate = 5; //How fast will object reach a maximum currentSpeed
    private float deaccelerationRate = 40; //How fast will object reach a currentSpeed of 0 (the greater value the greater deacceleration)
    
    public float maxSpeed = 3f; //maximum velocity of the object
    private float radius; //radius of the object's circular path
    public float initialRadius = 1f;
    private float angle = 0f; //starting angle, not important in this exercise
    public float displayTime = 5f; //for how many secodns should the travelled distance appear

    private Vector3 startPosition; //object's start position in space after constructing
    private Vector3 lastPosition; //object's position after each frame, stored, used and immediately overwritten
    private float distanceTravelled = 0f; //distance covered by the object from the startPosition to the current moment

    private Material mat; //object's material

    private void Start()
    {
        radius = initialRadius;
        //set up object's starting position according to the set target and set radius
        float x = target.position.x + Mathf.Cos(angle) * radius;
        float y = transform.position.y + currentSpeed * Time.deltaTime;
        float z = target.position.z + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, y, z);

        //saving the object's starting position
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        PrintVector3("Start position: ", startPosition);
        lastPosition = startPosition;

        mat = GetComponent<Renderer>().material;

        distanceValueGameObject = GUIManager.GetComponent<GUIManager>().distanceValue;
    }

    void Update()
    {
        shouldMove = InputManager.GetComponent<InputManager>().shouldMove; //this could be done better...
        if (shouldMove == true)
        {
            StartCoroutine(DisplayAndWait());
        }
        Move();
        MeasureDistance();
        ChangeColor();
        if (radius <= 0) Explode();
    }

    void ShrinkObject()
    {
        transform.localScale -= new Vector3(0.001f, 0.001f, 0.001f);
    }
    void ShrinkRadius()
    {
        radius -= initialRadius*0.001f;
    }

    //move the object
    void Move()
    {
        //accelarate by accelarationRate to reach maxSpeed
        //meanwhile shrink the object and the radius
        if (shouldMove) {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += accelerationRate * Time.deltaTime;
            }
            ShrinkObject();
            ShrinkRadius();
        }
        else
        {
            currentSpeed = 0;
        }
        //update the object's position
        float x = target.position.x + Mathf.Cos(angle) * radius;
        float y = (transform.position.y + currentSpeed * Time.deltaTime)*1;
        float z = target.position.z + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, y, z);
        angle += currentSpeed * Time.deltaTime;
    }

    //measure the distance travelled (from start to current position) and display the number in GUI
    void MeasureDistance()
    {
        distanceTravelled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (shouldMove == false)
        {
            distanceValueGameObject.SetActive(true);
            GUIManager.GetComponent<GUIManager>().SetText(distanceTravelled.ToString());
        }
        else
        {
            distanceValueGameObject.SetActive(false);
        }
    }
    

    //lerp continuously between two colors based on the Z position of the object
    void ChangeColor()
    {
        Color lerpedColor = Color.Lerp(Color.green, Color.red, 1.0f - transform.localScale.x);
        mat.color = lerpedColor;
    }
    

    //instantiate fireworks game object and play its particle system
    void Explode()
    {
        GameObject firework = Instantiate(fireworksParticle, transform.position, Quaternion.identity);
        firework.GetComponent<ParticleSystem>().Play();
        Destroy(this);
    }

    //display current distance measurements and wait for N seconds, continue movement
    IEnumerator DisplayAndWait()
    {
        yield return new WaitForSeconds(displayTime);
        InputManager.GetComponent<InputManager>().shouldMove = true;
    }

    //Special method to log Vector3 value as classic Debug.Log concatenation shortens the Vector3 values to max 1 decimal
    void PrintVector3(string message, Vector3 vector3Value)
    {
        Debug.Log(message + "X: " + vector3Value.x + "  Y: " + vector3Value.y + "  Z:" + vector3Value.z);
    }
}

