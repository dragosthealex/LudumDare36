using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour {
	public Material material1;
	public Material material2;
	public Material material3;

	public bool active;

	public float duration = 1.0F;
	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
		rend.material = material1;
	}
	void Update() {
		float lerp = Mathf.PingPong (Time.time, duration) / duration;
		if (!active) {
			rend.material.Lerp (material1, material2, lerp);
		} else {
			rend.material.Lerp (material2, material3, lerp);
		}
	}

	public void activate() {
		active = true;
		StartCoroutine ("IncreaseLight", 1.5f);
	}

	IEnumerator IncreaseLight(float value) {
		Light light = GetComponentInChildren<Light> ();
		while (light.intensity < value) {
			light.intensity += 0.005f;
			yield return new WaitForFixedUpdate ();
		}
	}
}
