using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(TimeUpdate());
    }
    IEnumerator TimeUpdate()
    {
        yield return new WaitForSeconds(1/1);
        Destroy(gameObject);
    }
}
