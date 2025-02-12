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
        Debug.Log("�����ɹ�");
        //����RenderTexture�Ĵ�С�͸�ʽ
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        //����������Ŀ����������Ϊ�½���RenderTexture
        Camera.main.targetTexture = rt;
        Camera.main.cullingMask = 1 << 0; // ֻ��ȾĿ��㼶
        Camera.main.Render();

        //����RenderTexture��������ȡ��һ��2D��ͼ��texture����
        RenderTexture.active = rt;
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        //���������ڣ������Ժ���ܻ���ʾbug��������������Ǵ���һ��ȫ����Imageȥ���ǻ��棬Ȼ�󲻼����Ҳ�С�
        Camera.main.targetTexture = null;
        Camera.main.cullingMask = -1;
        RenderTexture.active = null;

        //��������ͼ���浽����
        byte[] bytes = screenshotTexture.EncodeToPNG();
        File.WriteAllBytes("E:/" + (int)Time.realtimeSinceStartup * 100 + ".png", bytes);
        Debug.Log("�����ɹ�");
    }
   
    public static void RefreshGallery(string path)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // ����һ��ʵ���Է���AndroidJavaClass
            AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            // �������ȡ��ǰ���Activity
            AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            // ����MediaScannerConnection.scanFile����
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
        //����RenderTexture�Ĵ�С�͸�ʽ
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        //����������Ŀ����������Ϊ�½���RenderTexture
        Camera.main.targetTexture = rt;
        Camera.main.cullingMask = 1 << 0; // ֻ��ȾĿ��㼶
        Camera.main.Render();

        //����RenderTexture��������ȡ��һ��2D��ͼ��texture����
        RenderTexture.active = rt;
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        //���������ڣ������Ժ���ܻ���ʾbug��������������Ǵ���һ��ȫ����Imageȥ���ǻ��棬Ȼ�󲻼����Ҳ�С�
        Camera.main.targetTexture = null;
        Camera.main.cullingMask = -1;
        RenderTexture.active = null;

        //��������ͼ���浽����
        byte[] bytes = screenshotTexture.EncodeToPNG();
        string timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        string fileName = "Screenshot" + Time.frameCount + ".png";
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        System.IO.File.WriteAllBytes("/storage/emulated/0/DCIM/ARscreenshot/" + fileName, bytes);//Application.persistentDataPath
        RefreshGallery("/storage/emulated/0/DCIM/ARscreenshot/" + fileName);//ˢ��һ��
        Debug.Log("�����ɹ�");
    }

}
