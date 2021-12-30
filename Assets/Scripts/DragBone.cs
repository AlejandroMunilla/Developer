using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBone : MonoBehaviour
{

    public Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {
        targetTransform.LookAt(targetTransform);
    }

    private void LateUpdate()
    {
        if (targetTransform != null)
        {
            targetTransform.LookAt (targetTransform);

            //     transform.Translate(Vector3.Normalize(targetTransform.position - transform.position) );
            Vector3 direction = targetTransform.position - transform.position;
            GetComponent<Rigidbody>().AddRelativeForce(direction.normalized * 2, ForceMode.Force);
        }
    }
}
