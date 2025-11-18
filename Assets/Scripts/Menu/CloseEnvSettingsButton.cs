using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CloseEnvSettingsButton : MonoBehaviour
{
    public Button m_MyButton;

    //public Image diagramImage;

    GameObject envWindow;

    public GameObject menuScreen;
    
    void Start()
    {
        envWindow = transform.parent.gameObject;
        //diagram = GameObject.Find("DataDiagram");
        //diagram.GetComponent<DD_DataDiagram>().enabled = false;
        //diagramImage.enabled = false;
        //diagram.SetActive(false);

        //menuScreen = this.transform.parent.gameObject;

        m_MyButton.onClick.AddListener(OnButtonClick);
       // canvas.GetComponent<Canvas>().enabled = !canvas.GetComponent<Canvas>().enabled; //TODO: visible instad of active, or lines break
    }
    // Start is called before the first frame update
   void OnButtonClick()
    {
        //m_MyButton.onClick.RemoveListener(OnButtonClick);
        Debug.Log("Lol2");

        envWindow.SetActive(false);
        menuScreen.SetActive(true);
        //diagram.SetActive(true);
        //diagramImage.enabled = true;
        //diagram.GetComponent<DD_DataDiagram>().enabled = true;

        //menuScreen.SetActive(false);
        
   }

}
