using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
   // bool toEnable = true;

    GameObject canvas;
    public GameObject menuScreen;
    public GameObject cameraControl;

    public Button enterMenu;
    public Button exitMenu;


    bool inMenu = true;
    void Awake(){

       
        /*if (currentWidth >= 1920)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        }
        else if (currentWidth >= 1280)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        else
        {
            Screen.SetResolution(1024, 768, FullScreenMode.Windowed);
        } */

    }
    
    void Start()
    {
        enterMenu.onClick.AddListener(OnButtonClick);
        enterMenu.gameObject.SetActive(false);
        exitMenu.onClick.AddListener(OnButtonClick);
        int currentWidth = (int) (Screen.width * 0.8f);
        int currentHeight = (int) (Screen.height * 0.8f);
        //Screen.SetResolution(1280, 720, false);
       /*  if (currentWidth >= 1920)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        else
        {
            Screen.SetResolution(1024, 768, FullScreenMode.Windowed);
        } */
        if(currentHeight < 768){
            Screen.SetResolution(1280, 768, FullScreenMode.Windowed);
        } else {
             Screen.SetResolution(currentWidth, currentHeight, FullScreenMode.Windowed);
        }
        //Screen.SetResolution( (int) (currentWidth * 0.8f), (int) (currentHeight * 0.8f), FullScreenMode.Windowed);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q") && !inMenu)
        {
            MenuVisibilityChange();
        }
    }

    void OnButtonClick()
    {
        MenuVisibilityChange();
   }

    void MenuVisibilityChange() {
        cameraControl.GetComponent<CameraController2D>().ChangeInMenu();
        enterMenu.gameObject.SetActive(!enterMenu.gameObject.activeSelf);
        menuScreen.SetActive(!menuScreen.activeSelf);
        inMenu = !inMenu;
    }
}
