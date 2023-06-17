using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public GameObject toolBar, inventory,player,camera;
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
    void cursorStatus()
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
