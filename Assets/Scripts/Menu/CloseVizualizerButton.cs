using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CloseVisualizerButton : MonoBehaviour
{
    public Button m_MyButton;

    //public Image diagramImage;

    GameObject visualizerWindow;

    public GameObject menuScreen;
    
    void Start()
    {
        visualizerWindow = transform.parent.gameObject;
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

        visualizerWindow.SetActive(false);
        menuScreen.SetActive(true);
        //diagram.SetActive(true);
        //diagramImage.enabled = true;
        //diagram.GetComponent<DD_DataDiagram>().enabled = true;

        //menuScreen.SetActive(false);
        
   }

}
