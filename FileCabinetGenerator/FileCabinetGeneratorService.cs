using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetGenerator
{
    public class FileCabinetGeneratorService
    {
        private List<FileCabinetRecord> records = new List<FileCabinetRecord>();

        public FileCabinetGeneratorSnapshot MakeSnapshot()
        {
            return new FileCabinetGeneratorSnapshot(this.records);
        }

        public void GenerateRecords(int startId, int count)
        {
            var id = startId;

            for (int i = 0; i < count; i++)
            {
                var record = GenerateOneRecord(id);
                id++;

                this.records.Add(record);
            }
        }

        private static string GenerateName()
        {
            var chars = "qwertyuiopasdfghjklzxcvbnm";
            Random randomNumber = new Random();

            var length = randomNumber.Next(2, 60);

            var builder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                builder.Append(chars[randomNumber.Next(0, chars.Length)]);
            }

            return builder.ToString();
        }

        private static char GenerateSex()
        {
            Random number = new Random();
            var sex = number.Next();

            if (sex % 2 == 0)
            {
                return 'f';
            }
            else
            {
                return 'm';
            }
        }

        private static short GenerateWeight()
        {
            Random weight = new Random();

            return (short)weight.Next(30, 200);
        }

        private static DateTime GenerateDateOfBirth()
        {
            Random number = new Random();

            var year = number.Next(1950, 2020);
            var month = number.Next(1, 12);
            int day;

            if (month == 2)
            {
                day = number.Next(1, 28);
            }
            else
            {
                day = number.Next(1, 30);
            }

            return new DateTime(year, month, day);
        }

        private static decimal GenerateBalance()
        {
            Random number = new Random();

            return number.Next();
        }

        private static FileCabinetRecord GenerateOneRecord(int id)
        {
            var firstName = GenerateName();
            var lastName = GenerateName();
            var sex = GenerateSex();
            var weight = GenerateWeight();
            var dateOfBirth = GenerateDateOfBirth();
            var balance = GenerateBalance();

            return new FileCabinetRecord()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Sex = sex,
                Weight = weight,
                DateOfBirth = dateOfBirth,
                Balance = balance,
            };
        }
    }
}
