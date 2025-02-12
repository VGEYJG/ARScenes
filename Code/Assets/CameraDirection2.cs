using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using System.IO;

[ExecuteInEditMode]
public class CameraDirection2 : MonoBehaviour
{
    private void OnEnable()
    {
        MainDo();

    }
    
    /// <summary>
    /// 同步欧氏距离获取关键帧
    /// </summary>
    void MainDo()
    {
        //读取原始数据
        List<ARPose> allPose = GetAllpose();
        List<int> resKeys = GeyKeys(allPose, 41);
        string outputResKeys = "";
        foreach (var p in resKeys)
        {
            outputResKeys += (p+1) + " ";
        }
        Debug.Log(outputResKeys);
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
    List<ARPose> GetAllpose()
    {
        List<ARPose> allPose = new List<ARPose>();
        string path = "E:\\BaiduSyncdisk\\MyEpan\\SWJTU\\论文\\博士开题\\开题报告\\小论文\\时空语义约束的增强现实动态场景快速建模方法\\实验\\数据\\原始数据2024年6月24日083615\\位置1\\同步欧氏距离2024年11月19日085602\\";
        foreach (string line in File.ReadLines(path + "PositionPose.txt"))
        {
            string[] v = line.Split(' ');
            Quaternion q = new Quaternion(float.Parse(v[3]), float.Parse(v[4]), float.Parse(v[5]), float.Parse(v[6]));
            ARPose arPose = new ARPose() { num = int.Parse(v[0]) - 1, iskey = false, time = int.Parse(v[1]), pose = q };
            allPose.Add(arPose);
        }
        return allPose;
    }
    //获取关键帧
    List<int> GeyKeys(List<ARPose> allPose, int keyN )
    {
        List<int> keyPose = new List<int>();
        //设置首尾为关键帧
        ARPose arpose = allPose[0];
        arpose.iskey = true;
        allPose[0] = arpose;
        keyPose.Add(0);

        arpose = allPose[allPose.Count - 1];
        arpose.iskey = true;
        allPose[allPose.Count - 1] = arpose;
        keyPose.Add(allPose.Count - 1);

        //定义相似轨迹的起始位置
        int start = 0;
        ARPose p0 = allPose[0];

        int stop = allPose.Count - 1;
        ARPose p1 = allPose[allPose.Count - 1];

        //定义最大偏差的特征点
        float jdMax = 0;
        int numMax = 0;
            //迭代过程，输入需要的关键帧数量N，以及初始位姿数据 M*6,6：isKey + 时间戳 + 姿态四元数
        while (keyPose.Count < keyN) 
        {
            jdMax = 0;
            numMax = 0;
            //计算所有非关键帧与similarPose上对应近似帧之间的同步时空欧式距离，
            foreach (var p in allPose)
            {
                //处理所有非关键帧
                if (p.iskey == false)
                {
                    //更新关键帧起止点位置p0, p1
                    Vector2 p0p1 = Getp0p1(p, keyPose);
                    p0 = allPose[(int)p0p1.x];
                    p1 = allPose[(int)p0p1.y];

                    Quaternion pose2 = GetsimilarPose(p, p0, p1);
                    float jd = Mathf.Rad2Deg * Mathf.Acos(Quaternion.Dot(p.pose, pose2));
                    if (jd > jdMax)//选取最大距离点作为关键帧
                    {
                        jdMax = jd;
                        numMax = p.num;//将该关键帧加入到similarPose序列中
                    }
                }
            }
            arpose =allPose[numMax];
            arpose.iskey = true;
            allPose[numMax] = arpose;

            keyPose.Add(numMax);
            keyPose.Sort();
            Debug.Log(numMax+1);
        }
        return keyPose;
    }
    //计算同步时空姿态
    Quaternion GetsimilarPose(ARPose pi, ARPose p0, ARPose p1)//, List<ARPose>  similarPoseTrace
    {
        //计算pi的起始段
        float t = (pi.time - p0.time) / (p1.time - p0.time);
        Quaternion res = Quaternion.Slerp(p0.pose, p1.pose, t);
        return res;
    }
    //计算关键帧起止点位置p0, p1
    Vector2 Getp0p1(ARPose pi, List<int> keyPose)
    {
        for (int i = 0; i < keyPose.Count-1; i++)
        {
            if (pi.num > keyPose[i] && pi.num < keyPose[i + 1])
            {
                return new Vector2(keyPose[i], keyPose[i + 1]);
            }
        }
        return Vector2.zero;
    }
    //每一帧的数据结构
    public struct ARPose 
    {
        public int num;
        public bool iskey;
        public float time;
        public Quaternion pose;


        //public void SetKey()
        //{
        //    iskey = true;
        //}
        
    }
}
