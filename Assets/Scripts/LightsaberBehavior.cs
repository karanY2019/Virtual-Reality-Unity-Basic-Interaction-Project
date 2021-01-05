using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar;

public class LightsaberBehavior : MonoBehaviour
{
    //Accessing the script that take care of lightsaber's grabbing state
    OVRGrabbable m_GrabState;

    //The Quillon that already installed on the handle. Should be inactive at the begginning of the game
    [SerializeField]
    GameObject m_LightsaberQuillonInstalled;
    //The Quillon module that has not been installed yet.
    [SerializeField]
    GameObject m_LightsaberQuillonModule;
    //The active area to snap the quillon module to the handle
    [SerializeField]
    GameObject m_QuillonConnectZone;
    bool m_QuillonIsInstalled;

    //The Power that already installed on the handle. Should be inactive at the beginning of the game
    [SerializeField]
    GameObject m_LightsaberPowerInstalled;
    //The Power module that has not been installed yet
    [SerializeField]
    GameObject m_LightsaberPowerModule;
    //The active area to snap the power module to the handle
    [SerializeField]
    GameObject m_PowerConnectZone;
    bool m_PowerIsInstalled;

    //bool to signal if the lightsaber is done assembling
    bool m_LightsaberIsAssembled;

    //The blade that already installed on the handle
    [SerializeField]
    GameObject m_LightsaberBlade;
    [SerializeField]
    float m_LightsaberLength = 1f;
    [SerializeField]
    float m_BladeSmooth = 1f;
    bool m_BladeIsActivated;

    //Final Five: add sound effect 
    AudioSource audioSource;
    public AudioClip saberMoving;
    public AudioClip saberOut, saberBack;
    public AudioClip PowerOn, QuillonOn;
    
    private void Awake()
    {
        //[TODO]Getting the info of OVRGrabbable
        m_GrabState = this.GetComponent<OVRGrabbable>();
        m_QuillonIsInstalled = false;
        m_PowerIsInstalled = false;
        m_BladeIsActivated = false;

        //Final Five
        audioSource = this.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        //[TODO]Step one: check if the power is connected.
        if (!m_PowerIsInstalled)
        {
            ConnectingPower();
        }


        //[TODO]Step two: check in the Quillon is connected.
        if (m_PowerIsInstalled)
        {
            
            if (!m_QuillonIsInstalled)
            {
                
                ConnectingQuillon();
            }            
        }


        //[TODO]Once the lightsaber is done assembling, set the blade GameObject active.
        if (m_PowerIsInstalled)
        {
            if (m_QuillonIsInstalled )
            {
                
                m_LightsaberBlade.SetActive(true);
            }
        }
        
        //[TODO]If the lightsaber is done assembled, change bladeIsActivated after pressing the A button on the R-Controller while the player is grabbing it
        if (m_GrabState.isGrabbed)
        {
            if (OVRInput.Get(OVRInput.Button.One))
            {
                m_BladeIsActivated = !m_BladeIsActivated;
                if (m_BladeIsActivated)
                {
                    audioSource.PlayOneShot(saberOut);               
                }
                else
                {
                    audioSource.PlayOneShot(saberBack);
                }
            }
        }
        SetBladeStatus(m_BladeIsActivated);
    }

    void ConnectingPower()
    {

        //get the connector state of power
        //if it is connected:
        if (m_PowerConnectZone.GetComponent<LightsaberModuleConnector>().isConnected)
        {
        //activate the pre-installed power part on the handle
            m_LightsaberPowerInstalled.SetActive(true);

        //simply make the power module "invisible" by switching off its mesh renderer
            m_LightsaberPowerModule.GetComponent<Renderer>().enabled = false;

        //we dont need the connect area anymore so switch it off
            m_PowerConnectZone.SetActive(false);
            m_PowerIsInstalled = true;
            audioSource.PlayOneShot(PowerOn);
        }
    }

    void ConnectingQuillon()
    {

        //same process as in power connection 
        if (m_QuillonConnectZone.GetComponent<LightsaberModuleConnector>().isConnected)
        {
            m_LightsaberQuillonInstalled.SetActive(true);
            m_LightsaberQuillonModule.GetComponent<Renderer>().enabled = false;
            m_QuillonConnectZone.SetActive(false);
            m_QuillonIsInstalled = true;
            audioSource.PlayOneShot(QuillonOn);
        }        
    }

    void SetBladeStatus(bool bladeStatus)
    {
        if(!bladeStatus)
        {
            //Lightsaber goes back
            m_LightsaberBlade.transform.localScale = new Vector3(0f, m_LightsaberBlade.transform.localScale.y, m_LightsaberBlade.transform.localScale.z);
        }

        if(bladeStatus)
        {
            //Lightsaber pulls out
            m_LightsaberBlade.transform.localScale = new Vector3(0.5f, m_LightsaberBlade.transform.localScale.y, m_LightsaberBlade.transform.localScale.z);
        }
    }
}
