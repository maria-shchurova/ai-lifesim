using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{

    public GameObject DoorHint;
    public GameObject TalkHint;
    public float RayLength;
    public Transform RaySource;
    GameStateManager gameStates;

    public GameObject currentConversation;
    public bool hittingNPC;
    void Start()
    {
        gameStates = FindObjectOfType<GameStateManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(RaySource.position, transform.TransformDirection(Vector3.forward), out hit, RayLength, layerMask))
        {
           // Debug.DrawRay(RaySource.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            if (hit.collider.gameObject.GetComponent<OpenDoors>())
            {
                DoorHint.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    DoorHint.SetActive(false);
                    hit.collider.gameObject.GetComponent<OpenDoors>().OpenDoor();
                }
            }
            else
            {
                DoorHint.SetActive(false);
            }


            if (hit.collider.gameObject.CompareTag("NPC") && gameStates.GetCurrentGameState() == GameStateManager.GameState.GAME_STATE_PLAY)
            {
                hittingNPC = true;

                TalkHint.SetActive(true);
                currentConversation = hit.collider.gameObject;
                
                if (Input.GetKeyDown(KeyCode.Mouse0)&& hit.collider.gameObject.GetComponent<NPC>().isTalking == false)
                {
                    currentConversation = hit.collider.gameObject;
                    hit.collider.gameObject.GetComponent<NPC_Persona>().StartDialog();
                }
            }         

        }
        else
        {
            //Debug.DrawRay(RaySource.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            TalkHint.SetActive(false);
            DoorHint.SetActive(false);
            return;
        }
    }
}
