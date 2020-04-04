using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class which contains literals.
    /// </summary>
    public static class Literals
    {
#pragma warning disable SA1600 // Elements should be documented

        #region filesystem

        public const int SizeOfRecord = 278;
        public const int SizeOfShort = 2;
        public const int SizeOfInt = 4;
        public const int SizeOfChar = 2;
        public const int SizeOfDecimal = 16;

        #endregion

        #region system literals

        public const string FirstNameOutput = "first name: ";
        public const string LastNameOutput = "last name: ";
        public const string SexOutput = "sex: ";
        public const string DateOfBirthOutput = "date of birth: ";
        public const string WeightOutput = "weight: ";
        public const string BalanceOutput = "balance: ";

        public const string TryAgainOutput = "try it again.";

        public const char SpaceChar = ' ';
        public const char CommaChar = ',';
        public const char PointChar = '.';
        public const char SlashChar = '/';
        public const char BackSlashChar = '\\';
        public const char QuotationMarkChar = '"';
        public const char SingleQuoteChar = '\'';

        public const string FindFirstNameString = "firstname";
        public const string FindLastNameString = "lastname";
        public const string FindDateOfBirthString = "dateofbirth";

        public const int MinCountOfOptions = 2;

        #endregion

        #region default validation literals

        public const int MinLengthForFirstNameDefault = 2;
        public const int MaxLengthForFirstNameDefault = 60;

        public const int MinLengthForLastNameDefault = 2;
        public const int MaxLengthForLastNameDefault = 60;

        public const char MaleLetter = 'm';
        public const char FemaleLetter = 'f';

        public const short MinWeightDefault = 30;
        public const short MaxWeightDefault = 200;

        public const decimal MinBalanceDefault = 0;

        public static DateTime MinDateOfBirthDefault { get; } = new DateTime(1950, 1, 1);

        public static DateTime MaxDateOfBirthDefault { get; } = DateTime.Now;

        #endregion

        #region custom validation literals

        public const int MinLengthForFirstNameCustom = 3;
        public const int MaxLengthForFirstNameCustom = 60;

        public const int MinLengthForLastNameCustom = 3;
        public const int MaxLengthForLastNameCustom = 60;

        public const short MinWeightCustom = 50;
        public const short MaxWeightCustom = 100;

        public const decimal MinBalanceCustom = 100;

        public static DateTime MinDateOfBirthCustom { get; } = new DateTime(1970, 1, 1);

        public static DateTime MaxDateOfBirthCustom { get; } = DateTime.Now;

        #endregion

#pragma warning restore SA1600 // Elements should be documented
    }
}
