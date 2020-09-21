using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Task1.Models;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Catalog));
            Catalog catalog = new Catalog();

            using (var fileStream = File.OpenRead("..\\..\\books.xml"))
            {
                catalog = (Catalog)serializer.Deserialize(fileStream);
            }
       
            using (var xmlWriter = XmlWriter.Create("..\\..\\books1.xml", new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true
            }))
            {
                serializer.Serialize(xmlWriter, catalog);
            }
        }
    }
}
