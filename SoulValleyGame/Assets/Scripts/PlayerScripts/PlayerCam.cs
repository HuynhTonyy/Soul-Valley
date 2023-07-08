using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    private PhotonView view;

    public Transform orientation;
    public Transform playerCam;

    float xRotation;
    float yRotation;
    void Start()
    {
        view = GetComponent<PhotonView>();

        if(!view.IsMine){
            Destroy(playerCam.gameObject);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        if(view.IsMine){
            

            bool invState = InventoryUIControler.status;

            if(!invState)
            {
                // Get mouse input
                float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
                float mousey = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensX;

                yRotation += mouseX;
                xRotation -= mousey;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.Rotate(Vector3.up * mouseX);
            
                playerCam.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            }

        }
       
    }
}
