var textureColor		:	Texture2D;		// Input Color Texture
var generatedNormal		:	Texture2D;		// Normal Map Texture

internal var url		:	String = "http://www.francescogallorini.com/wp-content/uploads/25366_1121252231.jpg"; // Url For pick color texture

private var	tryDownload			:	boolean = false;
private	var tryProcess			:	boolean = false;
private	var contrast			:	float = 4.0;
private	var treshold			:	float = 0.0;



function TryDownload () {
	tryDownload = true;
	var www = new WWW ( url );
	yield www;
	if ( !www.error && www.texture) {
		textureColor = new Texture2D ( 512, 512 );
		textureColor = www.texture;
		tryDownload = false;
	}
	else {
		tryDownload = false;
	}
}

function TryProcess () {
	tryProcess = true;
	generatedNormal =  GrayScaleToNormalMap.CreateDOT3 ( textureColor, contrast, treshold );
	yield WaitForEndOfFrame();
	tryProcess = false;
}

function TrySave() {
	var bytes = generatedNormal.EncodeToPNG();
	System.IO.File.WriteAllBytes(Application.dataPath + "/../NormalMap.png", bytes);

}


function OnGUI () {
	GUI.Label( Rect ( 0,0,100,30 ), "Type Image URL " );
	GUI.Label( Rect ( 0,40,100,30 ), "Contrast ["+contrast.ToString("f2")+"]" );
	GUI.Label( Rect ( 0,80,100,30 ), "Treshold ["+treshold.ToString("f2")+"]" );
	url = GUI.TextField ( Rect ( 100,0,Screen.width-110,30 ), url );
	
	contrast = GUI.HorizontalSlider (Rect (115, 45, 400, 30), contrast, -100.0, 100.0);
	treshold = GUI.HorizontalSlider (Rect (115, 85, 400, 30), treshold, 0.0, 1.0);
	if ( url != "" ) {
		if ( GUI.Button ( Rect ( Screen.width/4,125+Screen.width/2,100,30 ), "Download" ) && !tryDownload ) {
			TryDownload ( );
		}
	}
	
	if ( textureColor ) {
		if ( GUI.Button ( Rect ( (Screen.width/2)+Screen.width/4,125+Screen.width/2,100,30 ), "Process" ) && !tryProcess ) {
			TryProcess();
		}
	}
	
	if ( generatedNormal ) {
		if ( GUI.Button ( Rect ( (Screen.width/2)+Screen.width/4+105,125+Screen.width/2,100,30 ), "Save" ) && !tryProcess ) {
			TrySave();
		}
		
	}
	
	GUI.Box ( Rect ( 0,120,Screen.width/2,Screen.width/2), textureColor );
	GUI.Box ( Rect ( Screen.width/2,120,Screen.width/2,Screen.width/2), generatedNormal );
	
}