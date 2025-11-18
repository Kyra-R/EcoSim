using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalSettingButton : MonoBehaviour
{
    public Button m_MyButton;
    public GameObject visualizer;
    GameObject menuScreen;
    // Start is called before the first frame update
    void Awake(){
        visualizer = GameObject.Find("AnimalSettings");
    }
    void Start()
    {
       //visualizer = GameObject.Find("EnvSettings");
        visualizer.SetActive(false);
        m_MyButton.onClick.AddListener(OnButtonClick);
        menuScreen = this.transform.parent.gameObject;
    }

    // Update is called once per frame
 void OnButtonClick()
    {
        visualizer.SetActive(true);
        menuScreen.SetActive(false);
    }
}
