using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float moveSpeed = 1f;
    public float PickUpRadius = 1f;
    public ItemScript itemData;

    private SphereCollider myCollider;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        
        if (transform.position.y <= 10)
        {
            float upYPosition = transform.position.y + moveSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, upYPosition, transform.position.z);
            Debug.Log("Len"+transform.position.y);
        }
        if (transform.position.y > 13 && transform.position.y > 10)
        {
            float downYPosition = transform.position.y - moveSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, downYPosition, transform.position.z);
            Debug.Log("Xuong" + transform.position.y);
        }
       
       
        
   
    }
    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }
    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;
        if (inventory.AddToInventory(itemData,1))
        {
            Destroy(this.gameObject);
        }
    }
}
