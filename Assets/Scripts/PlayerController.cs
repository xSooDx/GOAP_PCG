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
    //[SerializeField] GameObject rockPrefab;
    //[SerializeField] SmartObject rockTrower;

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
        UIManager.Instance.EndGame(false);
    }

    private void OnDamage(float currentHealth, float damage)
    {
        UIManager.Instance.healthText.text = "Health: " + Mathf.RoundToInt(currentHealth) + "/" + Mathf.RoundToInt(healthComponent.MaxHP);
    }

    // Start is called before the first frame update
    void Start()
    {
        OnDamage(healthComponent.CurrentHP, 0);
        DropItem();
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
                    if (Physics.Raycast(r, out RaycastHit objHit, 100, interactableMaks))
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
                if (pickedUpObject)
                {
                    pickedUpObject.UsePickedup(hit.point);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }


        if (objToInteractWith)
        {
            if (navAgent.ReachedNavDestination(1.5f, false))
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
                    //rockTrower.gameObject.SetActive(false);
                    UIManager.Instance.pickupText.text = "Pickup: " + pickedUpObject.cachedTag;
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
            //rockTrower.gameObject.SetActive(true);
        }
        if (!pickedUpObject)
        {
            UIManager.Instance.pickupText.text = "Pickup: None";
        }

    }
}
