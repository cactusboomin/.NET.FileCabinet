using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Realizes a writing in the CSV file.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Stream to write.</param>
        public FileCabinetRecordCsvWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Writes records.
        /// </summary>
        /// <param name="records">Records to write.</param>
        public void Write(IReadOnlyCollection<FileCabinetRecord> records)
        {
            var title = "ID;First name;Last name;Sex;Date of Birth;Weight;Balance";

            this.writer.Write(title);
            this.writer.Write(this.writer.NewLine);

            foreach (var r in records)
            {
                this.WriteOneRecord(r);
            }

            this.writer.Close();
        }

        private void WriteOneRecord(FileCabinetRecord record)
        {
            var recordToWrite =
                record.Id.ToString() + ";" +
                record.FirstName + ";" +
                record.LastName + ";" +
                record.Sex.ToString() + ";" +
                record.DateOfBirth.ToShortDateString() + ";" +
                record.Weight.ToString() + ";" +
                record.Balance.ToString();

            this.writer.Write(recordToWrite);
            this.writer.Write(this.writer.NewLine);
        }
    }
}
