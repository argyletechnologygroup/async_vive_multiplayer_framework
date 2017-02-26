using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//TODO set or unset VR mode
		if( false ) {
			
		}
		//TODO load network controller
		
		//TODO setup network controller (set 
		//TODO load main menu objects
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void DisableVR () {
		UnityEngine.VR.VRSettings.enabled = false;
	}
	
	void EnableVR () {
		UnityEngine.VR.VRSettings.enabled = true;
	}
}
