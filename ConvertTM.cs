using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace FixTM
{
    class Program
    {
        static void ConvertDB2TMX
        {
            string data = @"C:\Users\Hello\Desktop\TEMP\test\testpart2.txt";

            Regex linerule = new Regex(@"^\[(.*?)\]\s+,(.*?)'[-0-9a-f]+', [-0-9]+,'(.*?)','(..-..)', ?[-0-9]+,'(.*?)','(..-..)', ?", RegexOptions.Compiled);


            StreamReader sr = new StreamReader(data, Encoding.UTF8, true);
            string line = string.Empty;

            string fname = string.Empty;
            string sourceLanguage = string.Empty;
            string sourceLangaugeText = string.Empty;
            string targetLanguage = string.Empty;
            string targetLangaugeText = string.Empty;

            string last_fname = string.Empty;
            string last_sourceLanguage = string.Empty;
            string last_targetLanguage = string.Empty;

            string fpath = Path.GetDirectoryName(data);

            StreamWriter sw;

            while (sr.Peek() > 0)
            {
                fpath = Path.GetDirectoryName(data);

                line = sr.ReadLine();
                Match m = linerule.Match(line);

                if (m != null)
                {
                    //new TMX
                    fname = m.Groups[1].Value;
                    sourceLanguage = m.Groups[4].Value;
                    sourceLangaugeText = m.Groups[3].Value;
                    targetLanguage = m.Groups[6].Value;
                    targetLangaugeText = m.Groups[5].Value;

                    sourceLangaugeText = fixTag(sourceLangaugeText);
                    targetLangaugeText = fixTag(targetLangaugeText);

                    fpath = fpath + "\\" + fname + "\\";
                    if (!Directory.Exists(fpath))
                    {
                        Directory.CreateDirectory(fpath);
                        Console.Write(".");
                    }
                    fpath = fpath + sourceLanguage + "_" + targetLanguage + ".tmx";

                    sw = new StreamWriter(fpath, true, Encoding.UTF8);
                    string TMTime = GetTime();
                    string T1 = "<tu creationdate=\"" + TMTime + "\" creationid=\"AutoTool\">\r\n<tuv xml:lang=\"" + sourceLanguage + "\">\r\n<seg>";
                    string T2 = "</seg>\r\n</tuv>\r\n<tuv xml:lang=\"" + targetLanguage + "\">\r\n<seg>";
                    string T3 = "</seg>\r\n</tuv>\r\n</tu>\r\n";

                    sw.Write(T1);
                    sw.Write(sourceLangaugeText);
                    sw.Write(T2);
                    sw.Write(targetLangaugeText);
                    sw.Write(T3);
                    sw.Close();
                    sr.Close();
                }
            }
        }

        private static string GetTime()
        {
            DateTime TimeNow = System.DateTime.Now;
            string TMDate = TimeNow.ToString("yyyyMMdd");
            string TMTime = TimeNow.ToString("hhmmss");
            return TMDate + "T" + TMTime + "Z";
        }

        private static string fixTag(string text)
        {
            Regex bpt = new Regex(@"<Tag><Type>Start</Type><Anchor>(\d+)</Anchor><AlignmentAnchor>\d+</AlignmentAnchor><TagID>.*?</TagID>.*?</Tag>", RegexOptions.Compiled);
            Regex ept = new Regex(@"<Tag><Type>End</Type><Anchor>(\d+)</Anchor><AlignmentAnchor>\d+</AlignmentAnchor>.*?</Tag>", RegexOptions.Compiled);
            Regex ph = new Regex(@"<Tag><Type>Standalone</Type><Anchor>(\d+)</Anchor><AlignmentAnchor>\d+</AlignmentAnchor><TagID>.*?</TagID>.*?</Tag>", RegexOptions.Compiled);

            Regex NonBreakingHyphen = new Regex(@"<Tag><Type>TextPlaceHolder</Type><Anchor>(\d+)</Anchor><AlignmentAnchor>\d+</AlignmentAnchor><TagID>NonBreakingHyphen</TagID>.*?</Tag>", RegexOptions.Compiled);
            Regex nbsp = new Regex(@"<Tag><Type>TextPlaceHolder</Type><Anchor>(\d+)</Anchor><AlignmentAnchor>\d+</AlignmentAnchor><TagID>.*?</TagID><TextEquivalent>nbsp</TextEquivalent>", RegexOptions.Compiled);

            Regex LockedContent = new Regex(@"<Tag><Type>LockedContent</Type><Anchor>(\d+)</Anchor><AlignmentAnchor>\d+</AlignmentAnchor><TagID>.*?</TagID><TextEquivalent>(.*?)</TextEquivalent>.*?</Tag>", RegexOptions.Compiled);

            text = bpt.Replace(text, @"<bpt i=""$1"" x=""$1""/>");
            text = ept.Replace(text, @"<ept i=""$1""/>");
            text = ph.Replace(text, @"<ph x=""$1"" />");
            text = NonBreakingHyphen.Replace(text, @"-");
            text = nbsp.Replace(text, @" ");
            text = LockedContent.Replace(text, @"<bpt i=""$1"" type=""x - LockedContent1"" x=""$1"" />$2<ept i=""$1""/>");

            text = text.Replace("<Text><Value>", "");
            text = text.Replace("</Value></Text>", "");
            text = text.Replace("<Elements>", "");
            text = text.Replace("</Elements>", "");
            text = text.Replace("<CultureName>", "");
            text = text.Replace("</CultureName>", "");

            return text;
        }
    }
}
