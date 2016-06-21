using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClippingManager
{
    class _unusedCode
    {

        //MainWindow line 106: Original code for creating dict, replaced for a more precise system locating criticalWords and criticalWordsPosition
        //Both arrays an both in order, 0 in one goes to 0 in the other and so on (paired). 

        /*
         if (formatLanguagePack != null)
                    {
                        foreach (MyClippingsParser.FormatType formatInstance in formatLanguagePack)
                        {
                            foreach (FieldInfo field in formatInstance.GetType().GetFields())
                            {
                                if ((field.Name.ToString().Contains("pageWording")) || (field.Name == "locationWording") || 
                                    (field.Name == "criticalWords"))
                                {
                                    string keywordType = field.FieldType.Name;
                                    var keyword = field.GetValue(formatInstance); //Remember, GetValue() returns an object containing the value of the field. 

                                    if (keywordType == "String") //Note that keywords are detected using strings from Name. Other option is to use field.GetType(), but type casting and derived problems were occuring. 
                                    {
                                        engKeywords.Add(keyword.ToString());
                                        formatList.Add(new KeyValuePair<string, MyClippingsParser.FormatType>(keyword.ToString(), formatInstance));
                                    }

                                    if (keywordType == "String[]")
                                    {
                                        foreach (var str in (string[])keyword)
                                        {
                                            engKeywords.Add(str);
                                            formatList.Add(new KeyValuePair<string, MyClippingsParser.FormatType>(str, formatInstance));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    */
    }
}
