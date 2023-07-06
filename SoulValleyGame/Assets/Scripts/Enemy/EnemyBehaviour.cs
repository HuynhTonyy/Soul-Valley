using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float detackRange = 1f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] LayerMask playerMask;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,detackRange, playerMask);
        if(colliders.Length >= 1){
            Vector3 relativePos = new Vector3(colliders[0].transform.position.x,0,colliders[0].transform.position.z) 
                - new Vector3(transform.position.x,0,transform.position.z);
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
            if(!Physics.Raycast(transform.position,transform.forward,attackRange,playerMask)){
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(colliders[0].transform.position.x, colliders[0].transform.position.y, colliders[0].transform.position.z), speed * Time.deltaTime);
            }else{
                Debug.Log("Hit player!");
            }
        }
    }
    private  void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position,transform.position + transform.forward * attackRange);
        Gizmos.DrawWireSphere(transform.position,detackRange);
    }
}
