using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory
{
    // ToDo: objMemory are linked to states. Need to set and clear memory accordingly
    public Dictionary<string, GameObject> objMemory;

    public Memory()
    {
        objMemory = new Dictionary<string, GameObject>();
    }
}

[RequireComponent((typeof(GAgent)))]
public class GSensor : MonoBehaviour
{
    public float sensorRadius = 10f;
    public LayerMask sensorLayerMask;
    public float timeBetweenChecks = 0.1f;

    [SerializeField] Transform pickupHolder;

    public Memory memory;

    GAgent attachedAgent;


    private void Awake()
    {
        attachedAgent = GetComponent<GAgent>();
        memory = new Memory();
    }

    private void Start()
    {
        StartCoroutine(SensorCheck());
    }

    IEnumerator SensorCheck()
    {
        while (true)
        {
            HashSet<GameObject> sensedObjects = new HashSet<GameObject>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, sensorRadius, sensorLayerMask);

            foreach (Collider collider in colliders)
            {
                GameObject obj = collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;
                if (!Physics.Raycast(transform.position, obj.transform.position, out RaycastHit hit, sensorRadius))
                {
                    sensedObjects.Add(obj);
                }
            }
            yield return null;

            foreach (GameObject obj in sensedObjects)
            {
                CheckObject(obj);
            }

            if (memory.objMemory.ContainsKey("Player") && !sensedObjects.Contains(memory.objMemory["Player"]))
            {
                memory.objMemory.Remove("Player");
                attachedAgent.agentBeliefs.SetState(EState.SeesPlayer, 0);
            }

            yield return new WaitForSeconds(timeBetweenChecks);
        }
    }

    public bool CheckObject(GameObject obj, string trueIfTag = null)
    {
        if (obj.CompareTag("Tree"))
        {
            memory.objMemory[obj.tag] = obj;
            attachedAgent.agentBeliefs.SetState(EState.SeenTree, 1);
        }
        else if (obj.CompareTag("Player"))
        {

            memory.objMemory[obj.tag] = obj;
            attachedAgent.agentBeliefs.SetState(EState.SeesPlayer, 1);

        }
        else if (obj.CompareTag("Wood"))
        {
            memory.objMemory[obj.tag] = obj;
            attachedAgent.agentBeliefs.SetState(EState.SeesWood, 1);
        }

        return trueIfTag == null ? true : obj.CompareTag(trueIfTag);
    }

    public void RemoveObjFromMemory(GameObject obj)
    {
        if (memory.objMemory.ContainsKey(obj.tag) && memory.objMemory[obj.tag] == obj)
        {
            memory.objMemory.Remove(obj.tag);
        }
    }
}
