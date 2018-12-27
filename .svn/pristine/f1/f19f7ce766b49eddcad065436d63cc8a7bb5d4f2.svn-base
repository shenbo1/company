using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace Company.WX
{
    public class WXData
    {
        private SortedDictionary<string, object> Values = new SortedDictionary<string, object>();
        private string Key = string.Empty;
        public WXData() 
        {
           
        }
        public void SetValue(string key, object value)
        {
            Values[key] = value;
        }

        public object GetValue(string key)
        {
            object o = null;
            Values.TryGetValue(key, out o);
            return o;
        }

        public bool IsSet(string key)
        {
            object o = null;
            Values.TryGetValue(key, out o);
            return null != o;
        }
        public string ToUrl()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in Values)
            {
                 if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }

        public SortedDictionary<string, object> GetValues()
        {
            return Values;
        }

        public bool CheckSign()
        {
            string returnSign = GetValue("sign").ToString();

            //在本地计算新的签名
            string calSign = MakeSign();
            return calSign == returnSign;
        }

        public string MakeSign()
        {
            //转url格式
            string str = ToUrl();
            //在string后加入API KEY
            str += "&key=" + Key;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        public string ToXml()
        {          
            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in Values)
            {
                //字段值不能为null，会影响后续流程

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                  
                }
            }
            xml += "</xml>";
            return xml;
        }

        public SortedDictionary<string, object> FromXml(string xml)
        {            
            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(xml);
            XmlNode _xmlNode = _xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList _nodes = _xmlNode.ChildNodes;
            foreach (XmlNode xn in _nodes)
            {
                XmlElement xe = (XmlElement)xn;
                Values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }
            try
            {
                //2015-06-29 错误是没有签名
                if (Values.ContainsKey("return_code"))
                {
                    if (Values["return_code"] != "SUCCESS")
                    {
                        return Values;
                    }
                }
                CheckSign();//验证签名,不通过会抛异常
            }
            catch (Exception _ex)
            {
                //   throw new WxPayException(ex.Message);
            }

            return Values;
        }
        public string ToJson()
        {
            string jsonStr = JsonConvert.SerializeObject(Values);
            return jsonStr;
        }
        public T FromJson<T>(string json ) 
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


    }
}
