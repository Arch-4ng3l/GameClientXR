using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class Networking : MonoBehaviour
{
    public float renderDistance;
    public string ip;
    public string port;
    private JsonSettings settings = new();
    private Client client;
    public GameObject button;
    public List<GameObject> spawnableObjects;
    async void Start()
    {
        string url1 = "ws://"+ ip + ":" + port + "/api";
        string url2 = "http://"+ ip + ":" + port + "/api/assets";
        Uri uri = new(url1);
        Uri uri2 = new(url2);
        client = new Client(uri, uri2);
        
        int err = await client.Connect();
        
        if (err != 0)
        {
            return;
        }


        settings.nums = renderDistance;
        await client.SendJson<JsonSettings>(settings);
        var s = await client.ReceiveJson<JsonSettings>();   

        if (s == null) {
            return;;
        }

        uint  nums = (uint)s[0].nums;
        spawnableObjects = await client.GetAssets("./Assets/assets.zip", "./Assets");
        button.SendMessage("Done", nums);
    }

    public async void SendObject(GameObject send)
    {
        if (send == null || client == null) 
        {
            return;
        }
        JsonObject json = new JsonObject();
        json.FromGameObject(send);
        await client.SendJson<JsonObject>(json);
       
        if (json.name != "Player")
        {
            return;
        }
        await RecvObjects();

    }

    async Task RecvObjects()
    {
        JsonObject[] objs = await client.ReceiveJson<JsonObject>();
        if(objs == null) {
            return;
        }
        foreach (JsonObject json in objs)
        {
            ToGameObject(json);
            await Task.Delay(50);
        }
        
    }

    private void LoadAssets() 
    {

    }

    private void ToGameObject(JsonObject json)
    {

        GameObject obj = GameObject.Find(json.name);
        Vector3 position = new Vector3(json.x, json.y, json.z);

        if (obj == null)
        {
            string pattern = @"\d+$";
            string objName = Regex.Replace(name, pattern, "");
            foreach(GameObject gameObject in spawnableObjects) 
            {
                Debug.Log(gameObject.name);
                if(gameObject.name == objName) 
                {
                    obj = gameObject;
                    break;
                }
            }

            obj = UnityEngine.Object.Instantiate(obj, position, Quaternion.identity);
            obj.name = name;

            obj.AddComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>();
            obj.AddComponent<SpawnedObject>();

            return;
        }

        if (obj.transform.position != position)
        {
            obj.transform.position = position;
        }

        return;
    }
}

[System.Serializable]
public class JsonObject
{
    public float x;
    public float y;
    public float z;
    public string name;

    public JsonObject() : base()
    {
        this.name = "";
        this.x = 0;
        this.y = 0;
        this.z = 0;
    }
    public void FromGameObject(GameObject go)
    {
        this.name = go.name;
        this.x = go.transform.position.x;
        this.y = go.transform.position.y;
        this.z = go.transform.position.z;
    }
}

class JsonSettings
{
    public float nums;
}
