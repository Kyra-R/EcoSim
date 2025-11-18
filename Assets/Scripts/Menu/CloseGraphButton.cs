using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CloseGraphButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Button m_MyButton;

    GameObject diagram;

    public CanvasGroup uiGroup;

    public GameObject menuScreen;
    
    void Start()
    {
        //diagram = GameObject.Find("DataDiagram");

        m_MyButton.onClick.AddListener(OnButtonClick);
       // canvas.GetComponent<Canvas>().enabled = !canvas.GetComponent<Canvas>().enabled; //TODO: visible instad of active, or lines break
    }
    // Start is called before the first frame update
   void OnButtonClick()
    {
        //m_MyButton.onClick.RemoveListener(OnButtonClick);
        Debug.Log("Lol2");
        //diagram.SetActive(false);
        //diagramImage.enabled = true;
        //diagram.GetComponent<DD_DataDiagram>().enabled = true;

        uiGroup.alpha = 0f;       // Сделать полностью прозрачным
        uiGroup.interactable = false; // Отключить взаимодействие
        uiGroup.blocksRaycasts = false; 
        menuScreen.SetActive(true);
        
   }

}

