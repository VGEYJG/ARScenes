using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using System.IO;

[ExecuteInEditMode]
public class CameraDirection : MonoBehaviour
{
    public Vector3 v1, v2, v3, v4;
    public Material dsmMat;
    //计算
    void Point3DtoScreen()
    {
        Vector3 point3D1 = GameObject.Find("point1").transform.position; // 左侧建筑角点
        Vector3 point3D2 = GameObject.Find("point2").transform.position; // 右侧建筑角点
        Vector3 point3D3 = GameObject.Find("point3").transform.position; // 右2窗户左下角点
        Vector4 point3D4 = GameObject.Find("point4").transform.position; // 第三根柱子顶端
        Vector4 point3D5 = GameObject.Find("point5").transform.position; // 第4虚线右
        Vector4 point3D6 = GameObject.Find("point6").transform.position; // 第5虚线左
        Vector4 point3D7 = GameObject.Find("point7").transform.position; // 右侧窗户右上角点
        Vector4 point3D8 = GameObject.Find("point8").transform.position; // 第6虚线左

        int i = 0;
        float disMin = float.MaxValue;
        Vector3 positionRes = Vector3.zero;
        Vector3 rotationRes = Vector3.zero;
        float HfovRes = 60f;
        while (i++ < 1000000)
        {
            Vector3 positionRandom = new Vector3(130.86f, 485.07f, -236.97f) + new Vector3(Random.Range(-8, 8), Random.Range(-2, 2), Random.Range(-8, 8));
            Vector3 rotationRandom = new Vector3(-5f, 258.6f, -0f) + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
            float Hfov = Random.Range(25, 55);
            Camera.main.transform.localPosition = positionRandom;
            Camera.main.transform.localEulerAngles = rotationRandom;
            Camera.main.fieldOfView = Hfov;

            Vector3 point2D1 = Camera.main.WorldToScreenPoint(point3D1);
            float dis1 = Mathf.Sqrt((point2D1.x - 64 * 2) * (point2D1.x - 64 * 2) + (point2D1.y - 720 + 123 * 2) * (point2D1.y - 720 + 123 * 2));
            Vector3 point2D2 = Camera.main.WorldToScreenPoint(point3D2);
            float dis2 = Mathf.Sqrt((point2D2.x - 291 * 2) * (point2D2.x - 291 * 2) + (point2D2.y - 720 + 156 * 2) * (point2D2.y - 720 + 156 * 2));
            Vector3 point2D3 = Camera.main.WorldToScreenPoint(point3D3);
            float dis3 = Mathf.Sqrt((point2D3.x - 357 * 2) * (point2D3.x - 357 * 2) + (point2D3.y - 720 + 230 * 2) * (point2D3.y - 720 + 230 * 2));
            Vector3 point2D4 = Camera.main.WorldToScreenPoint(point3D4);
            float dis4 = Mathf.Sqrt((point2D4.x - 221 * 2) * (point2D4.x - 221 * 2) + (point2D4.y - 720 + 154 * 2) * (point2D4.y - 720 + 154 * 2));
            Vector3 point2D5 = Camera.main.WorldToScreenPoint(point3D5);
            float dis5 = Mathf.Sqrt((point2D5.x - 279 * 2) * (point2D5.x - 279 * 2) + (point2D5.y - 720 + 268 * 2) * (point2D5.y - 720 + 268 * 2));
            Vector3 point2D6 = Camera.main.WorldToScreenPoint(point3D6);
            float dis6 = Mathf.Sqrt((point2D6.x - 300 * 2) * (point2D6.x - 300 * 2) + (point2D6.y - 720 + 275 * 2) * (point2D6.y - 720 + 275 * 2));
            Vector3 point2D7 = Camera.main.WorldToScreenPoint(point3D7);
            float dis7 = Mathf.Sqrt((point2D7.x - 587 * 2) * (point2D7.x - 587 * 2) + (point2D7.y - 720 + 142 * 2) * (point2D7.y - 720 + 142 * 2));
            Vector3 point2D8 = Camera.main.WorldToScreenPoint(point3D8);
            float dis8 = Mathf.Sqrt((point2D8.x - 447 * 2) * (point2D8.x - 447 * 2) + (point2D8.y - 720 + 623 * 2) * (point2D8.y - 720 + 623 * 2));


            float dis = (dis1 + dis2 + dis3 + dis4 + dis5 + dis6 + dis7 ) / 7;
            if (dis < disMin)
            {
                disMin = dis;
                Debug.Log("迭代位置：" + i);
                Debug.Log("距离更新：" + dis);
                Debug.Log("位置更新：" + positionRandom);
                Debug.Log("姿态更新：" + rotationRandom);
                Debug.Log("dis:" + dis2 + ",\n" + dis3 + "\n" + dis5 + "\n" + dis6 + "\n" + dis7 + "\n");
                positionRes = positionRandom;
                rotationRes = rotationRandom;
                HfovRes = Hfov;
            }
        }
        Camera.main.transform.localPosition = positionRes;
        Camera.main.transform.localEulerAngles = rotationRes;
        Camera.main.fieldOfView = HfovRes;

    }

