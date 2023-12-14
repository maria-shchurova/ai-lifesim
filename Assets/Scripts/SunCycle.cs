using UnityEngine;

public class SunCycle : MonoBehaviour
{
    Light sunLight;
    public float intensityChangingSpeed;

    Vector3 rot = Vector3.zero;
    public float cycleLength = 20;
    bool lampsOn = false;

    private void Start()
    {
        sunLight = GetComponent<Light>();
    }

    void Update()
    {
        //Debug.Log(WrapAngle(transform.localEulerAngles.x));

        rot.x = cycleLength * Time.deltaTime;
        transform.Rotate(rot, Space.World);

        if(WrapAngle(transform.localEulerAngles.x) < -3)
        {
           // sunLight.intensity -= intensityChangingSpeed;

            if (!lampsOn)
            {
                Messenger.Broadcast("Sunset");
                Debug.Log("Sunset");
                lampsOn = true;
            }
            else
            {
                return;
            }
        }
        else
        {
            //sunLight.intensity += intensityChangingSpeed * cycleLength * Time.deltaTime;

            if (lampsOn)
            {
                Messenger.Broadcast("Sunrise");
                Debug.Log("Sunrise");
                lampsOn = false;
            }
            else
            {
                return;
            }
        }
    }

    public static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
}
