using UnityEngine;
using System.Collections;

public class GrayScaleToNormalMap : MonoBehaviour {


public static float Sgn ( float val ) {
	float retSign = 0f;
	if ( val > 0 ) retSign = 1f;
	if ( val < 0 ) retSign = -1f;
	return retSign;
}

public static float GetPixelGray ( int x, int y, Color[] colorList, int tWidth ) {
	float grayOut = 0f;
	grayOut = colorList[x+(tWidth*y)].grayscale*255;
	return grayOut;
}


public static Texture2D CreateDOT3 (Texture2D pixmap, float contrast, float treshold ) {
	
	Texture2D  retTexture = new Texture2D (pixmap.width, pixmap.height, TextureFormat.ARGB32, false );
	Color[] pixColor = pixmap.GetPixels();
	Color[] retColor = new Color[pixColor.Length];
	for (int x = 0; x<pixmap.width; x++ ) {
		for (int y = 0; y<pixmap.height; y++ ) {
			
			float 	tl	= -1.0f;
			float 	tm	= -1.0f;
			float 	tr	= -1.0f;
			float 	ml	= -1.0f;
			float	mm	= -1.0f;
			float	mr	= -1.0f;
			float	bl	= -1.0f;
			float	bm	= -1.0f;
			float	br	= -1.0f;
			
			if ( x>0 && y>0 ) {
				tl  =	GetPixelGray(x-1,y-1,pixColor,pixmap.width);
				if ( treshold > 0 ) {
					if ( tl > treshold ) tl = 255f;
					else tl = 0;
				}
			}	
			if ( y>0 ) {
				tm  =	GetPixelGray(x,y-1,pixColor,pixmap.width);
				if ( treshold > 0 ) {
					if ( tm > treshold ) tm = 255f;
					else tm = 0;
				}
			}
			if ( x<pixmap.width-1 && y>0 ) {				
				tr  =	GetPixelGray(x+1,y-1,pixColor,pixmap.width);
				if ( treshold > 0 ) {
					if ( tr > treshold ) tr = 255f;
					else tr = 0;
				}
			}
			if ( x>0 ) {										
				ml  =	GetPixelGray(x-1,y,pixColor,pixmap.width);
				if ( treshold > 0 ) {
					if ( ml > treshold ) ml = 255f;
					else ml = 0;
				}
				mm  =	GetPixelGray(x,y,pixColor,pixmap.width);
				if ( treshold > 0 ) {
					if ( mm > treshold ) mm = 255f;
					else mm = 0;
				}
			}
			if ( x<pixmap.width-1 )	{					
				mr  =	GetPixelGray(x+1,y,pixColor,pixmap.width);
				if ( treshold > 0 ) {
					if ( mr > treshold ) mr = 255f;
					else mr = 0;
				}
			}
			if ( x>0 && y<pixmap.height-1 ) {		
				bl  =	GetPixelGray(x-1,y+1,pixColor,pixmap.width);
				if ( treshold > 0 ) {
					if ( bl > treshold ) bl = 255f;
					else bl = 0;
				}
			}
			if ( y<pixmap.height-1 ) {		
				bm  =	GetPixelGray(x,y+1,pixColor,pixmap.width);
				if ( treshold > 0 ) {
					if ( bm > treshold ) bm = 255f;
					else bm = 0;
				}
			}
			if ( x<pixmap.width-1 && y<pixmap.height-1 ) {	
				br  =	GetPixelGray(x+1,y+1,pixColor,pixmap.width);
				if ( treshold > 0 ) {
					if ( br > treshold ) br = 255f;
					else br = 0;
				}
			}
			
			if ( tl == -1.0f ) tl = mm;
			if ( tm == -1.0f ) tm = mm;
			if ( tr == -1.0f ) tr = mm;
			if ( ml == -1.0f ) ml = mm;
			if ( mr == -1.0f ) mr = mm;
			if ( bl == -1.0f ) bl = mm;
			if ( bm == -1.0f ) bm = mm;
			if ( br == -1.0f ) br = mm;
			
			float	vx	=	0.0f;
			float	vy	=	0.0f;
			float	vz	=	1.0f;
			
			float	isq2	=	1.0f/Mathf.Sqrt(2.0f);
			float	sum		=	1.0f+isq2+isq2;
						
			float al	=	(tl*isq2+ml+bl*isq2)/sum;
			float ar	=	(tr*isq2+mr+br*isq2)/sum;
			float at	=	(tl*isq2+tm+tr*isq2)/sum;
			float ab	=	(bl*isq2+bm+br*isq2)/sum;			

			vx	=	(al-ar)/255.0f*contrast;
			vy	=	(at-ab)/255.0f*contrast;


			float r	=	vx*128.5f+128.5f;
			float g	=	vy*128.5f+128.5f;
			float b	=	vz*255.0f;
			
			if ( r<0 )		r=0f;
			if ( r>255 )	r=255f;
			if ( g<0 )		g=0f;
			if ( g>255 ) 	g=255f;
			if ( b<0 )		b=0f;
			if ( b>255 ) 	b=255f;
			
			Color rgb = new Color(r/255f,g/255f,b/255f,0.5f);
			
			retColor[x+(pixmap.width*y)] = rgb;
		}
	}
	retTexture.SetPixels(retColor);
	retTexture.Apply();
	return retTexture;
}


}
