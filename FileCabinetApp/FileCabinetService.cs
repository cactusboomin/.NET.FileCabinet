﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, char sex, short weight, DateTime dateOfBirth, decimal balance)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} can't be null.");
            }

            firstName = firstName.Trim();

            if (firstName.Length < 2)
            {
                throw new ArgumentOutOfRangeException($"{nameof(firstName)} can't be less than 2 letters.");
            }

            if (firstName.Length > 60)
            {
                throw new ArgumentOutOfRangeException($"{nameof(firstName)} can't be more than 60 letters.");
            }

            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} can't be null.");
            }

            lastName = lastName.Trim();

            if (lastName.Length < 2)
            {
                throw new ArgumentOutOfRangeException($"{nameof(lastName)} can't be less than 2 letters.");
            }

            if (lastName.Length > 60)
            {
                throw new ArgumentOutOfRangeException($"{nameof(lastName)} can't be more than 60 letters.");
            }

            if (!sex.Equals('m') && !sex.Equals('f'))
            {
                throw new ArgumentException($"{nameof(sex)} must be 'f' or 'm'.");
            }

            if (dateOfBirth.CompareTo(new DateTime(1950, 01, 01)) == -1 || dateOfBirth.CompareTo(DateTime.Now) == 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(dateOfBirth)} must be more than 01.01.1950 and less than current date.");
            }

            if (weight < 30 || weight > 200)
            {
                throw new ArgumentOutOfRangeException($"{nameof(weight)} must be more than 30 kg and less than 200 kg.");
            }

            if (balance < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(balance)} can't be less than 0.");
            }

            var newRecord = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                Sex = sex,
                Weight = weight,
                DateOfBirth = dateOfBirth,
                Balance = balance,
            };

            this.list.Add(newRecord);

            return newRecord.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}