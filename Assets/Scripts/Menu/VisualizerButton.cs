using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class VisualizerButton : MonoBehaviour
{
    public Button m_MyButton;

    //public Image diagramImage;
    GameObject diagram;
    GameObject menuScreen;
    public GameObject visualizerWindow;

    NeuralNetworkVisualizer visualizer;
    //AnimalController animalController;
    
    void Start()
    {
        visualizer = GameObject.Find("NetworkVisualizer").GetComponent<NeuralNetworkVisualizer>();
        visualizerWindow.SetActive(false);
       // visualizerWindow = GameObject.Find("NetworkContainer");
        //animalController = GameObject.Find("AnimalController").GetComponent<AnimalController>();
        menuScreen = this.transform.parent.gameObject;
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
        visualizerWindow.SetActive(true);
        menuScreen.SetActive(false);

        visualizer.Visualize();
        // visualizer.Visualize(animalController.lastBrain);
        //diagram.SetActive(true);
        //diagramImage.enabled = true;
        //diagram.GetComponent<DD_DataDiagram>().enabled = true;

        //menuScreen.SetActive(false);
        
   }

}
