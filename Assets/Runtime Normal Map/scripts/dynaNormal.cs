using UnityEngine;
using System.Collections;

public class dynaNormal : MonoBehaviour {

	public float bumpiness = 0.0f;
	public Texture2D myNormal;
	public Texture2D myBckg;

	// Use this for initialization
	void Start () {
		Debug.Log(this.renderer.sharedMaterial.GetTexture("_MainTex").name);
		myBckg = Resources.Load(this.renderer.sharedMaterial.GetTexture("_MainTex").name) as Texture2D;
		Texture2D myTexture = GrayScaleToNormalMap.CreateDOT3 (myBckg, bumpiness, 0.0f);
		this.renderer.sharedMaterial.SetTexture("_BumpMap", myTexture);	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.UpArrow)){
			Debug.Log ("pressed");
			bumpiness++;
			Texture2D myTexture = GrayScaleToNormalMap.CreateDOT3 (myBckg, bumpiness, 0.0f);
			this.renderer.sharedMaterial.SetTexture("_BumpMap", myTexture);	
		}
	}
}
