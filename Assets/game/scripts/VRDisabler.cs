using UnityEngine;
using System.Collections;

public class VRDisabler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UnityEngine.VR.VRSettings.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
