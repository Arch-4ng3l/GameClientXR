
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonTouch : MonoBehaviour
{

    GameObject obj = null;
    List<GameObject> gameObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //textLabel.text = count.ToString();
    }

        
    void DeleteLastObject()
    {
        if (gameObjects.Count > 0)
        {
            Destroy(gameObjects.Last());
            gameObjects.Remove(gameObjects.Last());
        }
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


        GameObject[] temp = GameObject.FindGameObjectsWithTag("MainCamera");

        Vector3 position = temp[0].transform.position;

        obj.transform.position = position;

        obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        obj.AddComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>();
        obj.AddComponent<SpawnedObject>();
        name = obj.name + gameObjects.Count().ToString();

        obj.name = name;
        gameObjects.Add(obj);
    }
    void SpawnCube()
    {
        SpawnObject("Cube");
    }
    void SpawnSphere()
    {
        SpawnObject("Sphere");
    }
    void SpawnCylinder()
    {
        SpawnObject("Cylinder");
    }

    void SpawnCapsula()
    {
        SpawnObject("Capsula");
    }

}
