using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pickup : MonoBehaviour
{
    public Material outlineMaterial;
    // Current object we are looking at
    private GameObject raycastedObject;

    private bool outlined;

    void FixedUpdate()
    {

        RaycastHit hit;
        Vector3 fwd = Camera.main.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, 100) && hit.transform.gameObject.CompareTag("Pickup"))
        {
            Debug.Log(hit.transform.name);
            // previously raycasted object is not equal to the current object
            if (hit.transform.gameObject && !(raycastedObject == hit.transform.gameObject))
            {
                if (raycastedObject)
                {
                    RevertMaterial(raycastedObject);
                }

                if (!outlined)
                {
                    AddOutline(hit);
                }
                // Sets the currently raycasted object to the new hit
                raycastedObject = hit.transform.gameObject;
            }
            
        }
        else
        {
            if (raycastedObject)
            {
                RevertMaterial(raycastedObject);
            }

            raycastedObject = null;
        }
    }

    // Handling reverting the old raycasted objects materials to non - outline
    void RevertMaterial(GameObject objectToRevert)
    {
        Material oldObjectMaterial = objectToRevert.transform.GetComponent<Renderer>().material;
        Material[] oldObjectMaterialArray = { oldObjectMaterial };
        objectToRevert.GetComponent<Renderer>().materials = oldObjectMaterialArray;
        outlined = false;
    }

    // Adds outline to the new raycast hit
    void AddOutline(RaycastHit hit)
    {
        Material temp = hit.transform.GetComponent<Renderer>().material;
        Material[] tempArray = { temp, outlineMaterial };
        hit.transform.GetComponent<Renderer>().materials = tempArray;
        outlined = true;

    }
}
