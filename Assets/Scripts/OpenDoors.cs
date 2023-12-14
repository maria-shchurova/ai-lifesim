using UnityEngine.SceneManagement;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    public float openingDistance;
    private Transform playerCharacter;
    public bool automatic;

    private Animator animController;

    public string LocationName; //next scene name

    private void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        animController = GetComponent<Animator>();

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(automatic)
        {
            if (Vector3.Distance(transform.position, playerCharacter.transform.position) < openingDistance)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }
    }

    public void OpenDoor()
    {
        animController.SetTrigger("Open");
    }

    void CloseDoor()
    {
        animController.SetTrigger("Close");
    }

    public void LoadLocation()
    {

        SceneManager.LoadScene(LocationName);        
    }
}
