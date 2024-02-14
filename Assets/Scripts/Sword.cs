using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : SmartObject
{
    bool isAttacking;
    [SerializeField] float damage = 10f;

    public override bool Drop()
    {
        return DefaultDrop();
    }

    public override bool Interact(out SmartObject pickup, GameObject interactor)
    {
        return DefaultPickupInteract(out pickup, interactor);
    }

    public override bool UsePickedup(Vector3 atPoint)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Vector3 startRotation = transform.rotation.eulerAngles;
            gameObject.LeanRotateAroundLocal(Vector3.right, 120f, .05f).setOnComplete(
                () =>
                {
                    gameObject.LeanRotateAroundLocal(Vector3.right, -120f, .2f).setOnComplete(
                        () =>
                        {
                            isAttacking = false;
                        });
                });

            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isPickedUp && isAttacking 
            && other.attachedRigidbody && other.attachedRigidbody.gameObject != owner
            && other.attachedRigidbody.TryGetComponent<HealthComponent>(out HealthComponent hc))
        {
            hc.TakeDamage(damage);
        }
    }
}
