using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CarnivoreAttributesButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Button m_MyButton;

    public GameObject animalAttributesButton;

    public GameObject herbivoreAttributesButton;

    void Start()
    {
        m_MyButton.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void OnButtonClick()
    {
        animalAttributesButton.GetComponent<SetAnimalButton>().ChangeAndUpdateIsHerbivore(false);
        this.GetComponent<Image>().color = Color.red;
        herbivoreAttributesButton.GetComponent<Image>().color = Color.white;
    }
}
