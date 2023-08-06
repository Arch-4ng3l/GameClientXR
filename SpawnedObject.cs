using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject network;
    Vector3 lastPosition = new Vector3();
    bool isMoving = false;
    void Start()
    {
        network = GameObject.FindGameObjectsWithTag("NetworkManager")[0];
        lastPosition = transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
         if(lastPosition != transform.position && !isMoving)
        {
            isMoving = true;
        } 

        if(isMoving && transform.position == lastPosition)
        {
            isMoving = false;
           network.SendMessage("SendObject", gameObject);
        }

        lastPosition = transform.position;
       
    }



}
