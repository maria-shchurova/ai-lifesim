using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControls : MonoBehaviour
{
	public CameraControl manual;
	public CameraFlyAround animation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        manual = GetComponent<CameraControl>();
    }

   void OnGUI()
    {

        if (GUI.Button(new Rect(10, 10, 300, 50), "Free Camera control (esc to unlock cursor)"))
        {
            manual.enabled = true;
           // animation.enabled = false;
            Cursor.visible = true;
        }

        //if (GUI.Button(new Rect(10, 70, 200, 30), "Animation"))
        //    {
        //    	animation.enabled = true;
        //    	manual.enabled = false;
        //    }
    }


    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            manual.enabled = false;
            Cursor.visible = true;
        }
    }
}
