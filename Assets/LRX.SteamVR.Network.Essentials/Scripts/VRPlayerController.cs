using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class VRPlayerController : NetworkBehaviour
{
	public GameObject vrCameraRig;
	public GameObject leftHandPrefab;
    public GameObject rightHandPrefab;
	private GameObject vrCameraRigInstance;
	private GameObject leftHand = null;
	private GameObject rightHand = null;

	public override void OnStartLocalPlayer ()
	{
		if (!isLocalPlayer)
			return;
        // delete main camera
        if (Camera.main)
        {
            DestroyImmediate(Camera.main.gameObject);
        }

		// create camera rig and attach player model to it
		vrCameraRigInstance = (GameObject)Instantiate (
			vrCameraRig,
			transform.position,
			transform.rotation);

		Transform bodyOfVrPlayer = transform.FindChild ("VRPlayerBody");
		if (bodyOfVrPlayer != null)
			bodyOfVrPlayer.parent = null;

		GameObject head = vrCameraRigInstance.GetComponentInChildren<SteamVR_Camera> ().gameObject;
		transform.parent = head.transform;
        transform.localPosition = new Vector3(0f, -0.03f, -0.06f);
		
		StartCoroutine(TryDetectControllers());
	}

	IEnumerator TryDetectControllers ()
	{
		SteamVR_TrackedObject[] controllers;
		bool firstRun = true;
		do{
			if(!firstRun){
                yield return new WaitForSeconds(2);
            }
            firstRun = false;
            controllers = vrCameraRigInstance.GetComponentsInChildren<SteamVR_TrackedObject> ();
		}while(controllers == null || controllers.Length < 1  || controllers[0] == null);
		CmdSpawnLeftHand(netId);
		
		//attempt to spawn right hand
		firstRun = true;
		do
        {
            if (!firstRun)
            {
                yield return new WaitForSeconds(2);
            }
            firstRun = false;
            controllers = vrCameraRigInstance.GetComponentsInChildren<SteamVR_TrackedObject> ();
		}while(controllers == null || controllers.Length < 2  || controllers[1] == null);
		CmdSpawnRightHand(netId);
	}
	
	GameObject SpawnHand(NetworkInstanceId playerId, GameObject handPrefab, HandSide side){
        // instantiate controllers
        // tell the server, to spawn two new networked controller model prefabs on all clients
        // give the local player authority over the newly created controller models
		GameObject hand = Instantiate(handPrefab);
		var vrHand = hand.GetComponent<NetworkVRHands> ();
		vrHand.side = side;
		vrHand.ownerId = playerId;
		NetworkServer.SpawnWithClientAuthority (hand, base.connectionToClient);
		return vrHand.gameObject;
	}
	
	[Command]
	void CmdSpawnLeftHand(NetworkInstanceId playerId)
	{
		SpawnHand(playerId, leftHandPrefab, HandSide.Left);
	}

	[Command]
	void CmdSpawnRightHand(NetworkInstanceId playerId)
	{
		SpawnHand(playerId, rightHandPrefab, HandSide.Right);
	}

	[Command]
	public void CmdGrab(NetworkInstanceId objectId, NetworkInstanceId controllerId)
	{
		var iObject = NetworkServer.FindLocalObject (objectId);
		var networkIdentity = iObject.GetComponent<NetworkIdentity> ();
        networkIdentity.AssignClientAuthority(connectionToClient);

        var interactableObject = iObject.GetComponent<InteractableObject>();
        interactableObject.RpcAttachToHand (controllerId);    // client-side
        var hand = NetworkServer.FindLocalObject(controllerId);
        interactableObject.AttachToHand(hand);    // server-side
    }

	[Command]
	public void CmdDrop(NetworkInstanceId objectId, Vector3 currentHolderVelocity)
	{
		var iObject = NetworkServer.FindLocalObject (objectId);
		var networkIdentity = iObject.GetComponent<NetworkIdentity> ();
        networkIdentity.RemoveClientAuthority(connectionToClient);
        
        var interactableObject = iObject.GetComponent<InteractableObject>();
        interactableObject.RpcDetachFromHand(currentHolderVelocity); // client-side
        interactableObject.DetachFromHand(currentHolderVelocity); // server-side
    }
}
