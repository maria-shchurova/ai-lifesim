using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    bool walking;
    [SerializeField] float stoppingDistance;
    [SerializeField] float speed;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.collider.CompareTag("Floor"))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;
                    else
                        SetDestination(hit.point);
                }
            }
        }
        if(agent.remainingDistance <= stoppingDistance)
        {
            walking = false;
        }
        else
        {
            walking = true;
        }

        animator.SetBool("Walk", walking);
    }

    public void SetDestination(Vector3 target)
    {
        agent.destination = target;
    }
    public void ResetDestination()
    {
        agent.ResetPath();
        agent.isStopped = true;
    }
    public void ReleaseAgent()
    {
        agent.isStopped = false;
    }
}
