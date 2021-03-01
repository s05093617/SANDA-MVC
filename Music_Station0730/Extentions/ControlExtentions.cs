using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace Music_Station0730.Extentions
{
    /// <summary>
    /// 控制項擴充函式庫
    /// </summary>
    public static class ControlExtentions
    {
        /// <summary>
        /// 以 ',' 分隔後立即轉換成整數清單
        /// </summary>
        public static List<int> SplitToInt(this string str)
        {
            return (str ?? "").Split(',').Where(i => i.Trim().Length > 0).Select(i => int.Parse(i)).ToList();
        }

        /// <summary>
        /// (擴充)以斷行符號進行字串分割並傳回 List 型別
        /// </summary>
        public static List<string> SplitByBreakLineSimbo(this string str)
        {
            return str.Split('\n').Select(i => i.Trim()).Where(i => i.Trim().Length > 0).ToList();
        }

        /// <summary>
        /// 自訂擴充功能(將該 List 以 ',' 組合成字串)
        /// </summary>
        public static string ToArrayString(this IList list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var i in list)
            {
                sb.Append(i);
                sb.Append(",");
            }
            if (sb.Length > 0)
            {
                return sb.ToString(0, sb.Length - 1);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetPropertyHash(object properties)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            if (properties != null)
            {
                PropertyDescriptorCollection props =
                    TypeDescriptor.GetProperties(properties);

                foreach (PropertyDescriptor prop in props)
                {
                    values.Add(prop.Name, prop.GetValue(properties));
                }
            }
            return values;
        }

        /// <summary>
        /// Creates a simple {0}='{1}' list based on current object state.
        /// </summary>
        public static string ToAttributeList(this object o)
        {
            IDictionary<string, object> dics = null;
            if (o is IDictionary<string, object>)
            {
                dics = (IDictionary<string, object>)o;
            }
            else if (o != null)
            {
                dics = GetPropertyHash(o);
            }

            StringBuilder sb = new StringBuilder();
            string resultFormat = " {0}=\"{1}\"";
            if (dics != null)
            {
                foreach (string key in dics.Keys)
                {
                    if (dics[key] != null)
                    {
                        sb.AppendFormat(resultFormat, key.Replace("_", ""), dics[key]);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a simple {0}='{1}' list based on current object state. Ignores the passed-in string[] items
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ignoreList"></param>
        /// <returns></returns>
        public static string ToAttributeList(this object o, params object[] ignoreList)
        {
            Dictionary<string, object> attributeHash = GetPropertyHash(o);

            string resultFormat = "{0}=\"{1}\" ";
            StringBuilder sb = new StringBuilder();
            foreach (string attribute in attributeHash.Keys)
            {
                if (!ignoreList.Contains(attribute))
                {
                    sb.AppendFormat(resultFormat, attribute, attributeHash[attribute]);
                }
            }
            return sb.ToString();

        }
    }
}
