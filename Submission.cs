using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://brian-vuong05.github.io/CSE445Assignment4/NationalParks.xml";
        public static string xmlErrorURL = "https://brian-vuong05.github.io/CSE445Assignment4/NationalParksErrors.xml";
        public static string xsdURL = "https://brian-vuong05.github.io/CSE445Assignment4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            //return "No errors are found" if XML is valid. Otherwise, return the desired exception message.
            try
            {
                string xmlContent = DownloadContent(xmlUrl);
                string xsdContent = DownloadContent(xsdUrl);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                
                using (StringReader sr = new StringReader(xsdContent))
                {
                    XmlSchema schema = XmlSchema.Read(sr, null);
                    settings.Schemas.Add(schema);
                }

                string error = "";
                settings.ValidationEventHandler += (s, e) =>
                {
                    if (e.Severity == XmlSeverityType.Error)
                    {
                        error += e.Message + "\n";
                    }
                };

                using (StringReader sr = new StringReader(xmlContent))
                {
                    using (XmlReader reader = XmlReader.Create(sr, settings))
                    {
                        while (reader.Read()) { }
                    }
                }

                if (string.IsNullOrEmpty(error))
                {
                    return "No errors are found";
                }
                else
                {
                    return error.Trim();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            try
            {
                string xmlContent = DownloadContent(xmlUrl);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                
                string json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
                return json;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }

}
