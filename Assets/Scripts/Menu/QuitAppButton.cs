using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitAppButton : MonoBehaviour
{
    public Button m_MyButton;
    
    void Start()
    {
        m_MyButton.onClick.AddListener(OnButtonClick);
       // canvas.GetComponent<Canvas>().enabled = !canvas.GetComponent<Canvas>().enabled; //TODO: visible instad of active, or lines break
    }
    // Start is called before the first frame update
   void OnButtonClick()
    {
        //m_MyButton.onClick.RemoveListener(OnButtonClick);
        Debug.Log("exit");

        Application.Quit();
   }
}
