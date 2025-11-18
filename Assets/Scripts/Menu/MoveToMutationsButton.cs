using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MoveToPlantMutationsButton : MonoBehaviour
{
    public Button m_MyButton;

    public GameObject sliders;
    
    void Start()
    {
        m_MyButton.onClick.AddListener(OnButtonClick);
       // canvas.GetComponent<Canvas>().enabled = !canvas.GetComponent<Canvas>().enabled; //TODO: visible instad of active, or lines break
    }
    // Start is called before the first frame update
   void OnButtonClick()
    {
        Debug.Log("Lol2");
        if(sliders.GetComponent<SetPlantButton>() != null){
            sliders.GetComponent<SetPlantButton>().SwitchPage();
        } else
        if(sliders.GetComponent<SetAnimalButton>() != null){
            sliders.GetComponent<SetAnimalButton>().SwitchPage();
        }

   }

}