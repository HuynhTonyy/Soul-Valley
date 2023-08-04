using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    float minPositionY;
    float maxPositionY;
    bool isMax = false;
    bool isSet = false;

    // Update is called once per frame
    void Update()
    {
        if(!isSet){
            minPositionY = transform.position.y;
            maxPositionY = transform.position.y + 0.1f;
            isSet = true;
        }
        if (!isMax)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, maxPositionY, transform.position.z), 0.001f);
            if (transform.position.y >= maxPositionY)
            {
                isMax = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, minPositionY, transform.position.z), 0.001f);
            if (transform.position.y <= minPositionY)
            {
                isMax = false;
            }
        }
    }
}
