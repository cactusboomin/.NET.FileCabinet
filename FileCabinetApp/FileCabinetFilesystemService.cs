﻿using System;
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

                this.stream.Seek(SizeOfShort, SeekOrigin.End);

                this.WriteRecordWithoutID(writer, id, record);

                this.stream.Seek(0, SeekOrigin.Begin);
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
            using (var reader = new BinaryReader(this.stream, Encoding.Unicode, true))
            {
                this.stream.Seek(SizeOfShort, SeekOrigin.Begin);
                int foundId = 0;

                while (reader.PeekChar() > -1)
                {
                    foundId = reader.ReadInt32();

                    if (foundId == id)
                    {
                        this.stream.Seek(-SizeOfInt, SeekOrigin.Current);
                        break;
                    }

                    this.stream.Seek(SizeOfRecord - SizeOfInt, SeekOrigin.Current);
                }

                if (foundId == 0)
                {
                    throw new ArgumentException($"A record with ID {id} was not found.");
                }
            }

            using (var writer = new BinaryWriter(this.stream, Encoding.Unicode, true))
            {
                this.WriteRecordWithoutID(writer, id, changedRecord);
                this.stream.Seek(0, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Finds a record by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth of records.</param>
        /// <returns>An array of records with certain date of birth.</returns>
        public List<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            var result = new List<FileCabinetRecord>();

            using (var reader = new BinaryReader(this.stream, Encoding.Unicode, true))
            {
                this.stream.Seek(0, SeekOrigin.Begin);

                while (reader.PeekChar() > -1)
                {
                    this.stream.Seek(
                        SizeOfShort +
                        SizeOfInt +
                        (SizeOfChar * MaxLengthForFirstNameDefault * 2) +
                        SizeOfChar,
                        SeekOrigin.Current);

                    int day = reader.ReadInt32();
                    int month = reader.ReadInt32();
                    int year = reader.ReadInt32();

                    var foundDate = new DateTime(year, month, day);

                    if (foundDate.Equals(dateOfBirth))
                    {
                        this.stream.Seek(-(SizeOfRecord - SizeOfShort - SizeOfDecimal), SeekOrigin.Current);

                        result.Add(this.ReadRecord(reader));
                    }
                    else
                    {
                        this.stream.Seek(SizeOfShort + SizeOfDecimal, SeekOrigin.Current);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Finds a record by first name.
        /// </summary>
        /// <param name="firstName">First name of records.</param>
        /// <returns>An array of records with certain first name.</returns>
        public List<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var result = new List<FileCabinetRecord>();

            using (var reader = new BinaryReader(this.stream, Encoding.Unicode, true))
            {
                this.stream.Seek(0, SeekOrigin.Begin);

                while (reader.PeekChar() > -1)
                {
                    this.stream.Seek(
                        SizeOfShort +
                        SizeOfInt,
                        SeekOrigin.Current);

                    var foundName = new string(reader.ReadChars(MaxLengthForFirstNameDefault)).Trim('\0');

                    if (foundName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.stream.Seek(-(SizeOfShort + SizeOfInt + (MaxLengthForFirstNameDefault * SizeOfChar)), SeekOrigin.Current);

                        result.Add(this.ReadRecord(reader));
                    }
                    else
                    {
                        this.stream.Seek(SizeOfRecord - (SizeOfShort + SizeOfInt + (MaxLengthForFirstNameDefault * SizeOfChar)), SeekOrigin.Current);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Finds a record by last name.
        /// </summary>
        /// <param name="lastName">Last name of records.</param>
        /// <returns>An array of records with certain last name.</returns>
        public List<FileCabinetRecord> FindByLastName(string lastName)
        {
            var result = new List<FileCabinetRecord>();

            using (var reader = new BinaryReader(this.stream, Encoding.Unicode, true))
            {
                this.stream.Seek(0, SeekOrigin.Begin);

                while (reader.PeekChar() > -1)
                {
                    this.stream.Seek(
                        SizeOfShort +
                        SizeOfInt +
                        (MaxLengthForFirstNameDefault * SizeOfChar),
                        SeekOrigin.Current);

                    var foundName = new string(reader.ReadChars(MaxLengthForLastNameDefault)).Trim('\0');

                    if (foundName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.stream.Seek(-(SizeOfShort + SizeOfInt + (2 * MaxLengthForFirstNameDefault * SizeOfChar)), SeekOrigin.Current);

                        result.Add(this.ReadRecord(reader));
                    }
                    else
                    {
                        this.stream.Seek(SizeOfRecord - (SizeOfShort + SizeOfInt + (2 * MaxLengthForFirstNameDefault * SizeOfChar)), SeekOrigin.Current);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>An array of records.</returns>
        public List<FileCabinetRecord> GetRecords()
        {
            var records = new List<FileCabinetRecord>();

            using (var reader = new BinaryReader(this.stream, Encoding.Unicode, true))
            {
                while (reader.PeekChar() > -1)
                {
                    var newRecord = this.ReadRecord(reader);

                    records.Add(newRecord);
                }

                this.stream.Seek(0, SeekOrigin.Begin);
            }

            return records;
        }

        /// <summary>
        /// Gets a count of records.
        /// </summary>
        /// <returns>A count of records.</returns>
        public int GetStat()
        {
            return (int)(this.stream.Length / SizeOfRecord);
        }

        /// <summary>
        /// Makes a snapshot of file cabinet records.
        /// </summary>
        /// <returns>Snapshot of file cabinet records.</returns>
        public FileCabinetRecordSnapshot MakeSnapshot()
        {
            return new FileCabinetRecordSnapshot(this.GetRecords());
        }

        /// <summary>
        /// Restores records.
        /// </summary>
        /// <param name="snapshot">Snapshot to restore.</param>
        public void Restore(FileCabinetRecordSnapshot snapshot)
        {
            var newRecords = snapshot.GetRecords();
            var start = newRecords[0].Id;
            var end = newRecords[newRecords.Count - 1].Id;
            var startInsert = -1;
            int currentId = 0;
            int currentPosition = 0;
            bool readRecords = false;

            using (var reader = new BinaryReader(this.stream, Encoding.Unicode, true))
            {
                this.stream.Seek(SizeOfShort, SeekOrigin.Begin);

                if (reader.PeekChar() < 0)
                {
                    startInsert = 0;
                }

                while (reader.PeekChar() > -1)
                {
                    currentId = reader.ReadInt32();
                    this.stream.Seek(-SizeOfInt, SeekOrigin.Current);

                    if (currentId > start && startInsert == -1)
                    {
                        startInsert = currentPosition;
                    }

                    if (currentId < start)
                    {
                        startInsert = currentPosition + 1;
                    }

                    if (currentId > end)
                    {
                        readRecords = true;
                        break;
                    }

                    this.stream.Seek(SizeOfRecord, SeekOrigin.Current);
                }

                if (readRecords == true)
                {
                    this.stream.Seek(-SizeOfShort, SeekOrigin.Current);

                    while (reader.PeekChar() > -1)
                    {
                        newRecords.Add(ReadRecord(reader));
                    }
                }
            }

            using (var writer = new BinaryWriter(this.stream, Encoding.Unicode, true))
            {
                this.stream.Seek(startInsert * SizeOfRecord, SeekOrigin.Begin);

                foreach (var r in newRecords)
                {
                    this.stream.Seek(SizeOfShort, SeekOrigin.Current);
                    this.WriteRecordWithID(writer, r);
                }

                this.stream.Seek(0, SeekOrigin.Begin);
            }
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

        private void WriteRecordWithoutID(BinaryWriter writer, int id, FileCabinetRecordWithoutID record)
        {
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

        private void WriteRecordWithID(BinaryWriter writer, FileCabinetRecord record)
        {
            writer.Write(record.Id);

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

        private FileCabinetRecord ReadRecord(BinaryReader reader)
        {
            reader.ReadInt16();

            int id = reader.ReadInt32();

            string firstName = new string(reader.ReadChars(MaxLengthForFirstNameDefault)).Trim('\0');
            string lastName = new string(reader.ReadChars(MaxLengthForLastNameDefault)).Trim('\0');

            char sex = reader.ReadChar();

            int day = reader.ReadInt32();
            int month = reader.ReadInt32();
            int year = reader.ReadInt32();
            DateTime dateOfBirth = new DateTime(year, month, day);

            short weight = reader.ReadInt16();

            decimal balance = reader.ReadDecimal();

            return new FileCabinetRecord()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Sex = sex,
                DateOfBirth = dateOfBirth,
                Weight = weight,
                Balance = balance,
            };
        }
    }
}
