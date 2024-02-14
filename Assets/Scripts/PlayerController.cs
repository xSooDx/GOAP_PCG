using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent), typeof(HealthComponent))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] LayerMask moveableMask;
    [SerializeField] LayerMask interactableMaks;

    [SerializeField] SmartObject pickedUpObject;
    [SerializeField] Transform pickupTransfrom;
    
    HealthComponent healthComponent;

    NavMeshAgent navAgent;
    Camera mainCamera;
    SmartObject objToInteractWith;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnDamage += OnDamage;
        healthComponent.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        gameObject.SetActive(false);
    }

    private void OnDamage(float currentHealth, float damage)
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out RaycastHit hit, 100, moveableMask))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 3, moveableMask))
                {
                    navAgent.destination = navHit.position;
                    if(Physics.Raycast(r, out RaycastHit objHit, 100, interactableMaks))
                    {
                        if (objHit.rigidbody && objHit.rigidbody.GetComponent<SmartObject>())
                        {
                            objToInteractWith = objHit.rigidbody.GetComponent<SmartObject>();
                        }
                    }
                    else
                    {
                        objToInteractWith = null;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(r, out RaycastHit hit, 100, interactableMaks))
            {
                Physics.Raycast(r, out RaycastHit lookhit, 100, moveableMask);
                transform.LookAt(lookhit.point);
                if(pickedUpObject) pickedUpObject.UsePickedup(hit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }


        if(objToInteractWith)
        {
            if(navAgent.ReachedNavDestination(1.5f, false))
            {
                objToInteractWith.Interact(out SmartObject pickup, gameObject);
                if (pickup)
                {
                    DropItem();
                    pickedUpObject = pickup;

                    pickedUpObject.transform.position = pickupTransfrom.position;
                    pickedUpObject.transform.rotation = pickupTransfrom.rotation;
                    pickedUpObject.transform.parent = pickupTransfrom;
                    objToInteractWith = null;
                }
            }
        }
    }

    private void DropItem()
    {
        if (pickedUpObject)
        {
            pickedUpObject.Drop();
            pickedUpObject = null;
        }
    }
}
