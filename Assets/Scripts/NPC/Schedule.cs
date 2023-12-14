using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedule : MonoBehaviour
{
    public float DayLentghInSeconds;
    public List<PersonaCycle> NPCs;

    [SerializeField]
    Skybox cameraSkybox;

    [SerializeField]
    Material[] skyboxMaterials;

    void Start()
    {
        StartCoroutine(DayCycle());
    }

    IEnumerator DayCycle()
    {
        //day
        foreach (PersonaCycle p in NPCs)
        {
            p.SetActionAndPlace(0);
        }
        cameraSkybox.material = skyboxMaterials[0];

        yield return new WaitForSeconds(DayLentghInSeconds / 4);

        //evening
        foreach (PersonaCycle p in NPCs)
        {
            p.SetActionAndPlace(1);
        }
        cameraSkybox.material = skyboxMaterials[1];
        yield return new WaitForSeconds(DayLentghInSeconds / 4);

        //night
        foreach (PersonaCycle p in NPCs)
        {
            p.SetActionAndPlace(2);
        }
        cameraSkybox.material = skyboxMaterials[2];


        yield return new WaitForSeconds(DayLentghInSeconds / 4);
    }
    [System.Serializable]
    public class PersonaCycle
    {
        public NPC NPC_behaviour;
        public GameObject NPC_object;

        [Header("locations")]
        public Transform Work;
        public Transform Home;
        public Transform Leisure;
        public Transform Special;


        public void SetActionAndPlace(int action)
        {
            NPC_behaviour.SetAction(action);
            Debug.Log("Daytime switch to " + action.ToString());

            switch (action)
            {
                case 0:
                    NPC_object.transform.position = Work.position;
                    break;
                case 1:
                    NPC_object.transform.position = Home.position;
                    break;
                case 2:
                    NPC_object.transform.position = Leisure.position;
                    break;

                case 3:
                    NPC_object.transform.position = Special.position;
                    break;
                default:
                    print("no location assigned for index " + action);
                    break;

            }
        }
    }


}

