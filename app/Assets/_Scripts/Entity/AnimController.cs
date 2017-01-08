using UnityEngine;
using System.Collections;

public class AnimController : MonoBehaviour {

	public GameObject animatedModel;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = animatedModel.GetComponent<Animator> ();
	}

	public void SetShooting(bool value) {
		animator.SetBool ("shooting", value);
	}

	public void SetShoot() {
		animator.SetTrigger ("shoot");
	}

	public void SetGrab() {
		animator.SetTrigger ("grab");
	}

	public void SetLaunch() {
		animator.SetTrigger ("launch");
	}
}
