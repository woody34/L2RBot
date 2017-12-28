using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace L2RBot
{
    public class Xml
    {
        /// <summary>
        /// Returns the InnerText of and Element that has a specified attribte and value.
        /// </summary>
        /// 
        /// <param name="XmlFilePath">The path of the XML file.</param>
        /// 
        /// <param name="ElementTag">Element to search.</param>
        /// 
        /// <param name="Attribute">Attribute to be searched.</param>
        /// 
        /// <param name="Value">Specific attribute value.</param>
        /// 
        /// <returns>InnetText</returns>
        ///
        /// <example>
        /// string text = Xml.FindInnerTextByTagAttribute(@"C:\temp\file.xml", "string", "name", "LOCAL_PUSH_TARGET");
        /// </example>
        /// 
        /// The <paramref name = "XmlFilePath"> references an xml file with the below content:
        /// 
        /// <map>
        ///         <string name="LOCAL_PUSH_TARGET">JohnSmith</string>
        /// </map>
        /// 
        /// The example would return a string value of "JohnSmith".
        public static string FindInnerTextByTagAttribute(string XmlFilePath, string ElementTag, string Attribute, string Value)
        {
            //Create assign return object.
            string InnerText = "";

            //Create XML document object.
            XmlDocument doc = new XmlDocument();

            //Load document object with Xml data.
            doc.Load(XmlFilePath);

            //Create a NodeList of all of the 'ElementTags' in question.
            XmlNodeList XmlStrings = doc.GetElementsByTagName(ElementTag);

            //Iterate through each Node in the NodeList
            foreach (XmlNode o in XmlStrings)
            {
                //Iterate though each Node Elements Attribute
                foreach (XmlAttribute a in o.Attributes)
                {
                    //Find the desired 'Attribute.'
                    if (a.Name == Attribute)
                    {
                        //Find the desired 'Value.'
                        if (a.Value == Value)
                        {
                            //Grab the glorious InnerText.
                            InnerText = o.InnerText;
                        }
                    }
                }
            }

            //return glorious InnerText
            return InnerText;
        }
    }
}
