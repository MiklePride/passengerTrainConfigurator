class program
{
    static void Main(string[] args)
    {
        BoxOffice boxOffice = new BoxOffice();
        Operator tripOperator = new Operator();

        bool isWork = true;

        Console.WriteLine("Добро пожаловать в конфигуратор поездов!");

        while (isWork)
        {
            tripOperator.ShowTrip();
            Console.WriteLine("\n\n\n");

            Console.WriteLine("Выбирите действие:\n" +
            "1. Создать направление.\n" +
            "2. Продать билеты.\n" +
            "3. Сформировать поезд.\n" +
            "4. Отправить рейс.\n" +
            "5. Выход.");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    tripOperator.CreateDirection();
                    break;
                case "2":
                    tripOperator.TakePassengersToDirection(boxOffice);
                    break;
                case "3":
                    tripOperator.FormTrain();
                    break;
                case "4":
                    tripOperator.SendTrip();
                    break;
                case "5":
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

    public Trip(Direction direction, Train train)
    {
        _direction = direction;
        _train = train;
    }

    public void ShowInfo()
    {
        Console.WriteLine("Рейс:");
        _direction.ShowInfo();
        _train.ShowInfo();
    }
}

class Operator
{
    private Dictionary<int, string> _cities = new Dictionary<int, string>();
    private List<Trip> _trips = new List<Trip>();
    private Direction _direction;
    private Train _train;

    private bool _hasDirectionBeenCreated;
    private bool _haveTicketsBeenSold;
    private bool _isTrainFormed;

    public Operator()
    {
        _hasDirectionBeenCreated = false;
        _haveTicketsBeenSold = false;
        _isTrainFormed = false;

        _cities.Add(1, "Москва");
        _cities.Add(2, "Казань");
        _cities.Add(3, "Иркутск");
        _cities.Add(4, "Санкт-Петербург");
        _cities.Add(5, "Ростов");
        _cities.Add(6, "Тамбов");
        _cities.Add(7, "Рязань");
        _cities.Add(8, "Алма-Ата");
    }

    public void CreateDirection()
    {
        if (_hasDirectionBeenCreated == false)
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
                    _hasDirectionBeenCreated = true;
                    isWork = false;
                    Console.WriteLine("Направление успешно создано!");
                }
                else
                {
                    Console.WriteLine("Город отправления не может быть городом назначения! Повторите попытку.");
                }
            }
        }
        else
        {
            Console.WriteLine("Направление уже создано! Чтобы создать новый, завершите создание рейса.");
        }
    }

    public void TakePassengersToDirection(BoxOffice boxOffice)
    {
        if (_hasDirectionBeenCreated == true)
        {
            boxOffice.SellTickets(_direction);
            _haveTicketsBeenSold = true;
            Console.WriteLine($"На направление: {_direction.PointOfDeparture} - {_direction.PointOfDestination} купили {_direction.NumberOfPassengers} билетов.");
        }
        else
        {
            Console.WriteLine("Чтобы продать билеты сначала создайте направление!");
        }
    }

    public void FormTrain()
    {
        if (_haveTicketsBeenSold == true && _isTrainFormed == false)
        {
            _train = new Train();
            _train.Form(_direction);
            _isTrainFormed = true;
        }
        else if (_haveTicketsBeenSold == true && _isTrainFormed == true)
        {
            Console.WriteLine("Поезд уже сформирован! Отправьте рейс, чтобы сформировать новый!");
        }
        else
        {
            Console.WriteLine("Сначала создайте направление и продайте билеты на него!");
        }
    }

    public void SendTrip()
    {
        if (_isTrainFormed == true)
        {
            _trips.Add(new Trip(_direction, _train));
            _hasDirectionBeenCreated = false;
            _haveTicketsBeenSold = false;
            _isTrainFormed = false;
        }
        else
        {
            Console.WriteLine("Чтобы отправить рейс сформируйте поезд!");
        }
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
        foreach (var city in _cities)
        {
            Console.WriteLine($"{city.Key}. {city.Value}");
        }
    }

    private int GetNumber()
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

    private string GetCity()
    {
        bool isCity = false;
        string nameCity = "";

        while (isCity == false)
        {
            int numberCity = GetNumber();

            if (_cities.ContainsKey(numberCity))
            {
                nameCity = _cities[numberCity];
                isCity = true;
            }
            else
            {
                Console.WriteLine("Города под таким номером нет! Повторите попытку...");
            }
        }
        return nameCity;
    }
}

class Direction
{
    public string PointOfDeparture { get; private set; }
    public string PointOfDestination { get; private set; }
    public int NumberOfPassengers { get; private set; }

    public Direction(string pointOfDeparture, string pointOfDestination)
    {
        PointOfDeparture = pointOfDeparture;
        PointOfDestination = pointOfDestination;
    }

    public void TakePassengers(int numberOfPassengers)
    {
        NumberOfPassengers = numberOfPassengers;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Точка отправления: {PointOfDeparture}\nТочка назначения: {PointOfDestination}\nКуплено билетов: {NumberOfPassengers}");
    }
}

class BoxOffice
{
    public int Ticket { get; private set; }

    public void SellTickets(Direction direction)
    {
        Random random = new Random();
        Ticket = random.Next(80, 300);

        direction.TakePassengers(Ticket);
    }
}

class Train
{
    private int _numberOfseats;
    private List<Wagon> _wagonList = new List<Wagon>();

    public void ShowInfo()
    {

        if (_wagonList.Count > 0)
        {
            int numberOfWagon = 0;

            foreach (Wagon wagon in _wagonList)
            {
                numberOfWagon++;
            }

            Console.WriteLine($"Всего мест в поезде - {_numberOfseats}, количество вагонов {numberOfWagon}");
        }
    }

    public void Form(Direction direction)
    {
        LittleWagon littleWagon = new LittleWagon();
        MiddleWagon middleWagon = new MiddleWagon();
        BigWagon bigWagon = new BigWagon();

        while (direction.NumberOfPassengers > _numberOfseats)
        {
            Console.Clear();
            Console.WriteLine($"|всего мест / пассажиров| {_numberOfseats} / {direction.NumberOfPassengers}\n");
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
        Console.WriteLine($"Мест в поезде достаточно, чтобы отправить рейс(Мест {_numberOfseats} / Пассажиров {direction.NumberOfPassengers})!");
    }

    private void AddLittleWagon()
    {
        _wagonList.Add(new LittleWagon());
        CountNumberOfSeats();
    }

    private void AddMiddleWagon()
    {
        _wagonList.Add(new MiddleWagon());
        CountNumberOfSeats();
    }

    private void AddBigWagon()
    {
        _wagonList.Add(new BigWagon());
        CountNumberOfSeats();
    }

    private void CountNumberOfSeats()
    {
        int sumNumberOfSeats = 0;

        foreach (var wagon in _wagonList)
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
