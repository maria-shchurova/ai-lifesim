using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    private bool isMouseOver = false;

    public GameObject talkOption;
    NPC_Persona dialogScript;
    NPC npcScript;
    private Transform playerCharacter;

    private void Start()
    {
        dialogScript = GetComponent<NPC_Persona>();
        npcScript = GetComponent<NPC>();
        playerCharacter = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, playerCharacter.transform.position) < npcScript.TalkDistance)
        {
            if(npcScript.isTalking)
                talkOption.SetActive(false);
            else
                talkOption.SetActive(isMouseOver);
        }

        if(talkOption.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            dialogScript.StartDialog();
        }
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

}