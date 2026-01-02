using System;
using UnityEngine;

namespace PupilFramework
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        public static bool isChineseVersion;

        public LanguageManager()
        {
            ReadLanguage();
        }

        public static bool ReadLanguage()
        {
            string languageStr = Application.systemLanguage.ToString();
            if (String.Compare(languageStr, "ChineseSimplified", StringComparison.Ordinal) == 0
                || String.Compare(languageStr, "ChineseTraditional", StringComparison.Ordinal) == 0
                || String.Compare(languageStr, "Chinese", StringComparison.Ordinal) == 0)
            {
                isChineseVersion = true;
            }
            else
            {
                isChineseVersion = false;
            }
            return isChineseVersion;
        }

        public static string GetPushText(int text)
        {
            return text.ToString();
        }
    }
}
