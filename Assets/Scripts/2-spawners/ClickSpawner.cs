using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickSpawner : MonoBehaviour
{
    [SerializeField] protected InputAction spawnAction = new InputAction(type: InputActionType.Button);
    [SerializeField] protected GameObject[] lasers; // Array of different laser prefabs
    [SerializeField] protected Vector3 velocityOfSpawnedObject;
    [SerializeField] protected float spawnDelay; // Delay between spawns

    private bool canSpawn = true;
    private int shotCounter = 0;

    void OnEnable()
    {
        spawnAction.Enable();
    }

    void OnDisable()
    {
        spawnAction.Disable();
    }

    protected virtual GameObject spawnObject()
    {
        if (!canSpawn)
        {
            return null;
        }

        Debug.Log("Spawning a new object");

        // Determine which laser prefab to use based on shot count
        int lasersIndex = 0;
        if(shotCounter%5==0 && shotCounter!=0){lasersIndex=1;}

        // Step 1: spawn the new object.
        Vector3 positionOfSpawnedObject = transform.position;
        Quaternion rotationOfSpawnedObject = Quaternion.identity;
        GameObject newObject = Instantiate(lasers[lasersIndex], positionOfSpawnedObject, rotationOfSpawnedObject);

        // Step 2: modify the velocity of the new object.
        Mover newObjectMover = newObject.GetComponent<Mover>();
        if (newObjectMover)
        {
            newObjectMover.SetVelocity(velocityOfSpawnedObject);
        }

        StartCoroutine(SpawnCooldown());
        shotCounter += 1;
        return newObject;
    }

    IEnumerator SpawnCooldown()
    {
        canSpawn=false;
        yield return new WaitForSeconds(spawnDelay);
        canSpawn=true;
    }

    private void Update()
    {
        if (spawnAction.triggered)
        {
            spawnObject();
        }
    }
}
