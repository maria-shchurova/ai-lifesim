using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private GameObject playerCharacter;
    private GameObject targetPoint;
    private NPC_Persona AI_character;
    private Animator animatorController;

    public float GlanceDistance;
    public float TalkDistance;

    public bool isWillingToTalk = true;

    void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        targetPoint = GameObject.Find("CameraRoot");
        AI_character = GetComponent<NPC_Persona>();
        animatorController = GetComponent<Animator>();
    }



    public void SetAction(float actionNumber)
    {
        //maybe more complex but for now just animation
        animatorController.SetFloat("CurrentAction", actionNumber);
    }

}
