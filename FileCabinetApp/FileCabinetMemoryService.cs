using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using static FileCabinetApp.Literals;

namespace FileCabinetApp
{
    /// <summary>
    /// Contains a logic for file cabinet.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        /// <summary>
        /// Gets realization of the validator.
        /// </summary>
        /// <value>
        /// Realization of the validator.
        /// </value>
        public IRecordValidator Validator { get; }

        private readonly List<FileCabinetRecord> records = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.Validator = validator;
        }

        /// <summary>
        /// Makes a snapshot of file cabinet records.
        /// </summary>
        /// <returns>Snapshot of file cabinet records.</returns>
        public FileCabinetRecordSnapshot MakeSnapshot()
        {
            return new FileCabinetRecordSnapshot(this.records);
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
            var index = 0;
            var startInsert = 0;

            while (index < this.records.Count)
            {
                if (this.records[index].Id >= start && this.records[index].Id <= end)
                {
                    this.records.RemoveAt(index);
                }
                else
                {
                    index++;
                    startInsert++;
                }
            }

            this.records.InsertRange(startInsert, newRecords);
        }

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="record">A record that contains information about the new record.</param>
        /// <returns>An ID of the new record.</returns>
        public int CreateRecord(FileCabinetRecordWithoutID record)
        {
            this.Validator.ValidateParameters(record);

            var newRecord = new FileCabinetRecord(record);

            if (this.records.Count == 0)
            {
                newRecord.Id = this.records.Count + 1;
            }
            else
            {
                newRecord.Id = this.records[this.records.Count - 1].Id + 1;
            }

            this.records.Add(newRecord);

            if (!this.firstNameDictionary.ContainsKey(record.FirstName.ToLower(CultureInfo.CurrentCulture)))
            {
                this.firstNameDictionary[record.FirstName.ToLower(CultureInfo.CurrentCulture)] = new List<FileCabinetRecord>();
            }

            if (!this.lastNameDictionary.ContainsKey(record.LastName.ToLower(CultureInfo.CurrentCulture)))
            {
                this.lastNameDictionary[record.LastName.ToLower(CultureInfo.CurrentCulture)] = new List<FileCabinetRecord>();
            }

            if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth] = new List<FileCabinetRecord>();
            }

            this.firstNameDictionary[record.FirstName.ToLower(CultureInfo.CurrentCulture)].Add(newRecord);
            this.lastNameDictionary[record.LastName.ToLower(CultureInfo.CurrentCulture)].Add(newRecord);
            this.dateOfBirthDictionary[record.DateOfBirth].Add(newRecord);

            return newRecord.Id;
        }

        /// <summary>
        /// Edits a record.
        /// </summary>
        /// <param name="id">An ID of the editable record.</param>
        /// <param name="changedRecord">Changed record.</param>
        public void EditRecord(int id, FileCabinetRecordWithoutID changedRecord)
        {
            this.Validator.ValidateParameters(changedRecord);

            for (int i = 0; i < this.records.Count; i++)
            {
                if (this.records[i].Id == id)
                {
                    this.firstNameDictionary[this.records[i].FirstName.ToLower(CultureInfo.CurrentCulture)].Remove(this.records[i]);
                    this.lastNameDictionary[this.records[i].LastName.ToLower(CultureInfo.CurrentCulture)].Remove(this.records[i]);
                    this.dateOfBirthDictionary[this.records[i].DateOfBirth].Remove(this.records[i]);

                    int idOfTheRecord = this.records[i].Id;
                    this.records[i] = new FileCabinetRecord(changedRecord);
                    this.records[i].Id = idOfTheRecord;

                    if (!this.firstNameDictionary.ContainsKey(this.records[i].FirstName.ToLower(CultureInfo.CurrentCulture)))
                    {
                        this.firstNameDictionary[this.records[i].FirstName.ToLower(CultureInfo.CurrentCulture)] = new List<FileCabinetRecord>();
                    }

                    if (!this.lastNameDictionary.ContainsKey(this.records[i].LastName.ToLower(CultureInfo.CurrentCulture)))
                    {
                        this.lastNameDictionary[this.records[i].LastName.ToLower(CultureInfo.CurrentCulture)] = new List<FileCabinetRecord>();
                    }

                    if (!this.dateOfBirthDictionary.ContainsKey(this.records[i].DateOfBirth))
                    {
                        this.dateOfBirthDictionary[this.records[i].DateOfBirth] = new List<FileCabinetRecord>();
                    }

                    this.firstNameDictionary[this.records[i].FirstName.ToLower(CultureInfo.CurrentCulture)].Add(this.records[i]);
                    this.lastNameDictionary[this.records[i].LastName.ToLower(CultureInfo.CurrentCulture)].Add(this.records[i]);
                    this.dateOfBirthDictionary[this.records[i].DateOfBirth].Add(this.records[i]);

                    return;
                }
            }

            throw new ArgumentException($"A user with id {id} doesn't exist.");
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>An array of records.</returns>
        public List<FileCabinetRecord> GetRecords()
        {
            return this.records;
        }

        /// <summary>
        /// Finds a record by first name.
        /// </summary>
        /// <param name="firstName">First name of records.</param>
        /// <returns>An array of records with certain first name.</returns>
        public List<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} can't be null.");
            }

            if (firstName.Length == 0)
            {
                throw new ArgumentException($"{nameof(firstName)} can't be empty.");
            }

            if (!this.firstNameDictionary.ContainsKey(firstName.ToLower(CultureInfo.CurrentCulture)))
            {
                return new List<FileCabinetRecord>();
            }

            return this.firstNameDictionary[firstName.ToLower(CultureInfo.CurrentCulture)];
        }

        /// <summary>
        /// Finds a record by last name.
        /// </summary>
        /// <param name="lastName">Last name of records.</param>
        /// <returns>An array of records with certain last name.</returns>
        public List<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} can't be null.");
            }

            if (lastName.Length == 0)
            {
                throw new ArgumentException($"{nameof(lastName)} can't be empty.");
            }

            if (!this.lastNameDictionary.ContainsKey(lastName.ToLower(CultureInfo.CurrentCulture)))
            {
                return new List<FileCabinetRecord>();
            }

            return this.lastNameDictionary[lastName.ToLower(CultureInfo.CurrentCulture)];
        }

        /// <summary>
        /// Finds a record by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth of records.</param>
        /// <returns>An array of records with certain date of birth.</returns>
        public List<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                return new List<FileCabinetRecord>();
            }

            return this.dateOfBirthDictionary[dateOfBirth];
        }

        /// <summary>
        /// Gets a count of records.
        /// </summary>
        /// <returns>A count of records.</returns>
        public int GetStat()
        {
            return this.records.Count;
        }
    }
}
