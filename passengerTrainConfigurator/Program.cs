class program
{
    static void Main(string[] args)
    {
        Operator tripOperator = new Operator();

        bool isWork = true;

        Console.WriteLine("Добро пожаловать в конфигуратор поездов!");

        while (isWork)
        {
            tripOperator.ShowTrip();

            Console.WriteLine("\n");
            Console.WriteLine("Выбирите действие:\n" +
            "1. Создать рейс.\n" +
            "2. Выход.");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    tripOperator.MakeDirection();
                    tripOperator.TakePassengersToDirection();
                    tripOperator.FormTrain();
                    tripOperator.SendTrip();
                    break;
                case "2":
                    isWork = false;
                    break;
                default:
                    Console.WriteLine("Такой команды нет!");
                    break;
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

class Trip
{
    private Direction _direction;
    private Train _train;
    private int _countOfPassengers;

    public Trip(Direction direction, Train train, int countOfPassengers)
    {
        _direction = direction;
        _train = train;
        _countOfPassengers = countOfPassengers;
    }

    public void ShowInfo()
    {
        Console.WriteLine("Рейс:");
        _direction.ShowInfo();
        _train.ShowInfo();
        Console.WriteLine($"Всего пассажиров: {_countOfPassengers}");
    }
}

class Operator
{
    private List<string> _cities = new List<string>();
    private List<Trip> _trips = new List<Trip>();
    private Direction _direction;
    private Train _train;
    private BoxOffice _boxOffice = new BoxOffice();

    private int _countOfPassengers;

    public Operator()
    {
        _cities.Add("Москва");
        _cities.Add("Казань");
        _cities.Add("Иркутск");
        _cities.Add("Санкт-Петербург");
        _cities.Add("Ростов");
        _cities.Add("Тамбов");
        _cities.Add("Рязань");
        _cities.Add("Алма-Ата");
    }

    public void MakeDirection()
    {
        bool isWork = true;
        string cityOfDeparture;
        string cityOfDestination;

        while (isWork)
        {
            ShowCities();

            Console.Write($"Введите номер города в качестве точки отправления: ");
            cityOfDeparture = GetCity();

            Console.Write($"Введите номер города в качестве точки назначения: ");
            cityOfDestination = GetCity();

            if (cityOfDeparture != cityOfDestination)
            {
                _direction = new Direction(cityOfDeparture, cityOfDestination);
                isWork = false;
                Console.WriteLine("Направление успешно создано!");
            }
            else
            {
                Console.WriteLine("Город отправления не может быть городом назначения! Повторите попытку.");
            }
        }
    }

    public void TakePassengersToDirection()
    {
        _countOfPassengers = _boxOffice.SellTickets();
        Console.WriteLine($"На направление: {_direction.PointOfDeparture} - {_direction.PointOfDestination} купили {_countOfPassengers} билетов.");
        Console.ReadKey();
    }

    public void FormTrain()
    {
        _train = new Train();
        _train.Form(_countOfPassengers);
    }

    public void SendTrip()
    {
        _trips.Add(new Trip(_direction, _train, _countOfPassengers));
    }

    public void ShowTrip()
    {
        if (_trips.Count > 0)
        {
            foreach (var trip in _trips)
            {
                trip.ShowInfo();
            }
        }
        else
        {
            Console.WriteLine("Рейсы отсутствуют!!!");
        }
    }

    private void ShowCities()
    {
        int cityCount = 0;
        foreach (var city in _cities)
        {
            cityCount++;
            Console.WriteLine($"{cityCount}) {city}.");
        }
    }

    private string GetCity()
    {
        bool isCity = false;
        string nameCity = "";

        while (isCity == false)
        {
            int numberCity = UserUtils.GetNumber();

            if (numberCity - 1 < 0 || numberCity - 1 > _cities.Count)
            {
                Console.WriteLine("Города под таким номером нет! Повторите попытку.");
            }
            else
            {
                nameCity = _cities[numberCity - 1];
                isCity = true;
            }

        }
        return nameCity;
    }
}

class Direction
{
    public string PointOfDeparture { get; private set; }
    public string PointOfDestination { get; private set; }

    public Direction(string pointOfDeparture, string pointOfDestination)
    {
        PointOfDeparture = pointOfDeparture;
        PointOfDestination = pointOfDestination;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Точка отправления: {PointOfDeparture}\nТочка назначения: {PointOfDestination}");
    }
}

class BoxOffice
{
    private int _tickets;

    public int SellTickets()
    {
        Random random = new Random();
        _tickets = random.Next(80, 300);

        return _tickets;
    }
}

class Train
{
    private int _numberOfseats;
    private List<Wagon> _wagons = new List<Wagon>();

    public void ShowInfo()
    {

        if (_wagons.Count > 0)
        {
            int numberOfWagon = 0;

            foreach (Wagon wagon in _wagons)
            {
                numberOfWagon++;
            }

            Console.WriteLine($"Всего мест в поезде - {_numberOfseats}, количество вагонов {numberOfWagon}");
        }
    }

    public void Form(int countOfPassengers)
    {
        LittleWagon littleWagon = new LittleWagon();
        MiddleWagon middleWagon = new MiddleWagon();
        BigWagon bigWagon = new BigWagon();

        while (countOfPassengers > _numberOfseats)
        {
            Console.Clear();
            Console.WriteLine($"|всего мест / пассажиров| {_numberOfseats} / {countOfPassengers}\n");
            Console.WriteLine($"" +
                $"1. Добавить {littleWagon.Type} (+{littleWagon.NumberOfSeats} мест)\n" +
                $"2. Добавить {middleWagon.Type} (+{middleWagon.NumberOfSeats} мест)\n" +
                $"3. Добавить {bigWagon.Type} (+{bigWagon.NumberOfSeats} мест)");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    AddLittleWagon();
                    break;
                case "2":
                    AddMiddleWagon();
                    break;
                case "3":
                    AddBigWagon();
                    break;
                default:
                    Console.WriteLine("Ошибка ввода!");
                    break;
            }
        }
        Console.WriteLine($"Мест в поезде достаточно, чтобы отправить рейс(Мест {_numberOfseats} / Пассажиров {countOfPassengers})!");
    }

    private void AddLittleWagon()
    {
        _wagons.Add(new LittleWagon());
        CountNumberOfSeats();
    }

    private void AddMiddleWagon()
    {
        _wagons.Add(new MiddleWagon());
        CountNumberOfSeats();
    }

    private void AddBigWagon()
    {
        _wagons.Add(new BigWagon());
        CountNumberOfSeats();
    }

    private void CountNumberOfSeats()
    {
        int sumNumberOfSeats = 0;

        foreach (var wagon in _wagons)
        {
            sumNumberOfSeats += wagon.NumberOfSeats;
        }

        _numberOfseats = sumNumberOfSeats;
    }
}

class Wagon
{
    public string Type { get; protected set; }
    public int NumberOfSeats { get; protected set; }

    public void ShowInfo()
    {
        Console.WriteLine($"Тип вагона - {Type} | Количество мест - {NumberOfSeats}");
    }
}

class LittleWagon : Wagon
{
    public LittleWagon()
    {
        Type = "Маленький";
        NumberOfSeats = 15;
    }
}

class MiddleWagon : Wagon
{
    public MiddleWagon()
    {
        Type = "Средний вагон.";
        NumberOfSeats = 32;
    }
}

class BigWagon : Wagon
{
    public BigWagon()
    {
        Type = "Большой вагон.";
        NumberOfSeats = 54;
    }
}

static class UserUtils
{
    public static int GetNumber()
    {
        bool isNumberWork = true;
        int userNumber = 0;

        while (isNumberWork)
        {
            bool isNumber = true;
            string userInput = Console.ReadLine();

            if (isNumber = int.TryParse(userInput, out int number))
            {
                userNumber = number;
                isNumberWork = false;
            }
            else
            {
                Console.WriteLine($"Не правильный ввод данных!!!  Повторите попытку");
            }
        }

        return userNumber;
    }
}
