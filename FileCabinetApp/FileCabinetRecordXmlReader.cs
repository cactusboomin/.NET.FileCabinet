using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Realizes reading from XML-file.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">Stream to read.</param>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all records.
        /// </summary>
        /// <returns>Records.</returns>
        public List<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();
            var deserializer = new XmlSerializer(records.GetType());

            records = (List<FileCabinetRecord>)deserializer.Deserialize(this.reader);

            return records;
        }
    }
}
