using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private Animator myAnimator;
    private GameObject myParentHex;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myParentHex = transform.parent.gameObject;
        RunAnimation(myParentHex.tag);
    }
    private void RunAnimation(string aniToUse)
    {
        switch(aniToUse)
        {
            case ("TopDotPos"):
                myAnimator.Play("TopDotSpace");
                break;
            case ("RightDotPos"): //Currently in a hex offset to the right
                myAnimator.Play("RightToLeftDot");
                break;
            case ("LeftDotPos"):  //Currently in a hex offset to the left
                myAnimator.Play("LeftToRightDot");
                break;
            default:
                break;
        }
    }
    public void CommandAnimation(string aniToUse)
    {
        RunAnimation(aniToUse);
    }  
}
