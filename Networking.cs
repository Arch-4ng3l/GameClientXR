using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

public class Networking : MonoBehaviour
{
    public float renderDistance;
    public string url;
    private JsonSettings settings;
    private Client client;
    async void Start()
    {
        Uri uri = new("ws://192.168.178.20:3000/api");
        Uri uri2 = new("http://192.168.178.20:3000/api/assets");
        client = new Client(uri, uri2);
        //settings.renderdistance = renderDistance;
        
        int err = await client.Connect();
        
        if (err != 0)
        {
            return;
        }

        await client.GetAssets("./Assets/assets.zip", "./Assets");
        //Task task3 = client.SendJson<JsonSettings>(settings);
        //task3.RunSynchronously();
        

    }

    // Update is called once per frame
    //void Update()
    

    public async void SendObject(GameObject send)
    {

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

        foreach (JsonObject json in objs)
        {
            json.ToGameObject();
            await Task.Delay(50);
        }
        
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
    public void ToGameObject()
    {

        GameObject obj = GameObject.Find(name);
        Vector3 position = new Vector3(x, y, z);

        if (obj == null)
        {
            string pattern = @"\d+$";
            string objName = Regex.Replace(name, pattern, "");
            obj = GameObject.Find(objName);
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

class JsonSettings
{
    public float renderdistance;

}
