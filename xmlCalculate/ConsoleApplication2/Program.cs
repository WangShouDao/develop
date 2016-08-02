using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo TheFolder = new DirectoryInfo("G://热图/热图/两半球/两半球captrue1");  //选取指定文件夹
            List<string> fileNames = new List<string>();
            foreach (FileInfo NextFile in TheFolder.GetFiles())    //遍历当前文件夹下文件目录并添加到fileNames中
            {
                fileNames.Add(NextFile.Name);
            }

            double[] Arr = new double[76800] ;   //用来保存最后的结果
            for (int i = 0; i < 76800; i++)
                Arr[i] = 0;
            int[] a = new int[] {55625,90001,88967,88439,88685, 87950,90000, 53207, 89989, 90002, 90002, 90002, 89957, 89996, 90001, 89968,89969,89982, 89997, 89964, 89995, 89970, 90001,90002,90002,90002,89792,90003, 90002, 90001, 89993,90002, 89996, 89981, 90001, 90000, 90001,90002, 90001, 89997, 90001, 90002, 90002, 90002,90002,90002,90002, 90001,90002,90002,90001,90003, 90001,90003,90001, 89996, 90001,90002, 90002,90001, 90001, 90001, 90001,89996,90001, 85300,76704, 75696, 75118, 79204, 89938, 90000, 90002, 89995,90002,90003, 90002,89669,90003, 89788,89830,90002,90002,90001, 90001, 90001,89999,89922, 87795,86770, 79923,76174,74149,76898, 89955,90002, 90002, 90002,90001,90004,90002, 90002,88824,89978,90001, 90003,89976,90002,90003,90001,90002};
            //两半球captrue2int[] a = new int[] {55603,90001,89635, 87307,88679, 88097, 90001, 53208, 90002,90004, 90002, 90003, 89940, 90002, 89999, 85924,52791, 50783,51534, 50157, 50566, 55034, 89136, 68104, 89079, 89987, 89908, 90002, 90003,90002, 90003, 90003, 90002, 90002, 90001, 90003, 90002,90000, 90002, 89888, 81892, 80576,77541,76487,74711,74990,87812, 90002, 80508, 88000,90003, 90003, 90002,90003, 90000, 90003,90000, 90002,90002, 90002, 90002,90001,90002, 89872, 86861, 82025,59094, 56688, 57533, 56096, 56329, 78198, 90002, 89991,90001, 90002, 90002, 89698,90002, 89857, 89999, 90000, 90004, 90002, 90000,90004, 89912, 88666,42286, 42594,37978, 35907, 35236,35216, 61620, 89898, 89992,89978, 90002, 90003, 90004, 90003,89279, 89998, 90002,90002,89998,90002, 90004,90002,90000};
            
            for (int j = 0; j < fileNames.Count; j++)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("G://热图/热图/两半球/两半球captrue1/" + fileNames[j]);    //加载Xml文件  
                XmlElement rootElem = doc.DocumentElement;   //获取根节点  
                XmlNodeList captrueNodes = rootElem.GetElementsByTagName("captrue1"); //获取captrue1子节点集合  
                string[] numStrs = new string[100000];

                int i = 0;
                foreach (XmlNode node in captrueNodes)
                {
                    XmlNodeList subDataNodes = ((XmlElement)node).GetElementsByTagName("data");  //获取data中XmlElement集合                      
                    string strdata = subDataNodes[0].InnerText;   //读取data中的字符
                    numStrs = strdata.Split('.');   //拆分字符
                    foreach (string x in numStrs)
                    {
                        try
                        {
                            int y = Convert.ToInt32(x);
                            double z = y * a[j] /3600;
                            Arr[i++] += z;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }                   
                }
            }

            //计算总帧数
            int sum=0;
            for (int k = 0; k < a.Length; k++)
                sum += a[k];

            FileStream fs = new FileStream("G://热图/热图/两半球/captrue1_total.xml", FileMode.Create);  //文件输出路径
            StreamWriter sw = new StreamWriter(fs);
            //输入xml信息
            sw.WriteLine("<?xml version=" + '"' + "1.0" + '"' + "?>");
            sw.WriteLine("<opencv_storage>");
            sw.WriteLine("<captrue1 type_id=" + '"' + "opencv-matrix" + '"' + ">");
            sw.WriteLine("  <rows>240</rows>");
            sw.WriteLine("  <cols>320</cols>");
            sw.WriteLine("  <total>"+sum+"</total>");
            sw.WriteLine("  <dt>f</dt>");
            sw.WriteLine("  <data>");
            //写入数据
            for (int k = 0; k < Arr.Length; k++)
            {
                //开始写入
                sw.Write(Arr[k]+".");               
            }
            //输入xml信息
            sw.WriteLine("</data></captrue1>");
            sw.Write("</opencv_storage>");
            //清空缓冲区
            sw.Flush(); 
            //关闭流
            sw.Close();
            fs.Close();               
        }
    }
}
