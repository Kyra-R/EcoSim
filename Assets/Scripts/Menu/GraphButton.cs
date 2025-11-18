using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GraphButton : MonoBehaviour
{
    public Button m_MyButton;

    //public Image diagramImage;
    GameObject diagram;
    GameObject menuScreen;
    public CanvasGroup uiGroup;

    
    void Start()
    {
        //diagram = GameObject.Find("DataDiagram");
        //diagram.GetComponent<DD_DataDiagram>().enabled = false;
        //diagramImage.enabled = false;
       // diagram.SetActive(false);

        uiGroup.alpha = 0f;       // Сделать полностью прозрачным
        uiGroup.interactable = false; // Отключить взаимодействие
        uiGroup.blocksRaycasts = false; 

        menuScreen = this.transform.parent.gameObject;
        m_MyButton.onClick.AddListener(OnButtonClick);
       // canvas.GetComponent<Canvas>().enabled = !canvas.GetComponent<Canvas>().enabled; //TODO: visible instad of active, or lines break
    }
    // Start is called before the first frame update
   void OnButtonClick()
    {
        //m_MyButton.onClick.RemoveListener(OnButtonClick);
        Debug.Log("Lol");

        uiGroup.alpha = 1f;       // Сделать видимым
        uiGroup.interactable = true;
        uiGroup.blocksRaycasts = true;

        //diagram.SetActive(true);
        //diagramImage.enabled = true;
        //diagram.GetComponent<DD_DataDiagram>().enabled = true;
        menuScreen.SetActive(false);
        
   }

}


