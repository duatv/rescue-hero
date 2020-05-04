using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class screenshot : MonoBehaviour {
    
   public bool TakeAShot = false;
	
	
	// Update is called once per frame
	void Update () {

        if (Application.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeAShot = false;
                ScreenCapture.CaptureScreenshot("shot_" + Time.time + ".png", 2);
            }
        }
        else {
            if (TakeAShot)
            {
                TakeAShot = false;
                ScreenCapture.CaptureScreenshot("shot_" + Time.time + ".png", 2);
            }
        }

		
	}
}
