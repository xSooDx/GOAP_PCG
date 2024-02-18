using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WoodCollectionArea : MonoBehaviour
{
    [TagSelector]
    public string collectionTag = "Wood";
    [SerializeField] int countToSpawn = 5;
    [SerializeField] List<GameObject> objectsToSpawn = new List<GameObject>();
    public int woodCount = 0;

    Bounds spawnBounds;
    int lastSpawnIndex = 0;


    private void Awake()
    {
        spawnBounds = GetComponent<BoxCollider>().bounds;
    }
    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody && other.attachedRigidbody.CompareTag(collectionTag))
        {
            Destroy(other.attachedRigidbody.gameObject);
            woodCount++;
            UIManager.Instance.AddWood();
            if (woodCount >= countToSpawn)
            {
                woodCount -= countToSpawn;
                SpawnItem();
            }
        }
    }

    private void SpawnItem()
    {
        Vector3 spawnOffset = Random.insideUnitSphere;
        spawnOffset.Scale(spawnBounds.extents);
        spawnOffset.y = 0;
        Instantiate(objectsToSpawn[lastSpawnIndex], transform.position + spawnOffset, Quaternion.identity);
        lastSpawnIndex = (lastSpawnIndex + 1) % objectsToSpawn.Count;
    }
}

