using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInventoryInShop : MonoBehaviour
{
    public static bool isClosed = true;
    [FormerlySerializedAs("chestPanel")]
    public DynamicInventoryDisplay playerBackpackPanel;

    internal static bool status = false;
    private void Start()
    {
        playerBackpackPanel.gameObject.SetActive(false);

    }
    private void OnEnable()
    {
        PlayerInventoryHolder.OnDynamicPlayerInventoryDisplayRequested += DisplayPlayerInventory;
    }
    private void OnDisable()
    {
        PlayerInventoryHolder.OnDynamicPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClosed)
        {
            close();
        }

    }

    public void close()
    {      

        if (playerBackpackPanel.gameObject.activeInHierarchy)
        {
            playerBackpackPanel.gameObject.SetActive(false);
            status = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    void DisplayPlayerInventory(InventorySystem invDisplay, int offset)
    {
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invDisplay, offset);
        RectTransform rectTransform = playerBackpackPanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        Cursor.visible = true;
        status = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
