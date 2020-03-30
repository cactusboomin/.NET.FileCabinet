using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Realizes an interface for validation logic.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates a record.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        public void ValidateParameters(FileCabinetRecordWithoutID record);

        /// <summary>
        /// Validates a first name.
        /// </summary>
        /// <param name="firstName">First name of the record.</param>
        public void ValidateFirstName(string firstName);

        /// <summary>
        /// Validates a last name.
        /// </summary>
        /// <param name="lastName">Last name of the record.</param>
        public void ValidateLastName(string lastName);

        /// <summary>
        /// Validates sex.
        /// </summary>
        /// <param name="sex">Sex of the record.</param>
        public void ValidateSex(char sex);

        /// <summary>
        /// Validates date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth of the record.</param>
        public void ValidateDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Validates balance.
        /// </summary>
        /// <param name="balance">Balance of the record.</param>
        public void ValidateBalance(decimal balance);

        /// <summary>
        /// Validates weight.
        /// </summary>
        /// <param name="weight">Weight of the record.</param>
        public void ValidateWeight(short weight);
    }
}
