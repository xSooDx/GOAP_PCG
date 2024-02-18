using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : SmartObject
{
    bool isAttacking;
    [SerializeField] float damage = 5f;

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
                    Collider[] cs = Physics.OverlapSphere(owner.transform.position + owner.transform.forward, 1f);
                    foreach (Collider c in cs)
                    {
                        OnHt(c);
                    }
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

    private void OnHt(Collider other)
    {
        if (isPickedUp && isAttacking
            && other.attachedRigidbody && other.attachedRigidbody.gameObject != owner
            && other.attachedRigidbody.TryGetComponent<HealthComponent>(out HealthComponent hc))
        {
            if (other.attachedRigidbody.gameObject.CompareTag("Tree"))
            {
                hc.TakeDamage(damage * 5);
            }
            else
            {
                hc.TakeDamage(damage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if(owner)
        Gizmos.DrawSphere(owner.transform.position + owner.transform.forward, 1);
    }
}
