using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XMLLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            TextReader tr = new StringReader("<records><record id=\"1\"><headerInfo>header for 1</headerInfo><name>Record1</name></record><record id=\"1\"><headerInfo>header for 1</headerInfo><name>Record1.5</name></record><record id=\"2\"><headerInfo>header for 2</headerInfo><name>Record2</name></record></records>");
            XDocument xDoc = XDocument.Load(tr);

            Console.WriteLine("\n\n");
            // retrieve all records with the attribute id == "1" (should be 2 records returned)
            ShowForID(xDoc, "1");

            // group ALL records by the id value.  Should be 2 groups.  first group (id == "1") has 2 records.  the second (id == "2") has 1 record.
            ShowGrouped(xDoc);
        }

        static void ShowForID(XDocument xDoc, string id)
        {
            Console.WriteLine("Searching for records with ID: " + id);
            var returnItems = from i in xDoc.Element("records").Elements("record").AsEnumerable()
                where (string)i.Attribute("id") == id
                select i;
            
            foreach(var item in returnItems)
            {
                Console.WriteLine(item.Element("name").Value);
            }
            Console.WriteLine("\n--------------------------------------------\n\n");
        }

        static void ShowGrouped(XDocument xDoc)
        {
            Console.WriteLine("Showing Grouped");
            var returnItems = from i in xDoc.Element("records").Elements("record").AsEnumerable()
                group i by i.Attribute("id").Value into newGroup
                orderby newGroup.Key
                select newGroup;
            
            foreach(var item in returnItems)
            {
                // write the key out
                Console.WriteLine("Records for: " + item.Key);
                // write the header info for the grouped child items (get the first child record for that)
                Console.WriteLine(item.FirstOrDefault().Element("headerInfo").Value);

                // now go through each grouped child and write them out
                foreach(var itemRecord in item)
                {
                    Console.WriteLine(itemRecord.Element("name").Value);
                }
                Console.WriteLine("\n");
                
            }
            Console.WriteLine("\n--------------------------------------------\n\n");
        }
    }
}
