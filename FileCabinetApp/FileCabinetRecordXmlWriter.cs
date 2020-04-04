using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Realizes a writing in the XML file.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Stream to write.</param>
        public FileCabinetRecordXmlWriter(StreamWriter writer)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineOnAttributes = true;
            settings.NewLineChars = "\r\n";

            this.writer = XmlWriter.Create(writer, settings);
        }

        /// <summary>
        /// Writes records.
        /// </summary>
        /// <param name="records">Records to write.</param>
        public void Write(IReadOnlyCollection<FileCabinetRecord> records)
        {
            this.writer.WriteStartElement("records");

            foreach (var r in records)
            {
                this.WriteOneRecord(r);
            }

            this.writer.WriteEndElement();

            this.writer.Close();
        }

        private void WriteOneRecord(FileCabinetRecord record)
        {
            this.writer.WriteStartElement("record");
            this.writer.WriteAttributeString("id", $"{record.Id}");

            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("first", $"{record.FirstName}");
            this.writer.WriteAttributeString("last", $"{record.LastName}");
            this.writer.WriteEndElement();

            this.writer.WriteElementString("sex", $"{record.Sex}");
            this.writer.WriteElementString("dateOfBirth", $"{record.DateOfBirth}");
            this.writer.WriteElementString("weight", $"{record.Weight}");
            this.writer.WriteElementString("balance", $"{record.Balance}");

            this.writer.WriteEndElement();
        }
    }
}
