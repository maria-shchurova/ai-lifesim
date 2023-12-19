using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using static GameStateManager;

public class ClickToMove : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    bool walking;
    bool talking;

    Transform lastTalkingTarget = null;

    [SerializeField] float stoppingDistance;
    [SerializeField] float speed;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = speed;

        Messenger.AddListener("DialogueFinished", () => {
            talking = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        animator.SetBool("Walk", walking);
        walking = !(agent.remainingDistance <= stoppingDistance);

        if (talking)
        {
            if (agent.remainingDistance <= stoppingDistance)
            {
                TakeTalkRotation();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.collider.CompareTag("Floor"))
                    {
                        if (EventSystem.current.IsPointerOverGameObject())
                            return;
                        else
                            SetDestination(hit.point);
                    }
                }
            }
        }

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

    public void TakeTalkPosition(Transform talker)
    {
        Vector3 targetPosition = talker.position;
        Quaternion targetRotation = talker.rotation;

        // Calculate the forward vector based on the target object's rotation
        Vector3 forwardVector = targetRotation * Vector3.forward;

        // Calculate the new position in front of the target object
        Vector3 newPosition = targetPosition + forwardVector * 1.3f;

        Debug.DrawRay(talker.position, newPosition, Color.red);
        SetDestination(newPosition);

        lastTalkingTarget = talker;
        talking = true;
    }

    void TakeTalkRotation()
    {
        transform.LookAt(new Vector3(lastTalkingTarget.position.x, transform.position.y, lastTalkingTarget.position.z));
    }
}
