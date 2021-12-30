///This class handles GUI for this test, where we could select the three different scenarios requested on the test by just
///clicking on the GUI buttons, minimizing inspector usage
///Alejandro Munilla, Dec 30th, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class AnimatorGUI : MonoBehaviour
{
    private int buttonWidth;
    private int buttonHeight;
    private Rect talkRect;
    private Rect pointRect;
    private Rect toggleRect;
    private Rect toggleBoolRect;
    private Rect draggedRect;
    private Rect dragdollRect;
    public GameObject selectedAvatar;
    public GameObject targetForDragging;
    public bool beingDragged = false;


    private Animator animator;

    [SerializeField]
    private bool maskOn = false;

    void Start()
    {
        //we declare buttons this way to optimize GUI functions
        buttonHeight = (int)(Screen.height * 0.05f);
        buttonWidth = (int)(Screen.width * 0.12f);
        talkRect = new Rect(0, 0, buttonWidth, buttonHeight);
        pointRect = new Rect(buttonWidth, 0, buttonWidth, buttonHeight);
        toggleRect = new Rect(Screen.width - (2 * buttonWidth), 0, buttonWidth, buttonHeight);
        toggleBoolRect = new Rect(Screen.width - ( buttonWidth), 0, buttonWidth, buttonHeight);
        draggedRect = new Rect(Screen.width - (3 * buttonWidth), 0, buttonWidth, buttonHeight);
        dragdollRect = new Rect(Screen.width - (3 * buttonWidth), buttonHeight, buttonWidth, buttonHeight);
        animator = selectedAvatar.GetComponent<Animator>();
        maskOn = false;
        
        animator.SetLayerWeight(1, 0);                 //intially we want Mask layers not to affect Animator
        animator.SetLayerWeight(2, 0);

    }



    private void OnGUI()
    {
        //Potencially we could think on using Mouse Left Click to select Avatar and Mouse Left to cancel selection. selectedAvatar != null
        //Potentially we could use 3D models using old legacy Animation system (i got experience with that)
        if (selectedAvatar != null && selectedAvatar.GetComponent<Animator>() != null)
        {

            //So when it is in ragdoll mode on, animator is disable, we dont mess up things. 
            if (selectedAvatar.GetComponent<Animator>().enabled == true)
            {
                if (GUI.Button(talkRect, "TALK"))
                {
                    animator.SetTrigger("Talk");
                    //           Debug.Log("Talk!");
                    if (maskOn == true)
                    {
                        animator.SetLayerWeight(1, 0);
                        animator.SetLayerWeight(2, 1);
                    }
                }

                if (GUI.Button(pointRect, "POINT AT"))
                {
                    animator.SetTrigger("Point");
                    //            Debug.Log("Point!");
                    if (maskOn == true)
                    {
                        animator.SetLayerWeight(1, 1);
                        animator.SetLayerWeight(2, 0);
                    }
                }

                if (GUI.Button(toggleRect, "TOGGLE MASK "))
                {
                    ToggleMask("toggle");
                    CharacterAI cAI = selectedAvatar.GetComponent<CharacterAI>();
                    beingDragged = false;
                    cAI.target = null;
                }

                GUI.Label(toggleBoolRect, maskOn.ToString());

                if (GUI.Button(draggedRect, "BEING DRAGGED"))
                {
                    ToggleBeingDragged(!beingDragged);

                }
            }
               

            if (GUI.Button (dragdollRect, "DRAGDOLL"))
            {
                if (selectedAvatar.GetComponent<Animator>().enabled == true)
                {
                    animator.SetLayerWeight(1, 0);
                    animator.SetLayerWeight(2, 0);
                    animator.SetTrigger("IdleTrigger");
                    Invoke("DelayedDisableAnim", 0.1f);
                    CharacterAI cAI = selectedAvatar.GetComponent<CharacterAI>();
                    cAI.state = CharacterAI.State.ragdoll;
                    cAI.RigOffTrigger();
                    selectedAvatar.GetComponent<Animator>().enabled = false;
                    

                }
                else
                {
                    selectedAvatar.GetComponent<CharacterAI>().state = CharacterAI.State.idle;
                    selectedAvatar.transform.Find("EthanSkeleton/EthanHips").gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    selectedAvatar.GetComponent<Animator>().enabled = true;
                 
                }

            }
        }
    }

    //This funcion toggle on and off Masks at the same time. Each mask is controlled by a different trigger.  
    private void ToggleMask (string changeTo)
    {

        if (changeTo == "toggle")
        {
            if (maskOn == true)
            {
                maskOn = false;
                animator.SetLayerWeight(1, 0);
                animator.SetLayerWeight(2, 0);
            }
            else
            {
                maskOn = true;

            }
            

            Debug.Log("Toggle");
        }
        else if (changeTo == "true")
        {
            ToggleBeingDragged(false);
            maskOn = true;
        }
        else if (changeTo == "false")
        {
            maskOn = false;
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
        }

        animator.SetBool("MaskLayer", maskOn);
        ToggleBeingDragged(false);


    }

    //We make sure Masks will not interact with custom rigged animation. 
     private void ToggleBeingDragged (bool toggleState)
    {
 
        CharacterAI cAI = selectedAvatar.GetComponent<CharacterAI>();
        cAI.target = targetForDragging;        
 
        if (toggleState == true)
        {
            beingDragged = true;
            if (cAI.enabled == false)
            {
                
                cAI.enabled = true;
            }
            Debug.Log("Being Dragged");
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
            animator.SetTrigger("IdleTrigger");
        }
    }

    private void DelayedDisableAnim ()
    {
        selectedAvatar. transform.Find("EthanSkeleton/EthanHips").gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
