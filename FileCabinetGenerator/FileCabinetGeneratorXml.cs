using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Realizes a writing in the XML file.
    /// </summary>
    public class FileCabinetGeneratorXml
    {
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetGeneratorXml"/> class.
        /// </summary>
        /// <param name="writer">Stream to write.</param>
        public FileCabinetGeneratorXml(StreamWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Writes records.
        /// </summary>
        /// <param name="records">Records to write.</param>
        public void Write(IReadOnlyCollection<FileCabinetRecord> records)
        {
            var serializer = new XmlSerializer(typeof(FileCabinetRecord));

            foreach (var r in records)
            {
                serializer.Serialize(this.writer, r);
            }
        }
    }
}
