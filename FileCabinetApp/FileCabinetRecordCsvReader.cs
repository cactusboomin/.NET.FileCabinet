using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace FileCabinetApp
{
    /// <summary>
    /// Implements reading records from CSV-file.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">Stream for reading.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all records.
        /// </summary>
        /// <returns>Records.</returns>
        public List<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> result;

            using (CsvReader csvReader = new CsvReader(this.reader, CultureInfo.CurrentCulture, true))
            {
                csvReader.Configuration.Delimiter = ";";

                result = new List<FileCabinetRecord>(csvReader.GetRecords<FileCabinetRecord>());
            }

            return result;
        }
    }
}
