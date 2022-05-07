using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * The functions in this class serve to modify the current mitos already instantiated during run time.
 */
public class MenuControls : MonoBehaviour
{
    public GameObject menu;
    public GameObject menuButton;
    public GameObject cytoskeletonMenu;
    public GameObject mitoMenu;
    public UnityEngine.UI.Button mitoMenuButton;
    public UnityEngine.UI.Button cytoskeletonMenuButton;

    private Vector3 menuButtonOriginalPos;

    void Start()
    {
        menuButtonOriginalPos = menuButton.transform.position;
    }

    public void toggleMenu()
    {
        bool isActive = menu.activeSelf;

        if (isActive)
        {
            menuButton.transform.position = new Vector3(Screen.width, menuButton.transform.position.y, transform.position.z);
        }
        else
        {
            menuButton.transform.position = menuButtonOriginalPos;
        }
        menu.SetActive(!isActive);
    }

    public void showCytoskeletonMenu()
    {
        cytoskeletonMenu.SetActive(true);
        mitoMenu.SetActive(false);
        disableButton(cytoskeletonMenuButton);
        enableButton(mitoMenuButton);
    }

    public void showMitoMenu()
    {
        cytoskeletonMenu.SetActive(false);
        mitoMenu.SetActive(true);
        disableButton(mitoMenuButton);
        enableButton(cytoskeletonMenuButton);
    }

    private void disableButton(UnityEngine.UI.Button button)
    {
        button.enabled = false;
        button.GetComponent<UnityEngine.UI.Image>().color = Color.grey;
    }

    private void enableButton(UnityEngine.UI.Button button)
    {
        button.enabled = true;
        button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }
}
