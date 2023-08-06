using UnityEngine;

public class Player : MonoBehaviour
{
    int delay = 320;
    public GameObject network; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        delay--;
        if (delay <= 0)
        {
            network.SendMessage("SendObject", gameObject);
            delay = 320; 
        }
    }
}
