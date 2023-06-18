using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public GameObject toolBar, inventory,player;
    public static bool status = false;
    /*public GameObject camera;*/

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            toolBar.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            onAndOff(inventory);
            onAndOff(player);
            
            //onAndOff(camera);
            cursorStatus();
        }
    }
    void onAndOff(GameObject objectName)
    {
        objectName.SetActive(!objectName.activeSelf);
    }

    public static bool getState()
    {
        return status;
    }
    void cursorStatus()
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            status = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            status = false;
        }
    }
}
