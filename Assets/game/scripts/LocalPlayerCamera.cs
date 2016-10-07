using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Assertions;

[UnityEngine.RequireComponent(typeof(Camera))]
public class LocalPlayerCamera : MonoBehaviour
{
	public NetworkBehaviour owner;

	// Use this for initialization
	void Start () {
		Assert.IsNotNull(owner);
		GetComponent<Camera>().enabled = owner && owner.isLocalPlayer;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
