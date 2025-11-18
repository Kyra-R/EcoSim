using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HerbivoreAttributesButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Button m_MyButton;

    public GameObject animalAttributesButton;

    public GameObject carnivoreAttributesButton;

    void Start()
    {
        m_MyButton.onClick.AddListener(OnButtonClick);
        this.GetComponent<Image>().color = Color.green;
    }

    // Update is called once per frame
    void OnButtonClick()
    {
        animalAttributesButton.GetComponent<SetAnimalButton>().ChangeAndUpdateIsHerbivore(true);
        this.GetComponent<Image>().color = Color.green;
        carnivoreAttributesButton.GetComponent<Image>().color = Color.white;
    }
}
