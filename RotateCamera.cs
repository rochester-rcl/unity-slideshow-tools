using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlideshowTools
{
    public class RotateCamera : MonoBehaviour
    {
        public float speed;
        public GameObject go;
        public bool fitCamera = false;
        public bool useFade = false;
        private float fov;
        private bool rotationStarted = false;
        void Start()
        {
            this.fitVerticalFOV();
        }

        void OnEnable()
        {

            SceneTimer.OnSceneDurationStart += startRotation;
        }

        void OnDisable()
        {
            SceneTimer.OnSceneDurationStart -= startRotation;

        }

        // Update is called once per frame
        void Update()
        {
            if (fitCamera)
            {
                Camera.main.fieldOfView = this.fov;
            }
            if (rotationStarted)
            {
                transform.Translate(Vector3.right * (speed * Time.deltaTime));
                transform.LookAt(go.transform);
            }
        }

        void startRotation()
        {
            rotationStarted = true;
        }

        void fitVerticalFOV()
        {
            Camera mainCam = Camera.main;
            try
            {
                Mesh mesh = this.go.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh;
                float meshHeight = mesh.bounds.max.y - mesh.bounds.min.y;
                float dist = Vector3.Distance(mainCam.transform.position, mesh.bounds.min);
                this.fov = 2 * Mathf.Atan(meshHeight / (2 * dist)) * (180 / Mathf.PI);
            }
            catch (Exception)
            {
                this.fov = mainCam.fieldOfView;
            }

        }
    }

}