    //加载倾斜模型
    public void LoadDSM()
    {
        Transform parent = GameObject.Find("model").transform;
        string path = "E:\\BaiduSyncdisk\\MyEpan\\SWJTU\\unity\\projects\\SWJTU\\Assets\\Resources\\";
        string[] files = Directory.GetFiles(path, "*.obj", SearchOption.AllDirectories);
        //Debug.Log(files.Length);//2648
        foreach (string fileName in files)
        {
            string[] strfilename = fileName.Split('\\');
            //Debug.Log(strfilename[strfilename.Length-1].Split('.')[0]);
            GameObject goTemp = Resources.Load<GameObject>(strfilename[strfilename.Length - 1].Split('.')[0]);
            if (goTemp != null)
            {
                GameObject go = Instantiate(goTemp);
                go.transform.parent = parent;
                go.transform.localEulerAngles = new Vector3(0, 0, 0);
            }

        }

    }

    //设置倾斜模型显隐
    public void ShowDSM()
    {
        MeshRenderer[] allDsm = GameObject.Find("model").GetComponentsInChildren<MeshRenderer>();
        Debug.Log(allDsm.Length);
        Material[] mats = new Material[1];
        mats[0] = dsmMat;//=寻找某文件夹下name+"_0.mat"的文件
        foreach (MeshRenderer p in allDsm)
        {
            p.materials= mats;
        }
    }
    public void HideDSM()
    {
        MeshRenderer[] allDsm = GameObject.Find("model").GetComponentsInChildren<MeshRenderer>();
        Debug.Log(allDsm.Length);
        Material[] mats = new Material[1];
        mats[0] = dsmMat;
        foreach (MeshRenderer p in allDsm)
        {
            p.materials = mats;
        }
    }
    void GetPhoto()
    {
        //定义RenderTexture的大小和格式
        RenderTexture rt = new RenderTexture(1280, 720, 24);
        //将相机组件的目标纹理设置为新建的RenderTexture
        Camera.main.targetTexture = rt;
        Camera.main.cullingMask = 1 << 0; // 只渲染目标层级
        Camera.main.Render();

        //激活RenderTexture并将它读取到一张2D贴图（texture）中
        RenderTexture.active = rt;
        Texture2D screenshotTexture = new Texture2D(1280, 720, TextureFormat.ARGB32, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, 1280, 720), 0, 0);
        screenshotTexture.Apply();

        //激活主窗口，重置以后可能会显示bug，所以替代方案是创建一个全屏的Image去覆盖画面，然后不激活窗口也行。
        Camera.main.targetTexture = null;
        Camera.main.cullingMask = -7;
        RenderTexture.active = null;

