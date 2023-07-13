using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    [Header("Internal clock")]
    [SerializeField] GameTimeStamp timeStamp;
    public float timeScale = 1.0f;
    [Header("Day & Night cycle")]
    public Transform sunTranform;
    //List of Object to inform the change of time
    List<ITimeTracker> listeners = new List<ITimeTracker>();
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else { Instance = this; };
        SaveLoad.OnSaveData += SaveTimeData;
        SaveLoad.OnLoadGame += LoadTimeData;
    }
    private void Start()
    {
        timeStamp = new GameTimeStamp(0, GameTimeStamp.Season.Spring, 1, 7, 0);
        StartCoroutine(TimeUpdate());
    }
    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1/timeScale);
        }
    }
    public void Tick()
    {
        timeStamp.UpdateClock();
        foreach(ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timeStamp);
        }
        UpdateSunMovement();
    }
    void UpdateSunMovement()
    {
        int timeInMinute = GameTimeStamp.HoursToMinutes(timeStamp.hour) + timeStamp.minute;
        //Sun move 15 degree in a hour => 0.25 in minute
        //At midnight (0:00) the angle of the sun should be -90
        float sunAngle = .25f * timeInMinute - 90;
        sunTranform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }
    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }
    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }
    public GameTimeStamp GetTimeStamp()
    {
        return new GameTimeStamp(timeStamp);
    }
    void SaveTimeData(){
        SaveGameManager.data.timeData = Instance.timeStamp;
    }
    void LoadTimeData(SaveData data){
        Instance.timeStamp = data.timeData;
    }
}
