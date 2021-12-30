///This class autosetup the transform on the right hand to be used on CharacterAI to measures distance between hand and target being dragged to
///It could be used for a left hand (same script for both hands) by just adding a bool variable IsRightHand and adding another variable
///handLeftTransform to CharacterAI so we could drag character by both hands. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSetUprightHand : MonoBehaviour
{

    void Start()
    {
        Transform rootTransform = transform.root;
        Debug.Log(rootTransform.name);

        if (rootTransform.GetComponent<CharacterAI>() !=  null)
        {
            rootTransform.GetComponent<CharacterAI>().handTransform = transform;
           
        }
        else
        {
            Debug.LogWarning(gameObject.name + " cant find GetComponent<CharacterAI>() on root transform " + rootTransform.name);
        }
    }
}
