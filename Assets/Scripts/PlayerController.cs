using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] LayerMask moveableMask;
    [SerializeField] LayerMask interactableMask;

    NavMeshAgent agent;
    Camera mainCamera;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(r, out RaycastHit hit, 100, moveableMask))
            {
                if(NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1, moveableMask))
                {
                    agent.destination = hit.point;
                }
            }

        }
    }
}
