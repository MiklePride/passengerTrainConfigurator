class program
{
    static void Main(string[] args)
    {
        BoxOffice boxOffice = new BoxOffice();
        Operator tripOperator = new Operator();

        bool isWork = true;
        const int Key1 = 1, Key2 = 2, Key3 = 3, Key4 = 4, Key5 = 5;

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

            int userNumber = UserUtils.GetNumber();

            switch (userNumber)
            {
                case Key1:
                    tripOperator.CreateDirection();
                    break;
                case Key2:
                    tripOperator.TakePassengersToDirection(boxOffice);
                    break;
                case Key3:
                    tripOperator.FormTrain();
                    break;
                case Key4:
                    tripOperator.SendTrip();
                    break;
                case Key5:
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
    private Dictionary<int, string> _cities = new Dictionary<int, string>();
    private List<Trip> _trips = new List<Trip>();
    private Direction _direction;
    private Train _train;

    private bool _hasDirectionBeenCreated;
    private bool _haveTicketsBeenSold;
    private bool _isTrainFormed;

    private int _countOfPassengers;

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
            _countOfPassengers = boxOffice.SellTickets();
            _haveTicketsBeenSold = true;
            Console.WriteLine($"На направление: {_direction.PointOfDeparture} - {_direction.PointOfDestination} купили {_countOfPassengers} билетов.");
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
            _train.Form(_countOfPassengers);
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
            _trips.Add(new Trip(_direction, _train, _countOfPassengers));
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

    

    private string GetCity()
    {
        bool isCity = false;
        string nameCity = "";

        while (isCity == false)
        {
            int numberCity = UserUtils.GetNumber();

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
    public int Ticket { get; private set; }

    public int SellTickets()
    {
        Random random = new Random();
        Ticket = random.Next(80, 300);

        return Ticket;
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

            int userInput = UserUtils.GetNumber();
            const int Key1 = 1, Key2 = 2, Key3 = 3;

            switch (userInput)
            {
                case Key1:
                    AddLittleWagon();
                    break;
                case Key2:
                    AddMiddleWagon();
                    break;
                case Key3:
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
