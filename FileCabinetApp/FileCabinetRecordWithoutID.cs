using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents a file cabinet record without ID.
    /// </summary>
    public class FileCabinetRecordWithoutID
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordWithoutID"/> class.
        /// </summary>
        public FileCabinetRecordWithoutID()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordWithoutID"/> class.
        /// </summary>
        /// <param name="record">The record with ID.</param>
        public FileCabinetRecordWithoutID(FileCabinetRecord record)
        {
            this.FirstName = record.FirstName;
            this.LastName = record.LastName;
            this.Sex = record.Sex;
            this.Weight = record.Weight;
            this.DateOfBirth = record.DateOfBirth;
            this.Balance = record.Balance;
        }

        /// <summary>
        /// Gets or sets a first name of the record.
        /// </summary>
        /// <value>
        /// A first name of the record.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets a last name of the record.
        /// </summary>
        /// <value>
        /// A last name of the record.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets a sex of the record.
        /// </summary>
        /// <value>
        /// A sex of the record.
        /// </value>
        public char Sex { get; set; }

        /// <summary>
        /// Gets or sets a weight of the record.
        /// </summary>
        /// <value>
        /// A weight of the record.
        /// </value>
        public short Weight { get; set; }

        /// <summary>
        /// Gets or sets a date of birth of the record.
        /// </summary>
        /// <value>
        /// A date of birth of the record.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets a balance of the record.
        /// </summary>
        /// <value>
        /// A balance of the record.
        /// </value>
        public decimal Balance { get; set; }
    }
}
