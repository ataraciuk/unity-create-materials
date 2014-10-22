using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

[InitializeOnLoad]
public class LoadTextureAndSave {

	static LoadTextureAndSave() {
		Material baseMat = new Material(Shader.Find("Bumped Specular"));
		List<string> textureFiles = Directory.GetFiles("Assets/Resources/textures").Where(x => !x.EndsWith("meta") && !x.EndsWith("Store")).ToList();
		int i = 0;
		textureFiles.ForEach(delegate(string s) {
			Texture2D tex = AssetDatabase.LoadAssetAtPath(s, typeof(Texture2D)) as Texture2D;
			Debug.Log (s);
			for(int j = 0; j < 11; j++) {
				Material toSave = new Material(baseMat);
				Texture2D bump = NormalMap(tex, j);
				toSave.SetTexture("_BumpMap", bump);
				toSave.SetTexture("_MainTex", tex);
				string[] parts = s.Split('/');
				string name = parts[parts.Length-1];
				string[] nameParts = name.Split('.');
				string extension = nameParts[nameParts.Length-1];
				AssetDatabase.CreateAsset(bump, "Assets/Resources/bumpMaps/"+name+"-"+j+"."+extension);
				AssetDatabase.CreateAsset(toSave, "Assets/Resources/materials/"+i+"-"+j+".mat");
				Resources.UnloadAsset(bump);
				Resources.UnloadAsset(toSave);
				Debug.Log ("created: "+i + " - " + j);
			}
			Resources.UnloadAsset(tex);
			i++;
		});
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	private static Texture2D NormalMap(Texture2D source,float strength) {
		strength=Mathf.Clamp(strength,0.0F,10.0F);
		Texture2D result;
		float xLeft;
		float xRight;
		float yUp;
		float yDown;
		float yDelta;
		float xDelta;
		result = new Texture2D (source.width, source.height, TextureFormat.ARGB32, true);
		for (int by=0; by<result.height; by++) {
			for (int bx=0; bx<result.width; bx++) {
				xLeft = source.GetPixel(bx-1,by).grayscale*strength;
				xRight = source.GetPixel(bx+1,by).grayscale*strength;
				yUp = source.GetPixel(bx,by-1).grayscale*strength;
				yDown = source.GetPixel(bx,by+1).grayscale*strength;
				xDelta = ((xLeft-xRight)+1)*0.5f;
				yDelta = ((yUp-yDown)+1)*0.5f;
				result.SetPixel(bx,by,new Color(xDelta,yDelta,1.0f,yDelta));
			}
		}
		result.Apply();
		return result;
	}
}
