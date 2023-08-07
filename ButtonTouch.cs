
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonTouch : MonoBehaviour
{
    public GameObject player;
    uint count;
    GameObject obj = null;
    void Start()
    {
    }

    void Update()
    {
    }

    public void Done(uint nums) {
        count = nums;
    }

        
    void SpawnObject(string type)
    {
        switch (type) {
            case "Cube":
                obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case "Sphere":
                obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                break;
            case "Capsula":
                obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                break;
            case "Cylinder":
                obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                break;
            default:
                return;
        }

        Vector3 position = player.transform.position;

        obj.transform.position = position;

        obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        obj.AddComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>();
        obj.AddComponent<Microsoft.MixedReality.Toolkit.Input.NearInteractionGrabbable>();
        obj.AddComponent<SpawnedObject>();

        obj.name = obj.name + count.ToString();
        count++;
    }
    public void SpawnCube()
    {
        SpawnObject("Cube");
    }
    public void SpawnSphere()
    {
        SpawnObject("Sphere");
    }
    public void SpawnCylinder()
    {
        SpawnObject("Cylinder");
    }

    public void SpawnCapsula()
    {
        SpawnObject("Capsula");
    }

}
