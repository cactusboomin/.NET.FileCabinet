using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using static FileCabinetApp.Literals;

namespace FileCabinetApp
{
    /// <summary>
    /// Contains a logic for file cabinet.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private FileStream stream;
        private bool disposed = false;
        private int lastId = 0;

        /// <summary>
        /// Gets realization of the validator.
        /// </summary>
        /// <value>
        /// Realization of the validator.
        /// </value>
        public IRecordValidator Validator { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public FileCabinetFilesystemService(IRecordValidator validator)
        {
            this.Validator = validator;
            this.stream = new FileStream("cabinet-records.db", FileMode.Create);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// Disposes a file stream.
        /// </summary>
        ~FileCabinetFilesystemService()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Disposes a file cabinet system.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="record">A record that contains information about the new record.</param>
        /// <returns>An ID of the new record.</returns>
        public int CreateRecord(FileCabinetRecordWithoutID record)
        {
            if (record == null)
            {
                throw new ArgumentNullException($"{nameof(record)} can't be null.");
            }

            int id;

            using (var writer = new BinaryWriter(this.stream, Encoding.Unicode, true))
            {
                id = ++this.lastId;

                writer.Seek(SizeOfShort, SeekOrigin.End);

                writer.Write(id);

                var firstName = new char[60];
                for (int i = 0; i < record.FirstName.Length && i < MaxLengthForFirstNameDefault; i++)
                {
                    firstName[i] = record.FirstName[i];
                }

                writer.Write(firstName);

                var lastName = new char[60];
                for (int i = 0; i < record.LastName.Length && i < MaxLengthForLastNameDefault; i++)
                {
                    lastName[i] = record.LastName[i];
                }

                writer.Write(lastName);

                writer.Write(record.Sex);
                writer.Write(record.DateOfBirth.Day);
                writer.Write(record.DateOfBirth.Month);
                writer.Write(record.DateOfBirth.Year);
                writer.Write(record.Weight);
                writer.Write(record.Balance);
            }

            return id;
        }

        /// <summary>
        /// Edits a record.
        /// </summary>
        /// <param name="id">An ID of the editable record.</param>
        /// <param name="changedRecord">Changed record.</param>
        public void EditRecord(int id, FileCabinetRecordWithoutID changedRecord)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds a record by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth of records.</param>
        /// <returns>An array of records with certain date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds a record by first name.
        /// </summary>
        /// <param name="firstName">First name of records.</param>
        /// <returns>An array of records with certain first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds a record by last name.
        /// </summary>
        /// <param name="lastName">Last name of records.</param>
        /// <returns>An array of records with certain last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>An array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a count of records.
        /// </summary>
        /// <returns>A count of records.</returns>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Makes a snapshot of file cabinet records.
        /// </summary>
        /// <returns>Snapshot of file cabinet records.</returns>
        public FileCabinetRecordSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disposes a file cabinet system.
        /// </summary>
        /// <param name="disposing">True if you need to clear managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.stream.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
