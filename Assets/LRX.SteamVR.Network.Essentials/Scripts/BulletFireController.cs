using UnityEngine;
using UnityEngine.Networking;


public class BulletFireController : NetworkBehaviour
{
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
    public string fireKeyName;


	public override void OnStartLocalPlayer()
	{
		GetComponent<Renderer>().material.color = Color.blue;
	}

	void Update()
	{
		if (isLocalPlayer)
		{
            LocalPlayerUpdate();
        }
        else
        {
            RemotePlayerUpdate();
        }
    }
    
    void RemotePlayerUpdate()
    {
    }

    void LocalPlayerUpdate()
    {
		// common input here
		if (Input.GetButtonDown(fireKeyName))
		{
			CmdFire();
		}
    }


    [Command]
	/// Fires a projectile, by sending the command to the server where it is executed.
	protected void CmdFire()
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
}