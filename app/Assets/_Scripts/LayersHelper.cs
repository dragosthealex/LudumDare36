using UnityEngine;
using System.Collections;

public class LayersHelper : MonoBehaviour {

	private Camera camera;

	void Awake () {
		camera = Camera.main;
	}

	// Turn on the bit using an OR operation:
	public void Show(string layer) {
		camera.cullingMask |= 1 << LayerMask.NameToLayer(layer);
	}

	// Turn off the bit using an AND operation with the complement of the shifted int:
	public void Hide(string layer) {
		camera.cullingMask &=  ~(1 << LayerMask.NameToLayer(layer));
	}

	// Toggle the bit using a XOR operation:
	public void Toggle(string layer) {
		camera.cullingMask ^= 1 << LayerMask.NameToLayer(layer);
	}
}
