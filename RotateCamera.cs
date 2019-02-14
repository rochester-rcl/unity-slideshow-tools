using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateCamera : MonoBehaviour {
	public float speed;
	public GameObject go;

	public bool fitCamera = false;
	private float fov;
	private bool rotationStarted = false;
	// Use this for initialization
	void Start () {
		this.fitVerticalFOV();	
	}

	void OnEnable() {
		FullscreenFade.OnFadeInEnds += startRotation;
	}

	void OnDisable() {
		FullscreenFade.OnFadeInEnds += startRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (fitCamera) {
			Camera.main.fieldOfView = this.fov;
		}
		if (rotationStarted) {
			transform.Translate(Vector3.right * (speed * Time.deltaTime));
			transform.LookAt(go.transform);
		}
	}

	void startRotation() {
		rotationStarted = true;
	}

	void fitVerticalFOV() {
		Camera mainCam = Camera.main;
		try {
			Mesh mesh = this.go.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh;
			float meshHeight = mesh.bounds.max.y - mesh.bounds.min.y;
			float dist = Vector3.Distance(mainCam.transform.position, mesh.bounds.min);
			this.fov = 2 * Mathf.Atan(meshHeight / (2 * dist)) * (180 / Mathf.PI);
		} catch(NullReferenceException e) {
			this.fov = mainCam.fieldOfView;
		}
		
	}
}
