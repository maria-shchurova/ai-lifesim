using UnityEngine;

public class StreetLights : MonoBehaviour
{
    Light thisLight;
    public Material OffMaterial;
    public Material OnMaterial;

    MeshRenderer thisMesh;
    void Start()
    {
        thisLight = GetComponent<Light>();
        thisMesh = GetComponentInParent<MeshRenderer>();

        Messenger.AddListener("Sunrise", Sunrise);
        Messenger.AddListener("Sunset", Sunset);
    }

    void Sunrise()
    {
        thisLight.enabled = false;
        thisMesh.material = OffMaterial;
    }

    void Sunset()
    {
        thisLight.enabled = true;
        thisMesh.material = OnMaterial;
    }
}
