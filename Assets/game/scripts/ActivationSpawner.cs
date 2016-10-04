using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
/// <summary>
/// Spawns child objects on activation
/// </summary>
public class ActivationSpawner : MonoBehaviour {

    public Transform[] objectsToSpawnOnActivate;
    public bool selfRemoveAfterCreation;
    public bool attachObjectsAsChildren;

    void OnEnable()
    {
        foreach(Transform objectToSpawnOnActivate in objectsToSpawnOnActivate)
        {
            Assert.IsNotNull(objectToSpawnOnActivate);
            if (attachObjectsAsChildren)
            {
                Instantiate(objectToSpawnOnActivate, GetComponent<Transform>());
            }
            else
            {
                Instantiate(objectToSpawnOnActivate);
            }
        }

        if (selfRemoveAfterCreation)
        {
            Destroy(this);
        }
    }
}
