using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColumnManager : MonoBehaviour
{
    private List<GameObject> columnListOfHexs;
    private void Awake()
    {
        columnListOfHexs = new List<GameObject>();
        AddAllHexPointsToList();
        columnListOfHexs = OrderedList(columnListOfHexs);
    }
    private void Start()
    {
        CreateAllDots();
    }
    private void Update()
    { 
        CreateDotsUntilFull();  
    }

    private void AddAllHexPointsToList()//Adding all hex points in column to a list 
    {
        foreach (Transform hex in transform) //transform = this column
        {
            columnListOfHexs.Add(hex.gameObject);
        }
    }
    private List<GameObject> OrderedList(List<GameObject> listToBeOrdered)
    {
        //Order the column list by name(0,1,2,3,4,5)
        return listToBeOrdered = columnListOfHexs.OrderBy(hex => hex.name).ToList();
    }
    private void CreateDotsUntilFull() //Creates only missing dots
    {   
        int previousHexIndex = 0;
        for(int x = 0; x < columnListOfHexs.Capacity; x++)
        {
            GameObject hex = columnListOfHexs[x];
            //Check the current hex and the previous hex for dots
            if (hex.transform.childCount == 0 && 
                columnListOfHexs[previousHexIndex].transform.childCount == 1)
            {
                //Debug.Log("Empty Hex detected; relocating above hex to my location");
                DotCreation dotCreationScript = hex.GetComponent<DotCreation>();
                dotCreationScript.NewDotReplacement(columnListOfHexs[previousHexIndex].transform.GetChild(0),hex.transform);
            }
            //Check current hex for dot
            if (hex.transform.childCount == 0)
            {
                //Debug.Log("Empty Hex detected!!");
                DotCreation dotCreationScript = hex.GetComponent<DotCreation>();
                dotCreationScript.NewDotCreation();
            }
            //Iterate previous hex after the first
            if(x >= 1)
            {
                previousHexIndex++;
            }
        }
    }
    private void CreateAllDots() //ALWAYS CREATES ALL DOTS POSSIBLE
    {
        foreach (GameObject hex in columnListOfHexs)
        { 
            DotCreation dotCreationScript = hex.GetComponent<DotCreation>();
            dotCreationScript.NewDotCreation();    
        }
    }
}
