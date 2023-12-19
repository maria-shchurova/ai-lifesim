using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField]
    private GameObject DoorHint;
    [SerializeField]
    private GameObject TalkHint;
    [SerializeField]
    private ClickToMove movementController;

    private GameStateManager gameStates;
    private GameObject currentConversation;

    void Start()
    {
        gameStates = FindObjectOfType<GameStateManager>();
    }

    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
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
                TalkHint.SetActive(true);
                currentConversation = hit.collider.gameObject;

                if (Input.GetKeyDown(KeyCode.Mouse0) && hit.collider.gameObject.GetComponent<NPC_Persona>().isTalking == false)
                {
                    currentConversation = hit.collider.gameObject;
                    movementController.TakeTalkPosition(hit.collider.transform);
                    hit.collider.gameObject.GetComponent<NPC_Persona>().StartDialog();
                }
            }
            else
            {
                TalkHint.SetActive(false);
            }
        }
        else
        {
            //Debug.DrawRay(RaySource.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            DoorHint.SetActive(false);
            return;
        }

    }
    
}
