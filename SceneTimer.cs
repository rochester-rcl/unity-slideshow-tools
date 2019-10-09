using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlideshowTools
{
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
        // TODO Check input here and call OnSceneDurationEnd if a button is pressed
        void Update()
        {
            if (duration < Time.timeSinceLevelLoad && timesUp == false)
            {

                timesUp = true;
                if (OnSceneDurationEnd != null)
                {
                    OnSceneDurationEnd();
                }
            }
        }
    }
}
