using System;
using System.Collections.Generic;
using System.Text;
using static FileCabinetApp.Literals;

namespace FileCabinetApp
{
    /// <summary>
    /// Realizes custom validation logic.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Validates a record.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        public void ValidateParameters(FileCabinetRecordWithoutID record)
        {
            this.ValidateFirstName(record.FirstName);

            this.ValidateLastName(record.LastName);

            this.ValidateSex(record.Sex);

            this.ValidateDateOfBirth(record.DateOfBirth);

            this.ValidateWeight(record.Weight);

            this.ValidateBalance(record.Balance);
        }

        /// <summary>
        /// Validates a first name.
        /// </summary>
        /// <param name="firstName">First name of the record.</param>
        public void ValidateFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} can't be null.");
            }

            firstName = firstName.Trim();

            if (firstName.Length < MinLengthForFirstNameCustom)
            {
                throw new ArgumentOutOfRangeException($"{nameof(firstName)} can't be less than {MinLengthForFirstNameCustom} letters.");
            }

            if (firstName.Length > MaxLengthForFirstNameCustom)
            {
                throw new ArgumentOutOfRangeException($"{nameof(firstName)} can't be more than {MaxLengthForFirstNameCustom} letters.");
            }

            if (!OnlyLetters(firstName))
            {
                throw new ArgumentException($"{nameof(firstName)} must be only with letters.");
            }
        }

        /// <summary>
        /// Validates a last name.
        /// </summary>
        /// <param name="lastName">Last name of the record.</param>
        public void ValidateLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} can't be null.");
            }

            lastName = lastName.Trim();

            if (lastName.Length < MinLengthForLastNameCustom)
            {
                throw new ArgumentOutOfRangeException($"{nameof(lastName)} can't be less than {MinLengthForLastNameCustom} letters.");
            }

            if (lastName.Length > MaxLengthForLastNameCustom)
            {
                throw new ArgumentOutOfRangeException($"{nameof(lastName)} can't be more than {MaxLengthForLastNameCustom} letters.");
            }

            if (!OnlyLetters(lastName))
            {
                throw new ArgumentException($"{nameof(lastName)} must be only with letters.");
            }
        }

        /// <summary>
        /// Validates sex.
        /// </summary>
        /// <param name="sex">Sex of the record.</param>
        public void ValidateSex(char sex)
        {
            if (!sex.Equals(MaleLetter) && !sex.Equals(FemaleLetter))
            {
                throw new ArgumentException($"{nameof(sex)} must be 'female' or 'male'.");
            }
        }

        /// <summary>
        /// Validates date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth of the record.</param>
        public void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth.CompareTo(MinDateOfBirthCustom) == -1 || dateOfBirth.CompareTo(MaxDateOfBirthCustom) == 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(dateOfBirth)} must be more than {MinDateOfBirthDefault} and less than current date.");
            }
        }

        /// <summary>
        /// Validates balance.
        /// </summary>
        /// <param name="balance">Balance of the record.</param>
        public void ValidateBalance(decimal balance)
        {
            if (balance < MinBalanceCustom)
            {
                throw new ArgumentOutOfRangeException($"{nameof(balance)} can't be less than {MinBalanceDefault}.");
            }
        }

        /// <summary>
        /// Validates weight.
        /// </summary>
        /// <param name="weight">Weight of the record.</param>
        public void ValidateWeight(short weight)
        {
            if (weight < MinWeightCustom || weight > MaxWeightCustom)
            {
                throw new ArgumentOutOfRangeException($"{nameof(weight)} must be more than {MinWeightCustom} kg and less than {MaxWeightCustom} kg.");
            }
        }

        private static bool OnlyLetters(string source)
        {
            foreach (var s in source)
            {
                if (IsDigit(s))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsDigit(char symbol)
        {
            if (symbol >= '0' && symbol <= '9')
            {
                return true;
            }

            return false;
        }
    }
}
