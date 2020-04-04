using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents a file cabinet record.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="record">A record without ID.</param>
        public FileCabinetRecord(FileCabinetRecordWithoutID record)
        {
            this.FirstName = record.FirstName;
            this.LastName = record.LastName;
            this.Sex = record.Sex;
            this.DateOfBirth = record.DateOfBirth;
            this.Weight = record.Weight;
            this.Balance = record.Balance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        public FileCabinetRecord()
        {
        }

        /// <summary>
        /// Gets or sets an ID of the record.
        /// </summary>
        /// <value>
        /// An ID of the record.
        /// </value>
        public int Id { get; set; }

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

        /// <summary>
        /// Represents the record as string.
        /// </summary>
        /// <returns>The record as string.</returns>
        public override string ToString()
        {
            return $"#{this.Id}," +
                $"{this.FirstName}," +
                $"{this.LastName}," +
                $"{this.Sex}," +
                $"{this.Weight} kg," +
                $"{this.DateOfBirth.ToShortDateString()}," +
                $"${this.Balance}";
        }
    }
}
