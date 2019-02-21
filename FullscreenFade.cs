using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenFade : MonoBehaviour {
	// private properties
	private GameObject fadeCanvas;
	private Image fadeImage;
	private bool fadeInTriggered  = false;
	private float timeSinceFadeInTriggered = 0.0f;
	private bool fadeOutTriggered = false;
	private float timeSinceFadeOutTriggered = 0.0f;
	private float padding = 0.2f;
	private enum EASING { IN, OUT };
	// public properties
	public float fadeDuration = 2.0f;
	public bool useTimer = false;
	public Vector2 referenceResolution = new Vector2(1440, 2560);
	public Color fadeColor;
	public delegate void FadeInBegins();
	public delegate void FadeOutBegins();
	public delegate void FadeInEnds();
	public delegate void FadeOutEnds();
	public static event FadeInBegins OnFadeInBegins;
	public static event FadeOutBegins OnFadeOutBegins;
	public static event FadeInEnds OnFadeInEnds;
	public static event FadeOutEnds OnFadeOutEnds;
	void OnEnable() {
		if (useTimer) {
			initFadeImage();
			SceneTimer.OnSceneDurationStart += triggerFadeIn;
			SceneTimer.OnSceneDurationEnd += triggerFadeOut;
		}
	}

	void OnDisable() {
		if (useTimer) {
			SceneTimer.OnSceneDurationStart -= triggerFadeIn;
			SceneTimer.OnSceneDurationEnd -= triggerFadeOut;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeInTriggered) {
			timeSinceFadeInTriggered += Time.deltaTime;
			doFadeIn();
		} else if (fadeOutTriggered) {
			timeSinceFadeOutTriggered += Time.deltaTime;
			doFadeOut();
		}
	}

	/*** Fades ***/
	public void initFadeImage() {
		fadeCanvas = new GameObject();
		fadeCanvas.name = "CanvasOverlay";
		fadeCanvas.AddComponent<Canvas>();
		fadeCanvas.AddComponent<CanvasScaler>();
		Canvas canvas = fadeCanvas.GetComponent<Canvas>();
		CanvasScaler scaler = fadeCanvas.GetComponent<CanvasScaler>();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		scaler.matchWidthOrHeight = 1;
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		canvas.worldCamera = Camera.main;
		canvas.planeDistance = Camera.main.nearClipPlane + 0.001f;
		canvas.overrideSorting = true;
		fadeCanvas.AddComponent<Image>();
		fadeImage = fadeCanvas.GetComponent<Image>();
		fadeImage.preserveAspect = false;
		// FYI Color.black has an alpha of 0 so we need to construct the color ourselves
		fadeImage.color = fadeColor != null ? fadeColor : new Color(0.0f, 0.0f, 0.0f, 1.0f);
		fadeImage.enabled = useTimer ? true : false;
		fadeImage.rectTransform.anchorMin = Vector2.zero;
		fadeImage.rectTransform.anchorMax = Vector2.one;
		fadeImage.rectTransform.sizeDelta = Vector2.zero;
		// fadeImage.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
	}

	public void triggerFadeIn() {
		fadeImage.enabled = true;
		fadeInTriggered = true;
		if (OnFadeInBegins != null) {
			OnFadeInBegins();
		}
	}

	public void triggerFadeOut() {
		fadeImage.enabled = true;
		fadeOutTriggered = true;
		if (OnFadeOutBegins != null) {
			OnFadeOutBegins();
		}
	}

	public void finishFadeIn() {
		fadeInTriggered = false;
		fadeImage.enabled = false;
		timeSinceFadeInTriggered = 0.0f;
		if (OnFadeInEnds != null) {
			OnFadeInEnds();
		}
	}

	public void finishFadeOut() {
		fadeOutTriggered = false;
		timeSinceFadeOutTriggered = 0.0f;
		if (OnFadeOutEnds != null) {
			OnFadeOutEnds();
		}
	}

	private float ease(EASING mode) {
		float delta = mode == EASING.IN ? timeSinceFadeInTriggered : timeSinceFadeOutTriggered;
		return Mathf.SmoothStep(0.0f, 1.0f, (1.0f / (fadeDuration - delta) - padding));
	}

	public void doFadeOut() {
		if (fadeImage.color.a < 1.0f) {
			fadeImage.color = Color.Lerp(Color.clear, fadeColor, ease(EASING.OUT));
		} else {
			finishFadeOut();
		}
	}

	public void doFadeIn() {
		if (fadeImage.color.a > 0.0f) {
			fadeImage.color = Color.Lerp(fadeColor, Color.clear, ease(EASING.IN));
		} else {
			finishFadeIn();
		}
	}
}
