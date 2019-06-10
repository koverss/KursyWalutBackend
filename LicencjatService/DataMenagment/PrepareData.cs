using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace KursyWalutService.DataMenagment
{
    public static class PrepareData
    {
        public static List<string> selectedCurrencies_Names = new List<string>();
        public static List<string> selectedCurrencies_Values = new List<string>();
        public static DateTime updateDate;

        public static XmlDocument FormatXMLtoUTF8(string url)
        {
            var xml_table = new XmlDocument();
            try
            {
                xml_table.Load(url);
            }
            catch(Exception ex)
            {
                throw ex;
            }            

            XmlDeclaration xmlDeclaration = null;
            xmlDeclaration = xml_table.CreateXmlDeclaration("1.0", null, null);
            xmlDeclaration.Encoding = "utf-8";
            xml_table.ReplaceChild(xmlDeclaration, xml_table.FirstChild);

            Encoding iso = Encoding.GetEncoding(28592);
            Encoding utf8 = Encoding.UTF8;

            byte[] bytes = iso.GetBytes(xml_table.OuterXml); //xml_table.OuterXml

            byte[] convertISO = Encoding.Convert(iso, utf8, bytes);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Encoding.UTF8.GetString(convertISO));

            return xmlDoc;
        }

        public static DateTime GetDocumentDate(string url)
        {
            var doc = new XmlDocument();

            try
            {
                doc.Load(url);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            XmlNodeList dateNode = doc.SelectNodes("/tabela_kursow");

            string temp;
            XmlNode node = doc.SelectSingleNode("/tabela_kursow/data_publikacji");
            temp = node.InnerText;
            updateDate = Convert.ToDateTime(temp);
            return updateDate;
        }
    }
}