using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System.IO;
using UnityEngine.UI;

public class TakePhoto : MonoBehaviour
{
    [ExecuteInEditMode]
    private void OnEnable()
    {
        Debug.Log("截屏成功");
        //定义RenderTexture的大小和格式
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        //将相机组件的目标纹理设置为新建的RenderTexture
        Camera.main.targetTexture = rt;
        Camera.main.cullingMask = 1 << 0; // 只渲染目标层级
        Camera.main.Render();

        //激活RenderTexture并将它读取到一张2D贴图（texture）中
        RenderTexture.active = rt;
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        //激活主窗口，重置以后可能会显示bug，所以替代方案是创建一个全屏的Image去覆盖画面，然后不激活窗口也行。
        Camera.main.targetTexture = null;
        Camera.main.cullingMask = -1;
        RenderTexture.active = null;

        //将截屏贴图保存到本地
        byte[] bytes = screenshotTexture.EncodeToPNG();
        File.WriteAllBytes("E:/" + (int)Time.realtimeSinceStartup * 100 + ".png", bytes);
        Debug.Log("截屏成功");
    }
   
    public static void RefreshGallery(string path)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // 创建一个实例以访问AndroidJavaClass
            AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            // 从上面获取当前活动的Activity
            AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            // 调用MediaScannerConnection.scanFile方法
            AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", "android.intent.action.MEDIA_SCANNER_SCAN_FILE");
            AndroidJavaObject objFile = new AndroidJavaObject("java.io.File", path);
            AndroidJavaObject objUri = classUri.CallStatic<AndroidJavaObject>("fromFile", objFile);

            objIntent.Call<AndroidJavaObject>("setData", objUri);
            objActivity.Call("sendBroadcast", objIntent);
        }
        else
        {
            Debug.Log("This feature is not available on this platform.");
        }
    }


    public void TakeScreenshot()
    {
        //定义RenderTexture的大小和格式
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        //将相机组件的目标纹理设置为新建的RenderTexture
        Camera.main.targetTexture = rt;
        Camera.main.cullingMask = 1 << 0; // 只渲染目标层级
        Camera.main.Render();

        //激活RenderTexture并将它读取到一张2D贴图（texture）中
        RenderTexture.active = rt;
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        //激活主窗口，重置以后可能会显示bug，所以替代方案是创建一个全屏的Image去覆盖画面，然后不激活窗口也行。
        Camera.main.targetTexture = null;
        Camera.main.cullingMask = -1;
        RenderTexture.active = null;

        //将截屏贴图保存到本地
        byte[] bytes = screenshotTexture.EncodeToPNG();
        string timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        string fileName = "Screenshot" + Time.frameCount + ".png";
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        System.IO.File.WriteAllBytes("/storage/emulated/0/DCIM/ARscreenshot/" + fileName, bytes);//Application.persistentDataPath
        RefreshGallery("/storage/emulated/0/DCIM/ARscreenshot/" + fileName);//刷新一下
        Debug.Log("截屏成功");
    }

}
