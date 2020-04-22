using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Realizes a snapshot of records.
    /// </summary>
    public class FileCabinetGeneratorSnapshot
    {
        private IReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetGeneratorSnapshot"/> class.
        /// </summary>
        /// <param name="records">Stream to write.</param>
        public FileCabinetGeneratorSnapshot(IReadOnlyCollection<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>
        /// Saves to the CSV file.
        /// </summary>
        /// <param name="writer">Stream to write.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            var csvWriter = new FileCabinetGeneratorCsv(writer);

            csvWriter.Write(this.records);
        }

        /// <summary>
        /// Saves to the XML file.
        /// </summary>
        /// <param name="writer">Stream to write.</param>
        public void SaveToXml(StreamWriter writer)
        {
            var xmlWriter = new FileCabinetGeneratorXml(writer);

            xmlWriter.Write(this.records);
        }
    }
}
