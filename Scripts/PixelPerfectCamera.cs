using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour {

    //artworks pixel per unit value
    public static float pixelsToUnits = 1f;
    public static float scale = 1f;

    public Vector2 nativeResolution = new Vector2(800,600);
    private void Awake()
    {
        var camera = GetComponent<Camera>();

        if (camera.orthographic)
        {
            //testing the height value
            var dir = Screen.height;
            var res = nativeResolution.y;
            scale = dir / res;
            pixelsToUnits *= scale;

            camera.orthographicSize = (dir / 2.0f) / pixelsToUnits;
        }
    }
}
