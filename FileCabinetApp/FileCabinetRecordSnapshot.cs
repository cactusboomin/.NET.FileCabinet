using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Realizes a snapshot of records.
    /// </summary>
    public class FileCabinetRecordSnapshot
    {
        private List<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordSnapshot"/> class.
        /// </summary>
        /// <param name="records">Stream to write.</param>
        public FileCabinetRecordSnapshot(List<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>
        /// Gets records.
        /// </summary>
        /// <returns>Records.</returns>
        public List<FileCabinetRecord> GetRecords()
        {
            return this.records;
        }

        /// <summary>
        /// Loads from the CSV file.
        /// </summary>
        /// <param name="reader">Stream to read.</param>
        /// <returns>Count of records.</returns>
        public int LoadFromCsv(StreamReader reader)
        {
            var csvReader = new FileCabinetRecordCsvReader(reader);

            this.records = csvReader.ReadAll();

            return this.records.Count;
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
        /// Loads record from XML-file.
        /// </summary>
        /// <param name="reader">Stream to read.</param>
        /// <returns>Count of records.</returns>
        public int LoadFromXml(StreamReader reader)
        {
            var xmlReader = new FileCabinetRecordXmlReader(reader);

            this.records = xmlReader.ReadAll();

            return this.records.Count;
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
