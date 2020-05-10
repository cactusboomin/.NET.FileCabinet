using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Realizes an interface of file cabinet service.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Gets realization of the validator.
        /// </summary>
        /// <value>
        /// Realization of the validator.
        /// </value>
        public IRecordValidator Validator { get; }

        /// <summary>
        /// Makes a snapshot of file cabinet records.
        /// </summary>
        /// <returns>File cabinet records snapshot.</returns>
        public FileCabinetRecordSnapshot MakeSnapshot();

        /// <summary>
        /// Restores records.
        /// </summary>
        /// <param name="snapshot">Snapshot to restore.</param>
        public void Restore(FileCabinetRecordSnapshot snapshot);

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="record">A record that contains information about the new record.</param>
        /// <returns>An ID of the new record.</returns>
        public int CreateRecord(FileCabinetRecordWithoutID record);

        /// <summary>
        /// Edits a record.
        /// </summary>
        /// <param name="id">An ID of the editable record.</param>
        /// <param name="changedRecord">Changed record.</param>
        public void EditRecord(int id, FileCabinetRecordWithoutID changedRecord);

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>An array of records.</returns>
        public List<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Finds a record by first name.
        /// </summary>
        /// <param name="firstName">First name of records.</param>
        /// <returns>An array of records with certain first name.</returns>
        public List<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds a record by last name.
        /// </summary>
        /// <param name="lastName">Last name of records.</param>
        /// <returns>An array of records with certain last name.</returns>
        public List<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds a record by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth of records.</param>
        /// <returns>An array of records with certain date of birth.</returns>
        public List<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Gets a count of records.
        /// </summary>
        /// <returns>A count of records.</returns>
        public int GetStat();
    }
}
