using UnityEngine;
using System.Collections;

public class testScreenshot : MonoBehaviour
{
    //public int resWidth = 2550;
    //public int resHeight = 3300;
    public int resWidth = 1536;
    public int resHeight = 2048;

    private bool takeHiResShot = false;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{ 0}/ screenshots / screen_{ 1}x{ 2}_{ 3}.png",Application.dataPath,width, height,System.DateTime.Now.ToString("yyyy - MM - dd_HH - mm - ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            Debug.Log("takeHiResShot");
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            var camera1 = GetComponent<Camera>();
                camera1.targetTexture = rt;
            Debug.Log("1");
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera1.Render();
            Debug.Log("2");
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            camera1.targetTexture = null;
            Debug.Log("3");
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            Debug.Log("4");
            byte[] bytes = screenShot.EncodeToPNG();
            Debug.Log("5");
            //string filename = ScreenShotName(resWidth, resHeight);
            Debug.Log(Application.dataPath);
            string filename = Application.dataPath + "/test2.png";
            Debug.Log("6");
            System.IO.File.WriteAllBytes(filename, bytes);
            //Debug.Log(string.Format("Took screenshot to: { 0}", filename));
            takeHiResShot = false;
        }
    }
}