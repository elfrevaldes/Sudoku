using System;
using System.Drawing;

const short Column = 0;
const short Row = 1;
const short Value = 3;
const char invalidCol = 'X';
const short invalidRow = 0;
const short invalidValue = 0;



void gameIntro()
{
    Console.WriteLine("Welcome to Sudoku:");
    Console.WriteLine("The rules are simple select a column by saying c or C and a number follow by");
    Console.WriteLine("the row r or R follow by a number and then the number desired in that spot ");
    Console.WriteLine("Example: c1 9 or C1 9");
    Console.WriteLine("If you want to quit the game just type quit or q and enter");
}

void tString(string t)
{
    Console.WriteLine("Value: " + t);
}
bool isValidCoordinate(Coordinates coordenadas)
{
    if(coordenadas.Column == invalidCol || coordenadas.Row == invalidRow || coordenadas.Value == invalidValue)
    {
        return false;
    }
    return true;
}

Coordinates ParseCoordinate(string coordinate)
{
    tString(coordinate.Length.ToString());
    if (coordinate.Length != 4)
    {
        Console.WriteLine(coordinate + " is not a valid input");
        return new Coordinates() { Column = invalidCol, Row = invalidRow, Value = invalidValue };
    }

    string newCoor = coordinate.ToUpper();
    short row = 0;
    short value = 0;


    // Check if letters are correct
    if (newCoor[Column] >= 'A' && newCoor[Column] <= 'I')
    {
        if (short.TryParse(newCoor[Row].ToString(), out row) && row >= 1 && row <= 9)
        {
            if (short.TryParse(newCoor[Value].ToString(), out value) && value >= 1 && value <= 9)
            {
                return new Coordinates
                {
                    Column = newCoor[Column],
                    Row = row,
                    Value = value,
                };
            }
            else
            {
                Console.WriteLine(coordinate + " has an invalid value");
            }
        }
        else
        {
            Console.WriteLine(coordinate + " has an invalid row");
        }
    }
    else
    {
        Console.WriteLine(coordinate + " has an invalid column");
    }
   

    return new Coordinates() { Column = invalidCol, Row = invalidRow, Value = invalidValue };
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
    public char Column { get; set; }
    public short Row { get; set; }
    public short Value { get; set; }
}