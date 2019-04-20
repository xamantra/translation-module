using System;
using System.Windows.Forms;
using TranslationModule_v2.Models;

namespace TranslationModule_v2
{
    public static class Extensions
    {
        public static int ToInt32(this string str)
        {
            return Convert.ToInt32(str);
        }

        public static int ToInt32(this object obj)
        {
            return obj.ToString().ToInt32();
        }

        public static int ToInt(this bool tf)
        {
            return Convert.ToInt16(tf);
        }

        public static bool ToBoolean(this int num)
        {
            return Convert.ToBoolean(num);
        }

        public static string ToCapitalize(this string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
        }

        public static string UniqueID(this Form form)
        {
            return form.GetType().Namespace + "." + form.Name;
        }

        internal static string UniqueID(this ParentForm parentForm)
        {
            return parentForm.Namespace + "." + parentForm.Name;
        }

        internal static string UniqueID(this FormControl formControl)
        {
            return formControl.ParentFormID + "." + formControl.Name;
        }

        internal static string UniqueID(this Translation translation)
        {
            return translation.FormControlID + "." + translation.LanguageID;
        }
    }
}
