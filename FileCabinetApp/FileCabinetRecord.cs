using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public char Sex { get; set; }

        public short Weight { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal Balance { get; set; }

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
