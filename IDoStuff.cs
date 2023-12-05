namespace CarStore
{
    public interface IDoStuff
    {
        void PrintCars(IEnumerable<Car> carList);
        void PrintCarsPaginated(int pageSize);
        void PrintCarsGroupedByPrice();
        void ChangeCurrency();
        void WriteColoredLine(string message, ConsoleColor consoleColor);
        string GetTableHeader(string currencyName);
        void SelectOutput();
        void DisplayOptions();
    }
}
