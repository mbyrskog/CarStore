using Newtonsoft.Json;

namespace CarStore
{
    public class DoStuff : IDoStuff
    {
        private readonly string filePath = "cars.json";
        private List<Car> carList = [];
        private double currencyRate = 1.0;
        private string currencyName = "USD";
        private double distanceType = 1.0;

        public DoStuff()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ReadAndResetJsonFile(filePath);
        }

        public void ReadAndResetJsonFile(string filePath)
        {
            carList.Clear();
            var reader = new StreamReader(filePath);
            reader.DiscardBufferedData();
            var json = reader.ReadToEnd();
            carList = JsonConvert.DeserializeObject<List<Car>>(json);
        }

        public void PrintCars(IEnumerable<Car> carList)
        {
            WriteColoredLine(GetTableHeader(currencyName), ConsoleColor.DarkYellow);

            foreach (var item in carList)
            {
                Console.Write(item.Brand.PadRight(20) + item.Model.PadRight(20));
                Console.Write(item.Properties.Year.ToString().PadRight(20));
                Console.Write(item.Category.PadRight(20) + item.Properties.Transmission.PadRight(20));
                Console.Write(Math.Round((item.Properties.Mileage * distanceType), 2).ToString().PadRight(20));
                Console.WriteLine((string.Format("{0:0.00}", item.Price * currencyRate) + " " + currencyName));
                Thread.Sleep(20);
            }
        }

        public void PrintCarsPaginated(int pageSize)
        {
            int pageCounter = 1;
            var page = carList.Take(pageSize);

            while (page.Count() >= 1)
            {
                WriteColoredLine("Page " + pageCounter + " of " + carList.Count / pageSize, ConsoleColor.DarkYellow);
                PrintCars(page);
                WriteColoredLine("Press any key to continue", ConsoleColor.DarkYellow);
                Console.ReadKey(true);
                pageCounter++;
                page = carList.Skip(pageSize * pageCounter).Take(pageSize);
            }
            WriteColoredLine("Done", ConsoleColor.DarkYellow);
        }

        public void PrintCarsGroupedByPrice()
        {
            var orderedAndGroupedList = carList.Select(p => new Car()
            {
                Category = p.Category,
                Id = p.Id,
                Properties = p.Properties,
                Brand = p.Brand,
                Model = p.Model,
                Price = p.Price
            }).OrderBy(p => p.Price).GroupBy(p => Math.Floor(p.Price / 10000) * 10000).ToList();

            foreach (var priceSegment in orderedAndGroupedList)
            {
                WriteColoredLine($"{priceSegment.Key} - {priceSegment.Key + 10000}", ConsoleColor.DarkYellow);
                PrintCars(priceSegment.ToList());
            }
        }

        public void FilterCars()
        {
            string[] filters = ["brand", "model", "year (from)", "category", "transmission", "mileage (max)"];

            Console.WriteLine("Available options to filter out are " + string.Join(", ", filters));
            Console.WriteLine("Enter a option: ");

            var filterChoice = Console.ReadLine().ToLower().Trim();

            if (!filters.Any(filter => filter.Split(" ")[0].ToLower().Trim() == filterChoice))
            {
                WriteColoredLine("Invalid option", ConsoleColor.Red);
                return;
            }

            Console.WriteLine("Enter a " + filterChoice);
            var input = Console.ReadLine().ToLower().Trim();

            switch (filterChoice)
            {
                case "brand":
                    carList = carList.Where(b => b.Brand.ToLower().Contains(input)).ToList();
                    break;
                case "model":
                    carList = carList.Where(b => b.Model.ToLower().Contains(input)).ToList();
                    break;
                case "year":
                    carList = carList.Where(b => (b.Properties.Year >= int.Parse(input))).ToList();
                    break;
                case "category":
                    carList = carList.Where(b => b.Category.ToLower().Contains(input)).ToList();
                    break;
                case "transmission":
                    carList = carList.Where(b => b.Properties.Transmission.ToLower().Contains(input)).ToList();
                    break;
                case "mileage":
                    carList = carList.Where(b => (b.Properties.Mileage <= int.Parse(input))).ToList();
                    break;
            }

            if (carList.Count() < 1)
            {
                WriteColoredLine("No cars with " + filterChoice + ": " + input + " exists", ConsoleColor.Red);
                ReadAndResetJsonFile(filePath);
            }
            else
            {
                WriteColoredLine("Printing cars with " + filterChoice + ": " + input + ". Don't forget to reset the filter.", ConsoleColor.DarkYellow);
                PrintCars(carList);
            }

        }

