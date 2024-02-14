using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class SmartObject : MonoBehaviour
{
    public const string PICKEDUP_TAG = "__PickedUp";
    public bool isPickedUp;
    public EState stateToAffect;
    Rigidbody rb;
    public GameObject owner;

    string cachedTag;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cachedTag = tag;
    }

    public abstract bool UsePickedup(Vector3 atPoint);

    public abstract bool Interact(out SmartObject pickup, GameObject interactor);

    public abstract bool Drop();

    protected bool DefaultPickupInteract(out SmartObject pickup, GameObject interactor)
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
            pickup = Instantiate(this.gameObject).GetComponent<SmartObject>();
            pickup.tag = PICKEDUP_TAG;
            Destroy(this.gameObject);
            pickup.owner = interactor;
            return true;
        }
        pickup = null;
        return false;
    }

    protected bool DefaultDrop()
    {
        if (isPickedUp)
        {
            isPickedUp = false;
            transform.parent = null;
            transform.position = transform.position + transform.forward;
            tag = cachedTag;
            owner = null;
            return true;
        }
        return false;
    }
}
