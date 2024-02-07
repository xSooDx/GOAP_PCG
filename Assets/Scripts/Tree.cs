using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] GameObject woodPrefab;

    public GameObject Interact()
    {
        Destroy(gameObject);
        return Instantiate(woodPrefab, transform.position, transform.rotation);
    }

    private void OnDestroy()
    {
        
    }
}
