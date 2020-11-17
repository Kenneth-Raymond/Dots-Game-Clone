using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DotCreation : MonoBehaviour
{
    private Color myColor;
    private Color[] colors;
    private int randomNumberRange;
    private GameObject activeDot;
    //Attached in inspector
    [SerializeField]
    private GameObject dotPrefab;
    private void Awake()
    {
        AssignColors();
    }
    private void AssignColors()
    {
        colors = new Color[4];
        colors[0] = Color.blue;
        colors[1] = Color.green;
        colors[2] = Color.red;
        colors[3] = Color.yellow;
        randomNumberRange = colors.Length;
    }
    private Color RandomColor()
    {
        int randomNumber = Random.Range(0, randomNumberRange);
        return colors[randomNumber];
    }
    public void NewDotCreation()
    {
        myColor = RandomColor();
        activeDot = Instantiate(dotPrefab,transform);
        activeDot.name = "D " + name;
        activeDot.GetComponent<SpriteRenderer>().color = myColor;
    }
    public void NewDotReplacement(Transform dot, Transform originHex)
    {
        Dot dotScript = dot.GetComponent<Dot>();
        switch (originHex.tag)
        { 
            case ("TopDotPos"):
                //myAnimator.Play("TopDotSpace");
                dotScript.CommandAnimation("TopDotPos");
                break;
            case ("RightDotPos"): //Right-most grid space to left
                //Debug.Log("I am a right dot!");
                dot.transform.parent = transform;
                dotScript.CommandAnimation("LeftDotPos");
                break;
            case ("LeftDotPos"): //Left-most grid space to right
                //Debug.Log("I am a left dot!");
                dot.transform.parent = transform;
                dotScript.CommandAnimation("RightDotPos");
                break;
            default:
                break;
        }
    }  
}