        //将截屏贴图保存到本地
        byte[] bytes = screenshotTexture.EncodeToPNG();
        File.WriteAllBytes("E:/" + ".png", bytes);//+ (int)Time.realtimeSinceStartup * 100
        Debug.Log("截屏成功");
    }

    private void OnEnable()
    {
        //Point3DtoScreen();

        //LoadDSM();
        //HideDSM();
        GetPhoto();

        //Quaternion camerarotation = Camera.main.transform.rotation;
        //Debug.Log("camerarotation: " + camerarotation);
        //Quaternion q1 = Quaternion.Euler(344.9333f, 96.27331f, 88.10528f);
        //Debug.Log("q1:"+q1);
        //Debug.Log("q1Inverse:" + Quaternion.Inverse(q1));
        //Quaternion rotation = camerarotation * Quaternion.Inverse(q1);
        //Debug.Log("rotation:" + rotation);
        //Quaternion rotation2 = rotation * q1;
        //Debug.Log("rotation2"+rotation2);
        //Debug.Log("rotation3" + GameObject.Find("Camera").transform.rotation);
        //GameObject.Find("Camera").transform.rotation = rotation2;
        //Debug.Log("rotation4" + GameObject.Find("Camera").transform.rotation);

        //Quaternion p = Quaternion.Euler(339.7562f, 95.51752f, 85.76811f);
        //Quaternion rotation = Camera.main.transform.rotation * Quaternion.Inverse(p);
        //Debug.Log("rotation:" + rotation.x + "," + rotation.y + "," + rotation.z + "," + rotation.w);
        //GameObject.Find("Camera").transform.rotation = rotation * p;

        //Quaternion p1 = new Quaternion(0.6680592f, 0.3000539f, 0.1610983f, -0.661598f);
        //Quaternion p2 = new Quaternion(0.6349916f, 0.3393168f, 0.2230887f, -0.6571769f);
        //Quaternion p12 = Quaternion.Slerp(p1, p2, 0.294617563739377f);
        //Debug.Log(p12.x + "," + p12.y + "," + p12.z + "," + p12.w);

        //Quaternion p = Quaternion.Euler(343.0906f, 95.89059f, 88.52879f);
        //Quaternion rotation = new Quaternion(0.6587617f, 0.311832f, 0.179483f, -0.6607417f);
        //Quaternion q = rotation * p;
        //GameObject.Find("Camera").transform.rotation = q;


        //Quaternion p1 = new Quaternion(0.6680592f, 0.3000539f, 0.1610983f, -0.661598f);
        ////Quaternion p2 = new Quaternion(0.6587617f, 0.311832f, 0.179483f, -0.6607417f);
        ////Debug.Log(p2.eulerAngles.x + "," + p2.eulerAngles.y + "," + p2.eulerAngles.z);
        ////Quaternion p123 = Quaternion.Euler(p2.eulerAngles.x, p2.eulerAngles.y, p2.eulerAngles.z);
        ////Debug.Log(p123.x + "," + p123.y + "," + p123.z + "," + p123.w);

        //Quaternion p3 = new Quaternion(0.6349916f, 0.3393168f, 0.2230887f, -0.6571769f);
        //Debug.Log(Quaternion.Dot(p1, p2));//0.999718
        //Debug.Log(Quaternion.Dot(p2, p3));//0.9983827
        //Debug.Log(Quaternion.Dot(p1, p3));//0.9967514

        //Quaternion p1 = new Quaternion(0.662111f, 0.3038008f, 0.1849784f, -0.6596189f);
        //Quaternion p2 = new Quaternion(0.6587617f, 0.311832f, 0.179483f, -0.6607417f);
        //Debug.Log(Quaternion.Dot(p1, p2));//0.9999464

        //Quaternion p1 = Quaternion.Euler(343.0906f, 95.89059f, 88.52879f);
        //Quaternion q = new Quaternion(0.6587617f, 0.311832f, 0.179483f, -0.6607417f);
        //Quaternion r = q * p1;
        //GameObject.Find("Camera").transform.rotation = r;


        //Quaternion p1 = Quaternion.Euler(343.0906f, 95.89059f, 88.52879f);
        //Quaternion p2 = Quaternion.Euler(-4f, 66.41f, 0.6f);
        //Quaternion r = p2 * Quaternion.Inverse(p1);
        //Debug.Log(r.x + "," + r.y + "," + r.z + "," + r.w);

        //Euler2Quaternion();
        //Loadobj();

        //1、获取相机真实姿态
        //GetCameraQuaternion();
        //2、获取关键帧的姿态修正量
        //GetDquaternion(new Quaternion(-0.776565f, 0.04284224f, 0.02313423f, -0.628153f),new Quaternion(0.01788218f, 0.9785888f, 0.0968137f, -0.1807523f));
        //GetDquaternion(new Quaternion(-0.7831377f, 0.2281779f, 0.1951023f, -0.544578f), new Quaternion(-0.04663672f, -0.9435095f, -0.1511257f, 0.291163f));
        //3、插值每一帧的修正量
        //SlerpQ(0.106724616684776f, new Quaternion(-0.170089837f, - 0.531363836f, - 0.817336175f ,   0.143818071f), new Quaternion(-0.429677519f, - 0.661154323f, - 0.422934225f,    0.446518494f));
        //4、计算每一帧的修正后姿态
        //dQQ(new Quaternion(0.4589104f, 0.3607922f, 0.6846291f, -0.4364781f),
        //new Quaternion(-0.723272f, 0.2168779f, 0.2069486f, -0.6221044f));
        //5、获取所有帧的准确度
        //GetQ1Q2acc(new Quaternion(-0.04361326f, -0.9092568f, -0.1557609f, 0.3835211f), 
        //new Quaternion(-0.01891589f, -0.954082f, -0.06169771f, 0.2925118f));

        //MainDo();

        //GameObject.Find("Camera").transform.rotation = new Quaternion(-0.0436132f, -0.9092569f, -0.1557609f, 0.3835211f);
        //GameObject.Find("Camera").transform.rotation = new Quaternion(-0.01891589f, -0.954082f, -0.06169771f, 0.2925118f);


        //GetAccs();

        //SetCameraAsSlerpQ();

        //GetLij();
    }
    //
    void GetCameraQuaternion()
    {
        Quaternion camerarotation = Camera.main.transform.rotation;
        Debug.Log(System.DateTime.Now + "camerarotation: " + camerarotation.x + "," + camerarotation.y + "," + camerarotation.z + "," + camerarotation.w);
    }
    void GetDquaternion(Quaternion q1, Quaternion q2)
    {
        Quaternion dq = q2 * Quaternion.Inverse(q1);
        Debug.Log(System.DateTime.Now + "dq: " + dq.x + "," + dq.y + "," + dq.z + "," + dq.w);
    }
    void SlerpQ(float t, Quaternion p1, Quaternion p2)
    {
        Quaternion p12 = Quaternion.Slerp(p1, p2, t);
        Debug.Log(System.DateTime.Now + " " + t.ToString() + " SlerpQ: " + p12.x + "," + p12.y + "," + p12.z + "," + p12.w);
    }
    void dQQ(Quaternion dq, Quaternion q)
    {
        Quaternion q2 = dq * q;
        Debug.Log(System.DateTime.Now + " dQQ: " + q2.x + "," + q2.y + "," + q2.z + "," + q2.w);
    }
    void GetQ1Q2acc(Quaternion q1, Quaternion q2)
    {
        float q = Quaternion.Dot(q1, q2);
        Debug.Log(System.DateTime.Now + " Q1Q2acc: " + q);
    }

    void Euler2Quaternion()
    {
        string resEdt = "";
        string resSab = "";
        string resSab2 = "";
        string resEdtSab = "";

        List<float[]> resf = new List<float[]>();
        string path = "E:\\BaiduSyncdisk\\MyEpan\\SWJTU\\论文\\博士开题\\开题报告\\小论文\\一种视听融合的增强现实场景高效建模方法\\基于关键帧的增强现实场景快速建模方法\\实验\\数据\\原始数据2024年4月8日081513\\";
        foreach (string line in File.ReadLines(path + "PositionPose.txt"))
        {
            string[] v4 = line.Split(' ');
            //Quaternion q =new Quaternion(float.Parse(v4[6]), float.Parse(v4[7]), float.Parse(v4[8]), float.Parse(v4[9]));
            //res += v4[0] + " " + q.x + " " + q.y + " " + q.z + " " + q.w + "\n";
            float[] a = new float[] { float.Parse(v4[0]), float.Parse(v4[6]), float.Parse(v4[7]), float.Parse(v4[8]), float.Parse(v4[9]) };
            resf.Add(a);
        }
        float dtMax = resf[resf.Count - 1][0] - resf[0][0];
        float arfa = 1f / (dtMax);//exp(-2.3)=0.1；arfa越大，时间差的影响权重越小
        float Sabmin = 1, Sabmax = 0;
        for (int i = 0; i < resf.Count; i++)
        {
            float[] a = resf[i];
            Quaternion q1 = new Quaternion(a[1], a[2], a[3], a[4]);
            for (int j = 0; j < resf.Count; j++)
            {
                float[] b = resf[j];
                Quaternion q2 = new Quaternion(b[1], b[2], b[3], b[4]);
                float Edt = 1 - Mathf.Abs(a[0] - b[0]) / dtMax;
                float Sab = Quaternion.Dot(q1, q2) / 2 + 0.5f;
                Sabmin = Sab < Sabmin ? Sab : Sabmin;
                Sabmax = Sab > Sabmax ? Sab : Sabmax;
                //float EdtSab = Edt * Sab;
                resEdt += Edt + " ";
                resSab += Sab + " ";
            }
            resEdt += "\n";
            resSab += "\n";
        }
        string[] resSabfi = resSab.Split('\n');
        string[] resEdtfi = resEdt.Split('\n');
        float dSab = Sabmax - Sabmin;
        float arfa2 = 0.9f;
        for (int i = 0; i < resSabfi.Length; i++)
        {
            if (resSabfi[i] != "")
            {
                string[] resSabfj = resSabfi[i].Split(' ');
                string[] resEdtfj = resEdtfi[i].Split(' ');

                for (int j = 0; j < resSabfj.Length; j++)
                {
                    if (resSabfj[j] != "")
                    {
                        resSab2 += (float.Parse(resSabfj[j]) - Sabmin) / dSab + " ";
                        resEdtSab += (arfa2 * float.Parse(resEdtfj[j]) + (1 - arfa2) * ((float.Parse(resSabfj[j]) - Sabmin) / dSab)) + " ";
                    }
                }
                resSab2 += "\n";
                resEdtSab += "\n";
            }
        }
        File.WriteAllText(path + "EdtLij.txt", resEdt);
        File.WriteAllText(path + "SabLij.txt", resSab);
        File.WriteAllText(path + "EdtSabLij.txt", resEdtSab);
    }

    void MainDo2()
    {
        //读取原始数据
        //168行*19列（0timeStamp,1isKey,2poseX,3poseY,4poseZ,5poseW,6truePoseX,7truePoseY,8truePoseZ,9truePoseW,10DqX,11DqY,12DqZ,13DqW,14DqQX,15DqQY,16DqQZ,17DqQW,18Q2Acc）
        List<float[]> allPose = new List<float[]>();
        string path = "E:\\BaiduSyncdisk\\MyEpan\\SWJTU\\论文\\博士开题\\开题报告\\小论文\\一种视听融合的增强现实场景高效建模方法\\基于关键帧的增强现实场景快速建模方法\\实验\\数据\\原始数据2024年4月8日081513\\";
        foreach (string line in File.ReadLines(path + "PositionPose2.txt"))
        {
            string[] v = line.Split(' ');
            float[] a;
            //获取关键帧的姿态修正量
            if (float.Parse(v[1]) == 1)
            {

                Quaternion q1 = new Quaternion(float.Parse(v[2]), float.Parse(v[3]), float.Parse(v[4]), float.Parse(v[5]));
                Quaternion q2 = new Quaternion(float.Parse(v[6]), float.Parse(v[7]), float.Parse(v[8]), float.Parse(v[9]));
                Quaternion dq = q2 * Quaternion.Inverse(q1);

                a = new float[] { float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]), float.Parse(v[3]), float.Parse(v[4]), float.Parse(v[5]), float.Parse(v[6]), float.Parse(v[7]), float.Parse(v[8]), float.Parse(v[9]), dq.x, dq.y, dq.z, dq.w, 0, 0, 0, 0, 0 };
            }
            else
            {
                a = new float[] { float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]), float.Parse(v[3]), float.Parse(v[4]), float.Parse(v[5]), float.Parse(v[6]), float.Parse(v[7]), float.Parse(v[8]), float.Parse(v[9]), 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            allPose.Add(a);
        }
        Quaternion keyFrame1Dq = new Quaternion(allPose[0][10], allPose[0][11], allPose[0][12], allPose[0][13]);
        Quaternion keyFrame2Dq = new Quaternion(allPose[9][10], allPose[9][11], allPose[9][12], allPose[9][13]);
        foreach (var v in allPose)
        {
            if (v[1] == 0)//非关键帧
            {
                //插值每一帧的修正量
                Quaternion dq = Quaternion.Slerp(keyFrame1Dq, keyFrame2Dq, (v[0] - allPose[0][0]) / (allPose[9][0] - allPose[0][0]));
                v[10] = dq.x;
                v[11] = dq.y;
                v[12] = dq.z;
                v[13] = dq.w;
            }
            //计算每一帧的修正后姿态
            Quaternion q2 = new Quaternion(v[10], v[11], v[12], v[13]) * new Quaternion(v[2], v[3], v[4], v[5]);
            v[14] = q2.x;
            v[15] = q2.y;
            v[16] = q2.z;
            v[17] = q2.w;

            //获取所有帧的准确度
            float acc = Quaternion.Dot(new Quaternion(v[6], v[7], v[8], v[9]), q2) / 2 + 0.5f;
            v[18] = acc;
        }

        //GetDquaternion(new Quaternion(-0.7831377f, 0.2281779f, 0.1951023f, -0.544578f), new Quaternion(-0.04663672f, -0.9435095f, -0.1511257f, 0.291163f));
        //3、插值每一帧的修正量
        //SlerpQ(0.874125874f, new Quaternion(0.5081285f, 0.3963024f, 0.5782599f, -0.500365f), new Quaternion(0.4030154f, 0.3199264f, 0.7750325f, -0.366811f));
        //4、计算每一帧的修正后姿态
        //dQQ(new Quaternion(0.4589104f, 0.3607922f, 0.6846291f, -0.4364781f),
        //new Quaternion(-0.723272f, 0.2168779f, 0.2069486f, -0.6221044f));
        //5、获取所有帧的准确度
        //GetQ1Q2acc(new Quaternion(-0.04361326f, -0.9092568f, -0.1557609f, 0.3835211f), 
        //new Quaternion(-0.01891589f, -0.954082f, -0.06169771f, 0.2925118f));

        WriteRes(allPose, path);
    }
    
    /// <summary>
    /// 同步欧氏距离获取关键帧
    /// </summary>
    void MainDo()
    {
        //读取原始数据
        //168行*19列（0timeStamp,1isKey,2poseX,3poseY,4poseZ,5poseW,6truePoseX,7truePoseY,8truePoseZ,9truePoseW,10DqX,11DqY,12DqZ,13DqW,14DqQX,15DqQY,16DqQZ,17DqQW,18Q2Acc）
        List<float[]> allPose = new List<float[]>();
        string path = "E:\\BaiduSyncdisk\\MyEpan\\SWJTU\\论文\\博士开题\\开题报告\\小论文\\一种视听融合的增强现实场景高效建模方法\\基于关键帧的增强现实场景快速建模方法\\实验\\数据\\原始数据2024年4月8日081513\\";
        foreach (string line in File.ReadLines(path + "PositionPose2.txt"))
        {
            string[] v = line.Split(' ');
            float[] a;
            //获取关键帧的姿态修正量
            if (float.Parse(v[1]) == 1)
            {

                Quaternion q1 = new Quaternion(float.Parse(v[2]), float.Parse(v[3]), float.Parse(v[4]), float.Parse(v[5]));
                Quaternion q2 = new Quaternion(float.Parse(v[6]), float.Parse(v[7]), float.Parse(v[8]), float.Parse(v[9]));
                Quaternion dq = q2 * Quaternion.Inverse(q1);

                a = new float[] { float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]), float.Parse(v[3]), float.Parse(v[4]), float.Parse(v[5]), float.Parse(v[6]), float.Parse(v[7]), float.Parse(v[8]), float.Parse(v[9]), dq.x, dq.y, dq.z, dq.w, 0, 0, 0, 0, 0 };
            }
            else
            {
                a = new float[] { float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]), float.Parse(v[3]), float.Parse(v[4]), float.Parse(v[5]), float.Parse(v[6]), float.Parse(v[7]), float.Parse(v[8]), float.Parse(v[9]), 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            allPose.Add(a);
        }
        Quaternion keyFrame1Dq = new Quaternion(allPose[0][10], allPose[0][11], allPose[0][12], allPose[0][13]);
        Quaternion keyFrame2Dq = new Quaternion(allPose[9][10], allPose[9][11], allPose[9][12], allPose[9][13]);
        foreach (var v in allPose)
        {
            if (v[1] == 0)//非关键帧
            {
                //插值每一帧的修正量
                Quaternion dq = Quaternion.Slerp(keyFrame1Dq, keyFrame2Dq, (v[0] - allPose[0][0]) / (allPose[9][0] - allPose[0][0]));
                v[10] = dq.x;
                v[11] = dq.y;
                v[12] = dq.z;
                v[13] = dq.w;
            }
            //计算每一帧的修正后姿态
            Quaternion q2 = new Quaternion(v[10], v[11], v[12], v[13]) * new Quaternion(v[2], v[3], v[4], v[5]);
            v[14] = q2.x;
            v[15] = q2.y;
            v[16] = q2.z;
            v[17] = q2.w;

            //获取所有帧的准确度
            float acc = Quaternion.Dot(new Quaternion(v[6], v[7], v[8], v[9]), q2) / 2 + 0.5f;
            v[18] = acc;
        }

        //GetDquaternion(new Quaternion(-0.7831377f, 0.2281779f, 0.1951023f, -0.544578f), new Quaternion(-0.04663672f, -0.9435095f, -0.1511257f, 0.291163f));
        //3、插值每一帧的修正量
        //SlerpQ(0.874125874f, new Quaternion(0.5081285f, 0.3963024f, 0.5782599f, -0.500365f), new Quaternion(0.4030154f, 0.3199264f, 0.7750325f, -0.366811f));
        //4、计算每一帧的修正后姿态
        //dQQ(new Quaternion(0.4589104f, 0.3607922f, 0.6846291f, -0.4364781f),
        //new Quaternion(-0.723272f, 0.2168779f, 0.2069486f, -0.6221044f));
        //5、获取所有帧的准确度
        //GetQ1Q2acc(new Quaternion(-0.04361326f, -0.9092568f, -0.1557609f, 0.3835211f), 
        //new Quaternion(-0.01891589f, -0.954082f, -0.06169771f, 0.2925118f));

        WriteRes(allPose, path);
    }
    //存储结果
    void WriteRes(List<float[]> allPose, string path)
    {
        string res = "";
        foreach (var line in allPose)
        {
            foreach (var v in line)
            {
                res += v.ToString() + " ";
            }
            res += "\n";
        }
        File.WriteAllText(path + "PositionPose_OK.txt", res);
    }
    void Location2PositionPose()
    {

    }

    //获取首尾关键帧的时空插值精度
    void GetAcc()
    {
        //读取原始数据
        List<float[]> allPose = new List<float[]>();//t;posex,y,z,w;truex,y,z,w
        List<float[]> outPose = new List<float[]>();//dqx,y,z,w;dqposex,y,z,w;acc

        string path = "E:\\BaiduSyncdisk\\MyEpan\\SWJTU\\论文\\博士开题\\开题报告\\小论文\\一种视听融合的增强现实场景高效建模方法\\基于关键帧的增强现实场景快速建模方法\\实验\\数据\\原始数据2024年6月24日083615\\位置1\\实验结果\\";
        foreach (string line in File.ReadLines(path + "PositionPose.txt"))
        {
            string[] v = line.Split(' ');
            allPose.Add(new float[] { float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]), float.Parse(v[3]), float.Parse(v[4]), float.Parse(v[5]), float.Parse(v[6]), float.Parse(v[7]), float.Parse(v[8]) });
        }
        Quaternion dq1= new Quaternion(allPose[0][5], allPose[0][6], allPose[0][7], allPose[0][8]) * Quaternion.Inverse(new Quaternion(allPose[0][1], allPose[0][2], allPose[0][3], allPose[0][4]));
        Quaternion ddq1 = dq1 * new Quaternion(allPose[0][1], allPose[0][2], allPose[0][3], allPose[0][4]);
        float acc1= Quaternion.Dot(ddq1, new Quaternion(allPose[0][5], allPose[0][6], allPose[0][7], allPose[0][8]));
        outPose.Add(new float[] { dq1.x, dq1.y, dq1.z, dq1.w , ddq1.x, ddq1.y, ddq1.z, ddq1.w, acc1 });//首帧

        int num = allPose.Count;
        Quaternion dqN = new Quaternion(allPose[num - 1][5], allPose[num - 1][6], allPose[num - 1][7], allPose[num - 1][8]) * Quaternion.Inverse(new Quaternion(allPose[num - 1][1], allPose[num - 1][2], allPose[num - 1][3], allPose[num - 1][4]));
        Quaternion ddqN = dqN * new Quaternion(allPose[num - 1][1], allPose[num - 1][2], allPose[num - 1][3], allPose[num - 1][4]);
        float accN = Quaternion.Dot(ddqN, new Quaternion(allPose[num - 1][5], allPose[num - 1][6], allPose[num - 1][7], allPose[num - 1][8]));

        float t1 = allPose[0][0];
        float tN = allPose[num-1][0];

        for (int i = 1; i < allPose.Count-1; i++)//中间插值帧
        {
            Quaternion dqi = Quaternion.Slerp(dq1, dqN, (allPose[i][0] - t1) / (tN - t1));
            Quaternion ddqi = dqi * new Quaternion(allPose[i][1], allPose[i][2], allPose[i][3], allPose[i][4]);
            float acci = Quaternion.Dot( ddqi,new Quaternion(allPose[i][5], allPose[i][6], allPose[i][7], allPose[i][8]));
            outPose.Add(new float[] { dqi.x, dqi.y, dqi.z, dqi.w, ddqi.x, ddqi.y, ddqi.z, ddqi.w, acci });//首帧
        }

        outPose.Add(new float[] { dqN.x, dqN.y, dqN.z, dqN.w, ddqN.x, ddqN.y, ddqN.z, ddqN.w, accN });//尾帧

        WriteRes(outPose, path);
        Debug.Log("GetAcc_OK!");
    }

    //根据关键帧位置计算全局时空插值精度
    void GetAccs()
    {
        //读取原始数据
        List<float[]> allPose = new List<float[]>();//t;posex,y,z,w;truex,y,z,w
        List<float[]> outPose = new List<float[]>();//dqx,y,z,w;dqposex,y,z,w;acc

        string path = "E:\\BaiduSyncdisk\\MyEpan\\SWJTU\\论文\\博士开题\\开题报告\\小论文\\时空语义约束的增强现实动态场景快速建模方法\\实验\\数据\\原始数据2024年6月24日083615\\位置1\\实验结果\\";
        foreach (string line in File.ReadLines(path + "PositionPose.txt"))
        {
            if (line.Length > 0)
            {
                string[] v = line.Split(' ');
                allPose.Add(new float[] { float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]), float.Parse(v[3]), float.Parse(v[4]), float.Parse(v[5]), float.Parse(v[6]), float.Parse(v[7]), float.Parse(v[8]) });
            }
        }
        Quaternion dq1 = new Quaternion(allPose[0][5], allPose[0][6], allPose[0][7], allPose[0][8]) * Quaternion.Inverse(new Quaternion(allPose[0][1], allPose[0][2], allPose[0][3], allPose[0][4]));
        Quaternion ddq1 = dq1 * new Quaternion(allPose[0][1], allPose[0][2], allPose[0][3], allPose[0][4]);
        float acc1 = Quaternion.Dot(ddq1, new Quaternion(allPose[0][5], allPose[0][6], allPose[0][7], allPose[0][8]));
        outPose.Add(new float[] { dq1.x, dq1.y, dq1.z, dq1.w, ddq1.x, ddq1.y, ddq1.z, ddq1.w, acc1 });//首帧

        int num = allPose.Count;
        Quaternion dqN = new Quaternion(allPose[num - 1][5], allPose[num - 1][6], allPose[num - 1][7], allPose[num - 1][8]) * Quaternion.Inverse(new Quaternion(allPose[num - 1][1], allPose[num - 1][2], allPose[num - 1][3], allPose[num - 1][4]));
        Quaternion ddqN = dqN * new Quaternion(allPose[num - 1][1], allPose[num - 1][2], allPose[num - 1][3], allPose[num - 1][4]);
        float accN = Quaternion.Dot(ddqN, new Quaternion(allPose[num - 1][5], allPose[num - 1][6], allPose[num - 1][7], allPose[num - 1][8]));

        List<int> Keys = new List<int>(){
1,11 , 22 , 32 , 43  ,53 , 63 , 74 , 84 , 94  ,105 ,115, 126, 136


        };
        Keys.Sort();
        int keysNN = Keys.Count;
        Debug.Log(keysNN);
        for (int i = 1; i < allPose.Count - 1; i++)//中间插值帧
        {
            if (i+1 > Keys[1])
            {
                Keys.RemoveAt(0);
            }
            //计算当前的首尾帧
            int a = Keys[0]-1, b = Keys[1]-1;
            float t1i = allPose[a][0];
            float tNi = allPose[b][0];
            Quaternion dq1i = new Quaternion(allPose[a][5], allPose[a][6], allPose[a][7], allPose[a][8]) * Quaternion.Inverse(new Quaternion(allPose[a][1], allPose[a][2], allPose[a][3], allPose[a][4]));
            Quaternion dqNi = new Quaternion(allPose[b][5], allPose[b][6], allPose[b][7], allPose[b][8]) * Quaternion.Inverse(new Quaternion(allPose[b][1], allPose[b][2], allPose[b][3], allPose[b][4]));
            Quaternion dqi = Quaternion.Slerp(dq1i, dqNi, (allPose[i][0] - t1i) / (tNi - t1i));
            Quaternion ddqi = dqi * new Quaternion(allPose[i][1], allPose[i][2], allPose[i][3], allPose[i][4]);
            float acci = Quaternion.Dot(ddqi, new Quaternion(allPose[i][5], allPose[i][6], allPose[i][7], allPose[i][8]));
            outPose.Add(new float[] { dqi.x, dqi.y, dqi.z, dqi.w, ddqi.x, ddqi.y, ddqi.z, ddqi.w, acci });
        }

        outPose.Add(new float[] { dqN.x, dqN.y, dqN.z, dqN.w, ddqN.x, ddqN.y, ddqN.z, ddqN.w, accN });//尾帧

        //WriteRes(outPose, path);
        //Debug.Log("GetAcc_OK!");

        WriteAllAcc(outPose, keysNN);
        DebugMaxAcc(outPose);
    }
    void WriteAllAcc(List<float[]> allPose, int keyN)
    {
        float sumAcc = 0;
        foreach (var line in allPose)
        {
            if (line[8].ToString() != "1")
            {
                float a = line[8];
                float b = Mathf.Acos(a);
                float c = Mathf.Rad2Deg * b;
                sumAcc += c;
            }
        }
        Debug.Log(sumAcc / (136 - keyN));
    }

    void DebugMaxAcc(List<float[]> allPose)
    {
        float maxAcc = 0;
        int resI = 0;
        for (int i = 0;i< allPose.Count;i++)
        {
            if (allPose[i][8].ToString() != "1")
            {
                float a = allPose[i][8];
                float b = Mathf.Acos(a);
                float c = Mathf.Rad2Deg * b;
                if (c > maxAcc)
                {
                    maxAcc = c;
                    resI = i + 1;
                }
            }
        }
        Debug.Log(maxAcc+","+ resI);
    }
    //设置相机姿态为插值得到的位姿四元数
    void SetCameraAsSlerpQ()
    {
        //Camera.main.transform.rotation = new Quaternion(0.005553945f, 0.9511961f, 0.01323362f, -0.3082529f);//1625714
        Camera.main.transform.rotation = new Quaternion(0.01730816f, 0.9489973f, 0.06109627f, -0.3088233f);//1624942
    }

    void GetLij()
    {
        //读取原始数据
        List<float[]> allPose = new List<float[]>();//t;x,y,z,w
        float rt = 4828.02107964322f;
        float rx = 0.0777384621659451f;
        float ry = 0.140994946176377f;
        float rz = 0.129908401562649f;
        float rw = 0.0599896609956336f;

        List<float[]> outPose = new List<float[]>();//Lij

        string path = "E:\\BaiduSyncdisk\\MyEpan\\SWJTU\\论文\\博士开题\\开题报告\\小论文\\一种视听融合的增强现实场景高效建模方法\\基于关键帧的增强现实场景快速建模方法\\实验\\数据\\原始数据2024年6月24日083615\\位置1\\实验结果\\";
        foreach (string line in File.ReadLines(path + "PositionPose.txt"))
        {
            if (line.Length > 0)
            {
                string[] v = line.Split(' ');
                allPose.Add(new float[] { float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]), float.Parse(v[3]), float.Parse(v[4]) });
            }
        }
        int num = allPose.Count;
        float disMax = 0;
        for (int i = 0; i < num; i++)
        {
            float[] linei = new float[num];
            for (int j = 0; j < num; j++)
            {
                if (j < i)
                {
                    linei[j] = outPose[j][i];
                }
                else if (j == i)
                {
                    linei[j] = 1;
                }
                else if (j > i)
                {
                    float disSum = 0;
                    for (int k = i; k < j; k++)
                    {
                        disSum += Mathf.Sqrt(
                            Mathf.Pow((allPose[k + 1][0] - allPose[k][0]) / rt, 2) +
                            Mathf.Pow((allPose[k + 1][1] - allPose[k][1]) / rx, 2) +
                            Mathf.Pow((allPose[k + 1][2] - allPose[k][2]) / ry, 2) +
                            Mathf.Pow((allPose[k + 1][3] - allPose[k][3]) / rz, 2) +
                            Mathf.Pow((allPose[k + 1][4] - allPose[k][4]) / rw, 2));
                    }
                    linei[j] = disSum;
                    if (disSum > disMax)
                    {
                        disMax = disSum;
                    }
                }
            }
            outPose.Add(linei);
        }
        for (int i = 0; i < num; i++)//归一化处理
        {
            for (int j = 0; j < num; j++)
            {
                if (j != i)
                {
                    outPose[i][j] = 1 - outPose[i][j] / disMax;
                }
            }
        }
        WriteRes(outPose, path);
        Debug.Log("GetLij_OK!");
    }
}
