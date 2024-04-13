using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct MemData
{
    [TagSelector]
    public string tag;

    public EState state;
}



[RequireComponent((typeof(GAgent)))]
public class GSensor : MonoBehaviour
{
    public List<MemData> TagsToSense = new();

    public float sensorRadius = 10f;
    public LayerMask sensorLayerMask;
    public LayerMask obstacleMask;
    public float timeBetweenChecks = 0.1f;
    //public float visionConeHalfRadius = 60f;
    public float closeRadius = 5f;
    [SerializeField] Transform pickupTransform;
    [SerializeField] SmartObject pickedUpObject;

    public Dictionary<string, GameObject> ObjMemory { get; private set; } = new();

    readonly Dictionary<string, EState> senseTagsToStateDict = new();

    GAgent attachedAgent;

    string pickedUpTag;

    public bool HasPickup
    {
        get
        {
            return pickedUpObject != null;
        }
    }

    private void Awake()
    {
        attachedAgent = GetComponent<GAgent>();
        foreach (MemData memData in TagsToSense)
        {
            senseTagsToStateDict.Add(memData.tag, memData.state);
        }
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
                if (obj && senseTagsToStateDict.ContainsKey(obj.tag) 
                    && (!Physics.Raycast(transform.position, obj.transform.position, out RaycastHit rayHit, sensorRadius, obstacleMask) || (rayHit.rigidbody && rayHit.rigidbody.gameObject == obj)))
                {
                    sensedObjects.Add(obj);
                }
                else if (obj && obj.CompareTag("Player") && ObjMemory.ContainsKey("Player"))
                {
                    sensedObjects.Add(obj);
                }
            }
            yield return null;

            foreach (GameObject obj in sensedObjects)
            {
                CheckObject(obj);
            }
            foreach (KeyValuePair<string, GameObject> kvp in ObjMemory.ToArray())
            {
                if (!kvp.Value)
                {
                    RemoveTagFromMemory(kvp.Key);
                }
                else if (kvp.Key == "Player")
                {

                    if (!sensedObjects.Contains(kvp.Value))
                    {
                        RemoveObjFromMemory(kvp.Value);
                        attachedAgent.agentBeliefs.SetState(EState.PlayerIsClose, 0);
                    }
                    else if (Vector3.Distance(kvp.Value.transform.position, transform.position) < closeRadius)
                    {
                        attachedAgent.agentBeliefs.SetState(EState.PlayerIsClose, 1);
                    }
                    else
                    {
                        attachedAgent.agentBeliefs.SetState(EState.PlayerIsClose, 0);
                    }
                }
                //else if (!sensedObjects.Contains(kvp.Value))
                //{
                //    RemoveObjFromMemory(kvp.Value);
                //}
            }

            yield return new WaitForSeconds(timeBetweenChecks);
        }
    }

    public bool CheckObject(GameObject obj, string trueIfTag = null)
    {
        if (obj && senseTagsToStateDict.TryGetValue(obj.tag, out EState state))
        {
            if (ObjMemory.ContainsKey(obj.tag) && ObjMemory[obj.tag] && ObjMemory[obj.tag] != obj)
            {
                if (Vector3.SqrMagnitude(transform.position - obj.transform.position) < Vector3.SqrMagnitude(transform.position - ObjMemory[obj.tag].transform.position))
                {
                    ObjMemory[obj.tag] = obj;
                }
            }
            else
            {
                ObjMemory[obj.tag] = obj;
                attachedAgent.agentBeliefs.SetState(state, 1);
            }
        }
        return trueIfTag == null || obj.CompareTag(trueIfTag);
    }

    public void RemoveObjFromMemory(GameObject obj)
    {
        if (ObjMemory.ContainsKey(obj.tag) && ObjMemory[obj.tag] == obj)
        {
            ObjMemory.Remove(obj.tag);
            attachedAgent.agentBeliefs.SetState(senseTagsToStateDict[obj.tag], 0);
        }
    }

    public void RemoveTagFromMemory(string tag)
    {
        ObjMemory.Remove(tag);
        attachedAgent.agentBeliefs.SetState(senseTagsToStateDict[tag], 0);
    }

    public bool TryGetObjectOfTag(string tag, out GameObject value)
    {
        if (ObjMemory.TryGetValue(tag, out value))
        {
            if (value) return true;

            ObjMemory.Remove(tag);
        }
        return false;
    }

    public void PickUp(SmartObject pickup)
    {
        if (pickup)
        {
            DropPickup();
            attachedAgent.agentBeliefs.SetState(pickup.stateToAffect, 1);
            pickedUpTag = pickup.tag;
            pickedUpObject = pickup;
            pickedUpObject.transform.position = pickupTransform.position;
            pickedUpObject.transform.rotation = pickupTransform.rotation;
            pickedUpObject.transform.parent = pickupTransform;
        }
    }

    public void DropPickup()
    {
        if (pickedUpObject)
        {
            attachedAgent.agentBeliefs.SetState(pickedUpObject.stateToAffect, 0);
            pickedUpObject.Drop();
            pickedUpObject = null;
            pickedUpTag = null;
        }
    }

    public void UsePickup(Vector3 at)
    {
        if (pickedUpObject)
        {
            transform.LookAt(at);
            pickedUpObject.UsePickedup(at);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, sensorRadius);
        //Vector3 p1 = Quaternion.AngleAxis(-visionConeHalfRadius, Vector3.up) * transform.forward;
        //Vector3 p2 = Quaternion.AngleAxis(visionConeHalfRadius, Vector3.up) * transform.forward;
        //Gizmos.DrawLine(transform.position, transform.position + p1 * sensorRadius);
        //Gizmos.DrawLine(transform.position, transform.position + p2 * sensorRadius);
    }
}
