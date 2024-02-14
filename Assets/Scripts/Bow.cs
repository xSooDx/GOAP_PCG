using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : SmartObject
{
    [SerializeField] Bullet ArrowPrefab;
    [SerializeField] Transform ArrowSpawnTransform;
    [SerializeField] float timeBetweenShots = 0.75f;

    float timer;

    public override bool Drop()
    {
        return DefaultDrop();
    }

    public override bool Interact(out SmartObject pickup, GameObject interactor)
    {
        timer = 0;
        return DefaultPickupInteract(out pickup, interactor);
    }

    public override bool UsePickedup(Vector3 atPoint)
    {
        if (timer <= 0)
        {
            Vector3 dir = atPoint - ArrowSpawnTransform.position;
            dir.Normalize();
            Instantiate(ArrowPrefab, ArrowSpawnTransform.position, ArrowSpawnTransform.rotation);
            timer = timeBetweenShots;
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        
    }
}
