using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SceneTimer : MonoBehaviour
{
    public float duration;
    public delegate void SceneDurationStart();
    public delegate void SceneDurationEnd();
    public static event SceneDurationStart OnSceneDurationStart;
    public static event SceneDurationEnd OnSceneDurationEnd;
    private bool timesUp = false;
    // Use this for initialization
    void Start()
    {
        if (OnSceneDurationStart != null)
        {
            OnSceneDurationStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (duration < Time.timeSinceLevelLoad && timesUp == false)
        {
            // dispatch something
            timesUp = true;
            if (OnSceneDurationEnd != null)
            {
                OnSceneDurationEnd();
            }
        }
    }
}
