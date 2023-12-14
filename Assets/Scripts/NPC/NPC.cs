using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private GameObject playerCharacter;
    private GameObject targetPoint;
    private NPC_Persona AI_character;
    private MouseHandler mouseHandler;
    private Animator animatorController;

    public float GlanceDistance;
    public float TalkDistance;

    public bool isWillingToTalk = true;
    public bool isTalking = false;

    void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        targetPoint = GameObject.Find("CameraRoot");
        AI_character = GetComponent<NPC_Persona>();
        mouseHandler = GetComponent<MouseHandler>();
        animatorController = GetComponent<Animator>();

        Messenger.AddListener("DialogueStarted", () => {
            isTalking = true;
        });       
        Messenger.AddListener("DialogueFinished", () => {
            isTalking = false;
        });
    }



    public void SetAction(float actionNumber)
    {
        //maybe more complex but for now just animation
        animatorController.SetFloat("CurrentAction", actionNumber);
    }

    void LateUpdate()
    {
        if (!playerCharacter || !targetPoint)
            return;


        if (Vector3.Distance(transform.position, playerCharacter.transform.position) < GlanceDistance && !isTalking)
        {
            GetComponent<HeadLookAt>().StartLookingAt(targetPoint.transform);
        }

        if (Vector3.Distance(transform.position, playerCharacter.transform.position) < TalkDistance)
        {           
            
            if (isWillingToTalk)
            {
                AI_character.StartDialog();
                isWillingToTalk = false; //after first encounter? TODo think about this or don'r let the dialog finish abruptly
            }
            
        }
    }
}
