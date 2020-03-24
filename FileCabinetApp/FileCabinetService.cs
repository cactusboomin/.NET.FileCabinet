using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> records = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

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
                Id = this.records.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                Sex = sex,
                Weight = weight,
                DateOfBirth = dateOfBirth,
                Balance = balance,
            };

            this.records.Add(newRecord);

            if (!this.firstNameDictionary.ContainsKey(firstName.ToLower(CultureInfo.CurrentCulture)))
            {
                this.firstNameDictionary[firstName.ToLower(CultureInfo.CurrentCulture)] = new List<FileCabinetRecord>();
            }

            if (!this.lastNameDictionary.ContainsKey(lastName.ToLower(CultureInfo.CurrentCulture)))
            {
                this.lastNameDictionary[lastName.ToLower(CultureInfo.CurrentCulture)] = new List<FileCabinetRecord>();
            }

            this.firstNameDictionary[firstName.ToLower(CultureInfo.CurrentCulture)].Add(newRecord);
            this.lastNameDictionary[lastName.ToLower(CultureInfo.CurrentCulture)].Add(newRecord);

            return newRecord.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, char sex, short weight, DateTime dateOfBirth, decimal balance)
        {
            for (int i = 0; i < this.records.Count; i++)
            {
                if (this.records[i].Id == id)
                {
                    this.firstNameDictionary[this.records[i].FirstName.ToLower(CultureInfo.CurrentCulture)].Remove(this.records[i]);
                    this.lastNameDictionary[this.records[i].LastName.ToLower(CultureInfo.CurrentCulture)].Remove(this.records[i]);

                    this.records[i].FirstName = firstName;
                    this.records[i].LastName = lastName;
                    this.records[i].Sex = sex;
                    this.records[i].Weight = weight;
                    this.records[i].DateOfBirth = dateOfBirth;
                    this.records[i].Balance = balance;

                    if (!this.firstNameDictionary.ContainsKey(this.records[i].FirstName.ToLower(CultureInfo.CurrentCulture)))
                    {
                        this.firstNameDictionary[this.records[i].FirstName.ToLower(CultureInfo.CurrentCulture)] = new List<FileCabinetRecord>();
                    }

                    if (!this.lastNameDictionary.ContainsKey(this.records[i].LastName.ToLower(CultureInfo.CurrentCulture)))
                    {
                        this.lastNameDictionary[this.records[i].LastName.ToLower(CultureInfo.CurrentCulture)] = new List<FileCabinetRecord>();
                    }

                    this.firstNameDictionary[this.records[i].FirstName.ToLower(CultureInfo.CurrentCulture)].Add(this.records[i]);
                    this.lastNameDictionary[this.records[i].LastName.ToLower(CultureInfo.CurrentCulture)].Add(this.records[i]);

                    return;
                }
            }

            throw new ArgumentException($"A user with id {id} doesn't exist.");
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.records.ToArray();
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} can't be null.");
            }

            if (firstName.Length == 0)
            {
                throw new ArgumentException($"{nameof(firstName)} can't be empty.");
            }

            return this.firstNameDictionary[firstName.ToLower(CultureInfo.CurrentCulture)].ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} can't be null.");
            }

            if (lastName.Length == 0)
            {
                throw new ArgumentException($"{nameof(lastName)} can't be empty.");
            }

            var result = new List<FileCabinetRecord>();

            foreach (var r in this.records)
            {
                if (r.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Add(r);
                }
            }

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            var result = new List<FileCabinetRecord>();

            foreach (var r in this.records)
            {
                if (r.DateOfBirth.Equals(dateOfBirth))
                {
                    result.Add(r);
                }
            }

            return result.ToArray();
        }

        public int GetStat()
        {
            return this.records.Count;
        }
    }
}
