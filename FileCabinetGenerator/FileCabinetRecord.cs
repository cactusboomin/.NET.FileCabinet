using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Represents a file cabinet record.
    /// </summary>
    [Serializable]
    [XmlRoot("record")]
    public class FileCabinetRecord
    {
        public FileCabinetRecord()
        {
        }

        /// <summary>
        /// Gets or sets an ID of the record.
        /// </summary>
        /// <value>
        /// An ID of the record.
        /// </value>
        [XmlAttribute]
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
