using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using Fclp;
using static FileCabinetApp.Literals;

namespace FileCabinetApp
{
    /// <summary>
    /// The main class that contains the IDE.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Alexei Putik";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static IFileCabinetService fileCabinetService;

        private class Service
        {
            public string Type { get; set; }
        }

        private static Service serviceType;

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "export", "exports the records", "The 'export' command exports records in file." },
            new string[] { "stat", "prints the statistics", "The 'stat' command prints the statistics." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "prints all records", "The 'list' command prints all records." },
            new string[] { "edit", "changes the record", "The 'edit' command changes the record." },
            new string[] { "find", "finds the records", "The 'find' command finds records." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">Arguments of the command line.</param>
        public static void Main(string[] args)
        {
            serviceType = ParseCommandLine(args);
            CreateService();

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine($"Using {serviceType.Type} validation rules.");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = fileCabinetService.GetStat();

            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Export(string parameters)
        {
            try
            {
                var options = ValidatePath(parameters);

                while (File.Exists(options[1]))
                {
                    Console.WriteLine($"File {options[1]} already exists. Do you want to rewrite it?(y/n)");
                    string answer = Console.ReadLine();

                    if (answer.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return;
                    }
                    else if (!answer.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                var snapshot = fileCabinetService.MakeSnapshot();
                var writer = new StreamWriter(options[1]);

                if (options[0].Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    snapshot.SaveToCsv(writer);
                }
                else
                {
                    snapshot.SaveToXml(writer);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string GetFolder(string path)
        {
            string[] folders = path.Split('\\');
            string folder = string.Empty;

            for (int i = 0; i < folders.Length - 1; i++)
            {
                folder += folders[i] + "\\";
            }

            return folder;
        }

        private static string[] ValidatePath(string parameters)
        {
            string[] inputs = parameters.Split(' ');

            if (inputs.Length < 2)
            {
                throw new ArgumentException("Incorrect command.");
            }

            inputs[1] = inputs[1].Trim(new char[] { ' ', '\"' });

            if (!inputs[0].Equals("csv", StringComparison.InvariantCultureIgnoreCase)
                && !inputs[0].Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Incorrect command.");
            }

            if (inputs[1].Contains('\\', StringComparison.CurrentCulture))
            {
                string folder = GetFolder(inputs[1]);

                if (!Directory.Exists(folder))
                {
                    throw new ArgumentException($"Directory {folder} doesn't exist.");
                }
            }

            return inputs;
        }

        private static bool Answer(string answer)
        {
            switch (answer.ToLower(CultureInfo.CurrentCulture))
            {
                case "yes":
                case "y":
                    {
                        answer = "y";
                        break;
                    }

                case "no":
                case "n":
                default:
                    {
                        answer = "n";
                        break;
                    }
            }

            return answer.Equals("y", StringComparison.InvariantCultureIgnoreCase) ? true : false;
        }

        private static void Create(string parameters)
        {
            try
            {
                Console.Write(FirstNameOutput);
                string firstName = ReadInput(StringConverter, FirstNameValidator);

                Console.Write(LastNameOutput);
                string lastName = ReadInput(StringConverter, LastNameValidator);

                Console.Write(SexOutput);
                char sex = ReadInput(CharConverter, SexValidator);

                Console.Write(WeightOutput);
                short weight = ReadInput(ShortConverter, WeightValidator);

                Console.Write(DateOfBirthOutput);
                DateTime dateOfBirth = ReadInput(DateConverter, DateOfBirthValidator);

                Console.Write(BalanceOutput);
                decimal balance = ReadInput(DecimalConverter, BalanceValidator);

                var newRecord = new FileCabinetRecordWithoutID()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Sex = sex,
                    DateOfBirth = dateOfBirth,
                    Weight = weight,
                    Balance = balance,
                };

                Console.WriteLine($"record #{fileCabinetService.CreateRecord(newRecord)} is created.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(TryAgainOutput);
            }
        }

        private static void Edit(string parameters)
        {
            try
            {
                int id;

                if (!int.TryParse(parameters, out id))
                {
                    Console.WriteLine("invalid id");
                    return;
                }

                Console.Write("first name: ");
                string firstName = ReadInput(StringConverter, FirstNameValidator);

                Console.Write("last name: ");
                string lastName = ReadInput(StringConverter, LastNameValidator);

                Console.Write("sex: ");
                char sex = ReadInput(CharConverter, SexValidator);

                Console.Write("weight: ");
                short weight = ReadInput(ShortConverter, WeightValidator);

                Console.Write("date: ");
                DateTime dateOfBirth = ReadInput(DateConverter, DateOfBirthValidator);

                Console.Write("balance: ");
                decimal balance = ReadInput(DecimalConverter, BalanceValidator);

                var changedRecord = new FileCabinetRecordWithoutID
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Sex = sex,
                    Weight = weight,
                    DateOfBirth = dateOfBirth,
                    Balance = balance,
                };

                fileCabinetService.EditRecord(id, changedRecord);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Find(string parameters)
        {
            var options = parameters.Split(SpaceChar);

            ReadOnlyCollection<FileCabinetRecord> result = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());

            try
            {
                if (options.Length != MinCountOfOptions)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(options)} can't contain not 2 parameters.");
                }

                string firstOption = options[0];
                string secondOption = options[1];

                secondOption = secondOption.Trim(QuotationMarkChar, SingleQuoteChar).Trim();

                if (firstOption.Equals(FindFirstNameString, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = fileCabinetService.FindByFirstName(secondOption);
                }
                else if (firstOption.Equals(FindLastNameString, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = fileCabinetService.FindByLastName(secondOption);
                }
                else if (firstOption.Equals(FindDateOfBirthString, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = fileCabinetService.FindByDateOfBirth(DateConvert(secondOption));
                }

                if (result.Count == 0)
                {
                    Console.WriteLine($"records with the {firstOption} {secondOption} was not found.");
                }
                else
                {
                    foreach (var r in result)
                    {
                        Console.WriteLine(r);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();

            foreach (var r in records)
            {
                Console.WriteLine(r);
            }
        }

        private static DateTime DateConvert(string dateString)
        {
            string[] date = dateString.Split(CommaChar, PointChar, SlashChar);
            DateTime dateOfBirth = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));

            return dateOfBirth;
        }

        private static Service ParseCommandLine(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToLower(CultureInfo.CurrentCulture);
            }

            var parser = new FluentCommandLineParser<Service>();

            parser.Setup(arg => arg.Type)
                .As('v', "validation-rules")
                .SetDefault("default");

            var result = parser.Parse(args);

            return parser.Object;
        }

        private static void CreateService()
        {
            switch (serviceType.Type)
            {
                case "default":
                    {
                        fileCabinetService = new FileCabinetService(new DefaultValidator());
                        break;
                    }

                case "custom":
                    {
                        fileCabinetService = new FileCabinetService(new CustomValidator());
                        break;
                    }
            }
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static Tuple<bool, string, string> StringConverter(string input)
        {
            bool result = true;
            string message = string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                result = false;
                message = $"{nameof(input)} can't be null or empty";
            }

            return new Tuple<bool, string, string>(result, message, input);
        }

        private static Tuple<bool, string, char> CharConverter(string input)
        {
            bool resultBool = true;
            string message = string.Empty;
            char resultChar = ' ';

            if (string.IsNullOrEmpty(input))
            {
                resultBool = false;
                message = $"{nameof(input)} can't be null or empty";
            }
            else
            {
                switch (input.ToLower(CultureInfo.CurrentCulture))
                {
                    case "male":
                    case "m":
                        {
                            resultChar = 'm';
                            break;
                        }

                    case "female":
                    case "f":
                        {
                            resultChar = 'f';
                            break;
                        }

                    default:
                        {
                            resultBool = false;
                            message = $"{nameof(input)} has incorrect type";
                            break;
                        }
                }
            }

            return new Tuple<bool, string, char>(resultBool, message, resultChar);
        }

        private static Tuple<bool, string, DateTime> DateConverter(string input)
        {
            bool resultBool = true;
            string message = string.Empty;
            var resultDate = DateTime.Now;

            if (!DateTime.TryParse(input, out resultDate))
            {
                resultBool = false;
                message = "invalid date";
            }

            return new Tuple<bool, string, DateTime>(resultBool, message, resultDate);
        }

        private static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            bool resultBool = true;
            string message = string.Empty;
            decimal resultDecimal = 0;

            if (!decimal.TryParse(input, out resultDecimal))
            {
                resultBool = false;
                message = $"{nameof(input)} can't be null or empty";
            }

            return new Tuple<bool, string, decimal>(resultBool, message, resultDecimal);
        }

        private static Tuple<bool, string, short> ShortConverter(string input)
        {
            bool resultBool = true;
            string message = string.Empty;
            short resultShort = 0;

            if (!short.TryParse(input, out resultShort))
            {
                resultBool = false;
                message = $"{nameof(input)} can't be null or empty";
            }

            return new Tuple<bool, string, short>(resultBool, message, resultShort);
        }

        private static Tuple<bool, string> FirstNameValidator(string input)
        {
            bool result = true;
            string message = string.Empty;

            try
            {
                fileCabinetService.Validator.ValidateFirstName(input);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return new Tuple<bool, string>(result, message);
        }

        private static Tuple<bool, string> LastNameValidator(string input)
        {
            bool result = true;
            string message = string.Empty;

            try
            {
                fileCabinetService.Validator.ValidateLastName(input);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return new Tuple<bool, string>(result, message);
        }

        private static Tuple<bool, string> SexValidator(char input)
        {
            bool result = true;
            string message = string.Empty;

            try
            {
                fileCabinetService.Validator.ValidateSex(input);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return new Tuple<bool, string>(result, message);
        }

        private static Tuple<bool, string> DateOfBirthValidator(DateTime input)
        {
            bool result = true;
            string message = string.Empty;

            try
            {
                fileCabinetService.Validator.ValidateDateOfBirth(input);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return new Tuple<bool, string>(result, message);
        }

        private static Tuple<bool, string> BalanceValidator(decimal input)
        {
            bool result = true;
            string message = string.Empty;

            try
            {
                fileCabinetService.Validator.ValidateBalance(input);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return new Tuple<bool, string>(result, message);
        }

        private static Tuple<bool, string> WeightValidator(short input)
        {
            bool result = true;
            string message = string.Empty;

            try
            {
                fileCabinetService.Validator.ValidateWeight(input);
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return new Tuple<bool, string>(result, message);
        }
    }
}
