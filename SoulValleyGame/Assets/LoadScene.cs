using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour, ITimeTracker
{
    private void Start() {
        TimeManager.Instance.RegisterTracker(this);
    }
    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        TimeManager.Instance.UnregisterTracker(this);
        Destroy(gameObject);
    }
}
