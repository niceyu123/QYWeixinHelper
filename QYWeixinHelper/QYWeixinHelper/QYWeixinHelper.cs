﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;

namespace QYWeixinHelper
{
    class QYWeixinHelper
    {
        static string corpid = System.Configuration.ConfigurationManager.AppSettings["corpid"].ToString();
        static string corpsecret = System.Configuration.ConfigurationManager.AppSettings["secret"].ToString();
        static string messageSendURl = System.Configuration.ConfigurationManager.AppSettings["messageSendURl"].ToString();

        /// <summary>
        /// 获取企业号的accessToken
        /// </summary>
        /// <param name="corpid">企业号ID</param>
        /// <param name="corpsecret">管理组密钥</param>
        /// <returns></returns>
        static string GetQYAccessToken(string corpid, string corpsecret)
        {
            string getAccessTokenUrl = System.Configuration.ConfigurationManager.AppSettings["getAccessTokenUrl"].ToString();
            string accessToken = "";

            string respText = "";

            //获取josn数据
            //string url = string.Format(getAccessTokenUrl, corpid, corpsecret);

            string url = getAccessTokenUrl;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.Default);
                respText = reader.ReadToEnd();
                resStream.Close();
            }

            try
            {
                JavaScriptSerializer Jss = new JavaScriptSerializer();
                Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
                //通过键access_token获取值
                accessToken = respDic["access_token"].ToString();
            }
            catch (Exception ex)
            {

            }
            return accessToken;
        }

        /// <summary>
        /// Post数据接口
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="paramData">提交json数据</param>
        /// <param name="dataEncode">编码方式</param>
        /// <returns></returns>
        static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 推送信息
        /// </summary>
        /// <param name="corpid">企业号ID</param>
        /// <param name="corpsecret">管理组密钥</param>
        /// <param name="paramData">提交的数据json</param>
        /// <param name="dataEncode">编码方式</param>
        /// <returns></returns>
        public static void SendText(string empCode, string message)
        {
            string accessToken = "";
            string postUrl = "";
            string param = "";
            string postResult = "";

            accessToken = GetQYAccessToken(corpid, corpsecret);
            postUrl = string.Format(messageSendURl, accessToken);
            CorpSendText paramData = new CorpSendText(message);
            foreach (string item in empCode.Split('|'))
            {
                //paramData.touser = GetOAUserId(item);//在实际应用中需要判断接收消息的成员是否在系统账号中存在。
                paramData.touser = item;
                param = JsonConvert.SerializeObject(paramData);
                if (paramData.touser != null)
                {
                    postResult = PostWebRequest(postUrl, param, Encoding.UTF8);
                }
                else
                {
                    postResult = "账号" + paramData.touser + "在OA中不存在!";
                }
                //CreateLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + ":\t" + item + "\t" + param + "\t" + postResult);
            }
        }

        //private static void CreateLog(string strlog)
        //{
        //    string str1 = "QYWeixin_log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        //    //BS CS应用日志自适应
        //    string path = System.Web.HttpContext.Current == null ? Path.GetFullPath("..") + "\\temp\\" : System.Web.HttpContext.Current.Server.MapPath("temp");
        //    try
        //    {
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        path = Path.Combine(path, str1);
        //        StreamWriter sw = File.AppendText(path);
        //        sw.WriteLine(strlog);
        //        sw.Flush();
        //        sw.Close();

        //    }
        //    catch
        //    {
        //    }
        //}
    }
}
