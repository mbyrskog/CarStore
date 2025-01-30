namespace CarStore
{
    public interface IDoStuff
    {
        void ReadAndResetJsonFile(string filePath);
        void PrintCars(IEnumerable<Car> carList);
        void PrintCarsPaginated(int pageSize);
        void PrintCarsGroupedByPrice();
        void FilterCars();
        void ResetFilter();
        void ChangeCurrency();
        void WriteColoredLine(string message, ConsoleColor consoleColor);
        string GetTableHeader(string currencyName);
        void SelectOutput();
        void DisplayOptions();
    }
}