        public void ResetFilter()
        {
            Console.WriteLine("Reset filter? yes/no");
            string input = Console.ReadLine().ToUpper().Trim();
            if (input == "YES" || input == "Y")
            {
                ReadAndResetJsonFile(filePath);
                WriteColoredLine("Filter is reset, print cars again to see all cars", ConsoleColor.DarkYellow);
            }
            else
            {
                WriteColoredLine("Filter is not reset", ConsoleColor.DarkYellow);
                return;
            }
        }

        public string GetTableHeader(string currencyName)
        {
            return
                (
                "Brand".PadRight(20) +
                "Model".PadRight(20) +
                "Year".PadRight(20) +
                "Category".PadRight(20) +
                "Transmission".PadRight(20) +
                (currencyName == "USD" || currencyName == "GBP" ? "Miles".PadRight(20) : "Mil".PadRight(20)) + "Price");
        }

        public void ChangeCurrency()
        {
            Console.WriteLine("Available options for currency are: USD, GBP, SEK, DKK. Distance type will also be converted.");

            string currencyInput = Console.ReadLine().ToUpper().Trim();
            currencyName = currencyInput;

            switch (currencyInput)
            {
                case "USD":
                    currencyRate = 1.0;
                    distanceType = 1.0;
                    break;
                case "GBP":
                    currencyRate = 0.71;
                    distanceType = 1.0;
                    break;
                case "SEK":
                    currencyRate = 8.38;
                    distanceType = 0.1609344;
                    break;
                case "DKK":
                    currencyRate = 6.06;
                    distanceType = 0.1609344;
                    break;
                default:
                    WriteColoredLine("Invalid option!", ConsoleColor.Red);
                    WriteColoredLine("Currency has been reset to USD, and distance type to miles", ConsoleColor.Red);
                    currencyRate = 1.0;
                    distanceType = 1.0;
                    currencyName = "USD";   
                    return;
            }
            WriteColoredLine("Currency converted to " + currencyName + ", print the cars to see the result.", ConsoleColor.DarkYellow);
        }

        public void WriteColoredLine(string message, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void SelectOutput()
        {
            WriteColoredLine("Welcome to the CarStore!", ConsoleColor.Cyan);
            DisplayOptions();
            var shouldRun = true;
            while (shouldRun)
            {
                Console.Write("Enter an option: ");
                var input = Console.ReadKey();
                Console.WriteLine("\n");
                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        WriteColoredLine("Printing cars", ConsoleColor.DarkYellow);
                        PrintCars(carList);
                        break;
                    case ConsoleKey.D2:
                        WriteColoredLine("Printing cars paginated", ConsoleColor.DarkYellow);
                        PrintCarsPaginated(5);
                        break;
                    case ConsoleKey.D3:
                        WriteColoredLine("Printing cars grouped by price", ConsoleColor.DarkYellow);
                        PrintCarsGroupedByPrice();
                        break;
                    case ConsoleKey.D4:
                        WriteColoredLine("Changing currency and distance type", ConsoleColor.DarkYellow);
                        ChangeCurrency();
                        break;
                    case ConsoleKey.D5:
                        WriteColoredLine("Applying filter", ConsoleColor.DarkYellow);
                        FilterCars();
                        break;
                    case ConsoleKey.D6:
                        WriteColoredLine("Reset filter", ConsoleColor.DarkYellow);
                        ResetFilter();
                        break;
                    case ConsoleKey.Q:
                        shouldRun = false;
                        WriteColoredLine("Press any key to exit!", ConsoleColor.DarkYellow);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        WriteColoredLine("Invalid option!", ConsoleColor.Red);
                        break;
                }
                DisplayOptions();
            }
            Console.ReadKey();
        }

        public void DisplayOptions()
        {
            WriteColoredLine("\n" + "Main menu", ConsoleColor.DarkYellow);
            WriteColoredLine("1 - Print cars", ConsoleColor.DarkYellow);
            WriteColoredLine("2 - Print cars paginated", ConsoleColor.DarkYellow);
            WriteColoredLine("3 - Print cars grouped by price", ConsoleColor.DarkYellow);
            WriteColoredLine("4 - Change currency and distance type", ConsoleColor.DarkYellow);
            WriteColoredLine("5 - Apply filter and list matching cars", ConsoleColor.DarkYellow);
            WriteColoredLine("6 - Reset filter", ConsoleColor.DarkYellow);
            WriteColoredLine("q - Quit", ConsoleColor.DarkYellow);
        }
    }

}
