using System;
using System.Globalization;
using System.IO;
using Fclp;

namespace FileCabinetGenerator
{
    public static class Program
    {
        private class Generator
        {
            public string OutputType { get; set; }

            public string OutputFile { get; set; }

            public int RecordsAmount { get; set; }

            public int StartId { get; set; }
        }

        private static Generator generator;
        private static FileCabinetGeneratorService service = new FileCabinetGeneratorService();

        public static void Main(string[] args)
        {
            generator = ParseCommandLine(args);
            ValidateGenerator();

            var writer = new StreamWriter(generator.OutputFile + "." + generator.OutputType);

            GenerateData(writer);

            writer.Close();
        }

        private static Generator ParseCommandLine(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToLower(CultureInfo.CurrentCulture);
            }

            var parser = new FluentCommandLineParser<Generator>();

            parser.Setup(arg => arg.OutputType)
                .As('t', "output-type")
                .SetDefault("csv");

            parser.Setup(arg => arg.OutputFile)
                .As('o', "output")
                .SetDefault("records");

            parser.Setup(arg => arg.RecordsAmount)
                .As('a', "records-amount")
                .SetDefault(1000);

            parser.Setup(arg => arg.StartId)
                .As('i', "start-id")
                .SetDefault(1000);

            var result = parser.Parse(args);

            return parser.Object;
        }

        private static void GenerateData(StreamWriter writer)
        {
            try
            {
                service.GenerateRecords(generator.StartId, generator.RecordsAmount);

                var snapshot = service.MakeSnapshot();

                if (generator.OutputType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
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

        private static void ValidateGenerator()
        {
            ValidateOutputType();
            ValidateOutputFile();
            ValidateRecordsAmount();
            ValidateStartId();
        }

        private static void ValidateOutputType()
        {
            if (!generator.OutputType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!generator.OutputType.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    generator.OutputType = "csv";
                }
            }
        }

        private static void ValidateOutputFile()
        {
            string[] path = generator.OutputFile.Split('\\', '/');

            string folder = string.Empty;

            for (int i = 0; i < path.Length - 1; i++)
            {
                folder += path[i] + "\\";
            }

            if (!Directory.Exists(folder))
            {
                generator.OutputFile = "records";
            }
        }

        private static void ValidateRecordsAmount()
        {
            if (generator.RecordsAmount < 1)
            {
                generator.RecordsAmount = 1000;
            }
        }

        private static void ValidateStartId()
        {
            if (generator.StartId < 1)
            {
                generator.StartId = 1000;
            }
        }
    }
}
