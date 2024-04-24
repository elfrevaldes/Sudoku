using System;

const short Row = 0;
const short uRow = 1;
const short Column = 2;
const short uColumn = 3;


void gameIntro()
{
    Console.WriteLine("Welcome to Sudoku:");
    Console.WriteLine("The rules are simple select a column by saying c or C and a number follow by");
    Console.WriteLine("the row r or R follow by a number and then the number desired in that spot ");
    Console.WriteLine("Example: c1r1 9 or C1R1 9");
    Console.WriteLine("If you want to quit the game just type quit or q and enter");
}

void tString(string t)
{
    Console.WriteLine("Value: " + t);
}
bool isValidCoordinate(Coordinates coordenadas)
{
    if(coordenadas.Column == 0 && coordenadas.Row == 0 && coordenadas.Value == 0)
    {
        return false;
    }
    return true;
}

Coordinates ParseCoordinate(string coordinate)
{
    if (coordinate.Length >= 5)
    {
        Console.WriteLine(coordinate + " is not a valid input");
        return new Coordinates() { Column = 0, Row = 0, Value = 0 };
    }

    string newCoor = coordinate.ToUpper();
    tString(newCoor);
    tString(newCoor[0].ToString());
    tString(newCoor[1].ToString());
    // Check if letters are correct
    if (newCoor[0] == 'C' && newCoor[1] == 'R')
    {
        short row;
        short column;

        if (short.TryParse(newCoor.Substring(2, 2), out column) && short.TryParse(newCoor.Substring(4, 2), out row))
        {
            short value;
            if (short.TryParse(newCoor.Substring(6), out value))
            {
                return new Coordinates() { Column = column, Row = row, Value = value };
            }
            else
            {
                Console.WriteLine(coordinate + " has an invalid value");
            }
        }
        else
        {
            Console.WriteLine(coordinate + " has an invalid column or row");
        }
    }
    else
    {
        Console.WriteLine(coordinate + " has an invalid format");
    }

    return new Coordinates() { Column = 0, Row = 0, Value = 0 };
}

void menu()
{
    string answer;
    bool quit = false;
    do
    {
        Console.Write("Move: ");

        answer = Console.ReadLine();
        if (answer == "quit" || answer == "q")
        {
            Console.WriteLine("Thanks for playing");
            quit = true;
        }
        if(!quit)
        {
            Coordinates coordinates = ParseCoordinate(answer);
            if(isValidCoordinate(coordinates))
            {
                Console.WriteLine("Good coordinates: " + coordinates + " Let's play!");
            }
            else
            {
                Console.WriteLine("That movement is not possible try again");
            }
        }

    } while (!quit); 
}

void main()
{
    gameIntro();
    menu();
}


main();


public struct Coordinates
{
    public short Column { get; set; }
    public short Row { get; set; }
    public short Value { get; set; }
}