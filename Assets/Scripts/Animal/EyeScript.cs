using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject opened;
    public GameObject closed;

    bool open = true;

    public void OpenCloseEye()
    {
        open = !open;
        opened.SetActive(open);
        closed.SetActive(!open);
//        Debug.Log(open);
    }


    // Update is called once per frame

}
