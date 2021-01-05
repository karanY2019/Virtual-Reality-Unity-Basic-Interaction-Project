using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar;

public class GlueGunBehavior : MonoBehaviour
{
    OVRGrabbable m_GrabState;

    //The interactive area that would be activated when pressing down the trigger while grabbing the gluegun
    [SerializeField]
    GameObject m_GlueZone;

    private void Awake()
    {
        //Get component of the OVRGrabbable
        m_GrabState = this.GetComponent<OVRGrabbable>();
    }

    private void FixedUpdate()
    {
        //If the gluegun is being grabbed, the gluezone is active while the trigger is pressed
        if (m_GrabState.isGrabbed)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                m_GlueZone.SetActive(true);
                m_GlueZone.GetComponent<MeshRenderer>().enabled = false;
                m_GlueZone.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                m_GlueZone.SetActive(false);
            }
        }
        else
        {
            m_GlueZone.SetActive(false);
        }
    }
}
