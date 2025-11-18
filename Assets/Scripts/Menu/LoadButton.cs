using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour
{
    public Button m_MyButton;

    //public Image diagramImage;
    SaveController saveController;
    GameObject menuScreen;

    
    void Start()
    {
        saveController = GameObject.Find("SaveController").GetComponent<SaveController>();
  
        menuScreen = this.transform.parent.gameObject;
        m_MyButton.onClick.AddListener(OnButtonClick);
       // canvas.GetComponent<Canvas>().enabled = !canvas.GetComponent<Canvas>().enabled; //TODO: visible instad of active, or lines break
    }
    // Start is called before the first frame update
   void OnButtonClick()
    {
        //m_MyButton.onClick.RemoveListener(OnButtonClick);
        Debug.Log("----");

        saveController.LoadSave();
   }
}
