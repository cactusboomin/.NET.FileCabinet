using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Realizes a snapshot of records.
    /// </summary>
    public class FileCabinetRecordSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordSnapshot"/> class.
        /// </summary>
        /// <param name="records">Stream to write.</param>
        public FileCabinetRecordSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        /// <summary>
        /// Saves to the CSV file.
        /// </summary>
        /// <param name="writer">Stream to write.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(writer);

            csvWriter.Write(this.records);
        }

        /// <summary>
        /// Saves to the XML file.
        /// </summary>
        /// <param name="writer">Stream to write.</param>
        public void SaveToXml(StreamWriter writer)
        {
            var xmlWriter = new FileCabinetRecordXmlWriter(writer);

            xmlWriter.Write(this.records);
        }
    }
}
