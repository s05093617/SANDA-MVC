using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Music_Station0730.Extentions;
using Music_Station0730.Models;

namespace Music_Station0730.Helpers
{
    /// <summary>
    /// 一般控制項 HtmlHelper
    /// </summary>
    public static class ControlsHelper
    {

        public static IHtmlString InputSearchBox(this HtmlHelper helper, string name, object value, object htmlAttributes = null)
        {
            return new HtmlString(string.Format("<input id=\"{0}\" name=\"{0}\" type=\"search\" value=\"{1}\" {2}/>", name, value != null ? value.ToString() : "", htmlAttributes.ToAttributeList()));
        }



        /// <summary>
        /// CheckBox(值為 true or false 二選一)
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="id">checkbox id</param>
        /// <param name="isChecked"></param>
        /// <param name="text">label text</param>
        /// <param name="htmlAttributes">checkbox attr</param>
        public static IHtmlString CheckBoxBit(this HtmlHelper helper, string id, string text, bool isChecked, object htmlAttributes = null)
        {
            string format = "<input id=\"{0}\" name=\"{0}\" value=\"true\" {1} type=\"checkbox\" {3}/>{2}";

            var setHash = htmlAttributes.ToAttributeList();

            string attributeList = string.Empty;
            if (setHash != null)
                attributeList = setHash;

            if (attributeList.IndexOf("disabled=\"disabled\"") == -1)
            {
                format += "<input name=\"{0}\" value=\"false\" type=\"hidden\" />";
            }
            else if (isChecked)
            {
                format += "<input name=\"{0}\" value=\"true\" type=\"hidden\" />";
            }

            return new HtmlString(string.Format(format,
                id,
                isChecked ? "checked=\"checked\"" : "",
                string.IsNullOrEmpty(text) ? "" : string.Format("<label id=\"{0}\" for=\"{1}\">{2}</label>", id + "_label", id, text),
                attributeList));
        }

        /// <summary>
        /// 擴展 checkbox 
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="id">checkbox id</param>
        /// <param name="isChecked"></param>
        /// <param name="value">checkbox value</param>
        /// <param name="text">label text</param>
        /// <param name="htmlAttributes">checkbox attr</param>
        public static IHtmlString CheckBoxLabel(this HtmlHelper helper, string id, string value, string text, bool isChecked, object htmlAttributes = null)
        {
            var setHash = htmlAttributes.ToAttributeList();

            string attributeList = string.Empty;
            if (setHash != null)
                attributeList = setHash;

            return new HtmlString(string.Format("<input id=\"{0}\" name=\"{1}\" value=\"{2}\" {3} type=\"checkbox\" {5}/>{4}",
                id,
                id,
                value,
                isChecked ? "checked=\"checked\"" : "",
                string.IsNullOrEmpty(text) ? "" : string.Format("<label id=\"{0}\" for=\"{1}\">{2}</label>", id + "_label", id, text),
                attributeList));
        }
        public static string setDateFormat(DateTime? date, bool b = false)
        {
            if (date == null) return "";
            if (b) return Convert.ToDateTime(date).ToString("yyyy/MM/dd HH:mm");
            return Convert.ToDateTime(date).ToString("yyyy/MM/dd");
        }

        public static string setDateFormatYM(int? date)
        {
            if (date == null) return "";
            String yyyy = date.ToString().Substring(0, 4);
            String MM = date.ToString().Substring(4, 2);
            return yyyy + "/" + MM;
        }

        //取得CourseCategories 課程類別屬於第二層的資料並產生DropDownList
        //參數一：_db、參數二:名稱(DropDownList的id以及name)、參數三：課程類別Id(預設值)、參數四：N(選單第一個值)/Y(預設"請選擇")
        //public static string GetCourseCategoryList(BackendContext _db, string Name, int selectId, string choice)
        //{

        //    var courseCategories = _db.CourseCategories.Where(x => x.ParentId == null).ToList();
        //    StringBuilder courseCategoriesselectstr = new StringBuilder();
        //    courseCategoriesselectstr.Append("<select id=\"" + Name + "\" name=\"" + Name + "\" class='rwd-select'  >");
        //    if (choice == "Y")
        //    {
        //        courseCategoriesselectstr.Append("<option>請選擇</option>");
        //    }

        //    foreach (var item in courseCategories.OrderBy(x => x.Subject))
        //    {
        //        courseCategoriesselectstr.Append("<optgroup label = \"" + item.Subject + "\" >");

        //        foreach (var items in item.ChildCourseCategory)
        //        {
        //            if (selectId == items.Id)
        //            {
        //                courseCategoriesselectstr.Append("<option selected value=\"" + items.Id + "\">" + items.Subject + "</option>");
        //            }
        //            else
        //            {
        //                courseCategoriesselectstr.Append("<option value=\"" + items.Id + "\">" + items.Subject + "</option>");
        //            }

        //        }
        //        courseCategoriesselectstr.Append("</optgroup>");
        //    }
        //    courseCategoriesselectstr.Append("</select>");
        //    return courseCategoriesselectstr.ToString();
        //}

        ////取得CourseCategories 課程類別屬於第三層的資料並產生DropDownList(過濾IsUsing當前版本為是、IsWorking是否啟用為啟用、以及CourseCategory.Working是否啟用為啟用)
        //public static string GetCourseList(BackendContext _db)
        //{

        //    var course = _db.Courses.Where(x => x.IsUsing == NewBooleanType.是&&x.IsWorking==Working.是&&x.CourseCategory.Working==Working.是).ToList();
        //    StringBuilder selectstr = new StringBuilder();
        //    selectstr.Append("<select id=\"CourseId\" name=\"Courselists\" class=\"Courselists\" >");
        //    string coursename = "";
        //    int i = 0;
        //    foreach (var item in course.OrderBy(x => x.CourseCategoryId))
        //    {

        //        if (item.CourseCategory.ParentId.HasValue)
        //        {
        //            if (coursename != item.CourseCategory.ParentCourseCategory.Subject)
        //            {
        //                if (i != 0)
        //                {
        //                    selectstr.Append("</optgroup>");
        //                }

        //                selectstr.Append("<optgroup label = \"" + item.CourseCategory.ParentCourseCategory.Subject +
        //                                 "\" >");
        //                coursename = item.CourseCategory.ParentCourseCategory.Subject;
        //            }
        //        }
        //        else
        //        {
        //            if (coursename != item.CourseCategory.Subject)
        //            {
        //                if (i != 0)
        //                {
        //                    selectstr.Append("</optgroup>");
        //                }

        //                selectstr.Append("<optgroup label = \"" + item.CourseCategory.Subject + "\" >");
        //                coursename = item.CourseCategory.Subject;
        //            }
        //        }

        //        selectstr.Append("<option value=\"" + item.Id + "\">" + item.CourseCategory.Subject.Trim() +
        //                         "</option>");
        //        i++;
        //    }
        //    selectstr.Append("</select>");
        //   return selectstr.ToString();
        //}
    }
}
