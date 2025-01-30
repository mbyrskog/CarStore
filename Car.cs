namespace CarStore
{
    public class Effect
    {
        public int KiloWatt { get; set; }
        public int Horsepower { get; set; }
    }

    public class Properties
    {
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Color { get; set; }
        public string Transmission { get; set; }
        public string Fuel { get; set; }
        public Effect Effect { get; set; }
    }

    public class Car
    {
        public string Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public Properties Properties { get; set; }
    }
}
