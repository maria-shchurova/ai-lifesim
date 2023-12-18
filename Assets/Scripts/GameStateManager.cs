using UnityEngine;
using System.Collections.Generic;
using System;

public class GameStateManager : MonoBehaviour
{
    List<NPC_Persona> NPC_OnScene;

    [SerializeField]
    GameObject Dialog;

    [SerializeField]
    GameObject pauseMenuObject;

    private GameState currentGameState = GameState.GAME_STATE_PLAY;

    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }

    public enum GameState
    {
        GAME_STATE_PLAY = 0,
        GAME_STATE_DIALOGUE = 1,
        GAME_STATE_PAUSE = 2
    }


    void Start()
    {
        NPC_OnScene = new List<NPC_Persona>();
        foreach (var npc in FindObjectsOfType<NPC_Persona>())
        {
            NPC_OnScene.Add(npc);
        }

        Messenger.AddListener("DialogueFinished", () => {
            SwitchToState(GameState.GAME_STATE_PLAY);
            foreach (NPC_Persona npc in NPC_OnScene)
            {
                npc.enabled = true;
            }
        });

        Messenger.AddListener("ResumeGame", () => {
            SwitchToState(GameState.GAME_STATE_PLAY);
        });

        Messenger.AddListener("DialogueMode", () => {
            SwitchToState(GameState.GAME_STATE_DIALOGUE);
        });

        Messenger.AddListener("Pause", () => {
            SwitchToState(GameState.GAME_STATE_PAUSE);
        });

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GAME_STATE_PAUSE)
                SwitchToState(GameState.GAME_STATE_PLAY);
            else
                SwitchToState(GameState.GAME_STATE_PAUSE);
        }
    }


    void SwitchToState(GameState stateNumber)
    {

        if (stateNumber == GameState.GAME_STATE_PLAY)
        {
            Time.timeScale = 1;
            pauseMenuObject.SetActive(false);
            Dialog.SetActive(false);
            //characterController.enabled = true;

            Debug.Log("Switched to PLAY");
        }
        else if (stateNumber == GameState.GAME_STATE_DIALOGUE)
        {
            Time.timeScale = 1;
            pauseMenuObject.SetActive(false);
            //characterController.StopWalking();
            //characterController.enabled = false;
            Dialog.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Debug.Log("Switched to DIALOG");
        }
        else if (stateNumber == GameState.GAME_STATE_PAUSE)
        {
            Time.timeScale = 0;
            pauseMenuObject.SetActive(true);
            //characterController.StopWalking();
            //characterController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Switched to PAUSE");
        }
    }


}
