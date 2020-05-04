using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace AddStructure
{
    class Program
    {
        static void Main(string[] args)
        {
            string fpath = @"C:\Users\Hello\Desktop\TEMP\test\Out1";
            //string fpath = Console.ReadLine();

            string[] files = Directory.GetFiles(fpath, "*.tmx", SearchOption.AllDirectories);
            foreach (string f in files)
            {
                StreamReader sr = new StreamReader(f, Encoding.UTF8, true);
                StreamWriter sw = new StreamWriter(f + "2", false, Encoding.UTF8);

                string sourceLang = Path.GetFileNameWithoutExtension(f).Substring(0, 5);

                sw.WriteLine("<?xml version=\"1.0\" ?>\r\n<tmx version=\"1.4b\">\r\n<header\r\n\tcreationtool=\"TRADOS " +
                    "Translator's Workbench for Windows\"\r\n\tcreationtoolversion=\"Edition 7 Build " +
                    "756\"\r\n\tsegtype=\"sentence\"\r\n\to-tmf=\"TW4Win 2.0 Format\"\r\n\tadminlang=\"en-US\"\r\n\tsrclang=\"" 
                    + sourceLang +
                    "\"\r\n\tdatatype=\"rtf\"\r\n\tcreationdate=\"" + GetTime() + 
                    "\"\r\n\tcreationid=\"LocalizationTools\"\r\n>\r\n</header>\r\n\r\n<body>\r\n");

                while (sr.Peek() > -1)
                {
                    sw.WriteLine(sr.ReadLine());
                }

                sw.WriteLine("\r\n</body>\r\n</tmx>");

                sw.Close();
                sr.Close();
            }

        }

        private static string GetTime()
        {
            DateTime TimeNow = System.DateTime.Now;
            string TMDate = TimeNow.ToString("yyyyMMdd");
            string TMTime = TimeNow.ToString("hhmmss");
            return TMDate + "T" + TMTime + "Z";
        }
    }
}
