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
        // TODO: Replace these with your actual GitHub Pages URLs
        public static string xmlURL = "https://YOUR_GITHUB_USERNAME.github.io/YOUR_REPO_NAME/NationalParks.xml";
        public static string xmlErrorURL = "https://YOUR_GITHUB_USERNAME.github.io/YOUR_REPO_NAME/NationalParksErrors.xml";
        public static string xsdURL = "https://YOUR_GITHUB_USERNAME.github.io/YOUR_REPO_NAME/NationalParks.xsd";

        public static void Main(string[] args)
        {
            // Q3.1: Verify XML without errors
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);
            Console.WriteLine();

            // Q3.2: Verify XML with errors
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);
            Console.WriteLine();

            // Q3.3: Convert XML to JSON
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1: XML Validation against XSD
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                // Download XML and XSD content from URLs
                string xmlContent = DownloadContent(xmlUrl);
                string xsdContent = DownloadContent(xsdUrl);

                // Create XmlReaderSettings with validation
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                
                // Add XSD schema
                using (StringReader xsdReader = new StringReader(xsdContent))
                {
                    XmlSchema schema = XmlSchema.Read(xsdReader, null);
                    settings.Schemas.Add(schema);
                }

                // Set validation event handler to capture errors
                string validationError = "";
                settings.ValidationEventHandler += (sender, e) =>
                {
                    if (e.Severity == XmlSeverityType.Error)
                    {
                        validationError += e.Message + Environment.NewLine;
                    }
                };

                // Validate XML against schema
                using (StringReader xmlReader = new StringReader(xmlContent))
                {
                    using (XmlReader reader = XmlReader.Create(xmlReader, settings))
                    {
                        while (reader.Read()) { }
                    }
                }

                // Return result
                if (string.IsNullOrEmpty(validationError))
                {
                    return "No errors are found";
                }
                else
                {
                    return validationError.Trim();
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        // Q2.2: XML to JSON Conversion
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                // Download XML content from URL
                string xmlContent = DownloadContent(xmlUrl);

                // Load XML document
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                // Convert XML to JSON
                string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

                return jsonText;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
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
