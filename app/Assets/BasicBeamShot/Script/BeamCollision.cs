using UnityEngine;
using System.Collections;

public class BeamCollision : MonoBehaviour {
	
    public bool Reflect = false;
	public float maxDist;

	private BeamLine BL;

	public GameObject HitEffect = null;

	private bool bHit = false;

	private BeamParam BP;

	// Use this for initialization
	void Start () {
		BL = (BeamLine)this.gameObject.transform.FindChild("BeamLine").GetComponent<BeamLine>();
		BP = this.transform.root.gameObject.GetComponent<BeamParam>();
	}
	
	// Update is called once per frame
	void Update () {
		//RayCollision
		RaycastHit hit;
		int layerMask = ~(1 << LayerMask.NameToLayer("NoBeamHit") | 1 << 2);
		if (!bHit && Physics.Raycast(transform.position, transform.forward, out hit, maxDist, layerMask))
		{
			GameObject hitobj = hit.collider.gameObject;
			if(hit.distance < BL.GetNowLength() && hitobj.tag != "me")
			{
				BL.StopLength(hit.distance);
				bHit = true;

				Quaternion Angle;
				//Reflect to Normal
				if (Reflect)
				{
					Angle = Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal));
				}
				else
				{
					Angle = Quaternion.AngleAxis(180.0f, transform.up) * this.transform.rotation;
				}
				GameObject obj = (GameObject)Instantiate(HitEffect,this.transform.position+this.transform.forward*hit.distance,Angle);
				obj.GetComponent<BeamParam>().SetBeamParam(BP);
				obj.transform.localScale = this.transform.localScale;
			}
		}
		/*
		if(bHit && BL != null)
		{
			BL.gameObject.renderer.material.SetFloat("_BeamLength",HitTimeLength / BL.GetNextLength() + 0.05f);
		}
		*/
	}
}
