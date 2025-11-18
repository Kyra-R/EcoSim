using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlantGraphsButton : MonoBehaviour
{

    public Button m_MyButton;

    GraphLinesController graphController;

    bool linesActive = true;
    // Start is called before the first frame update
    void Start()
    {
        graphController = GameObject.Find("MenuController").GetComponent<GraphLinesController>();
        m_MyButton.onClick.AddListener(OnButtonClick);
    }


    void OnButtonClick()
    {
        //m_MyButton.onClick.RemoveListener(OnButtonClick);
        if(linesActive){
            graphController.TurnOffPlantLines();
        } else {
            graphController.TurnOnPlantLines();
        }
        
        linesActive = !linesActive;
   }


}
