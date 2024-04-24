using System;
using System.Drawing;

const short Column = 0;
const short Row = 1;
const short Value = 3;
const char invalidCol = 'X';
const short invalidRow = 0;
const short invalidValue = 0;
const short NumRowCol = 9;
short[,] board = new short[NumRowCol, NumRowCol];
short[,] oriBoard = new short[NumRowCol, NumRowCol];

void gameIntro()
{
    Console.WriteLine("Welcome to Sudoku:");
    Console.WriteLine("The rules are simple select a column by saying c or C and a number follow by");
    Console.WriteLine("the row r or R follow by a number and then the number desired in that spot ");
    Console.WriteLine("Example: c1 9 or C1 9");
    Console.WriteLine("If you want to quit the game just type quit or q and enter");
}

bool readBoard(string fileName)
{
    /* debugging
    string currentDirectory = Directory.GetCurrentDirectory();
    Console.WriteLine($"Current Directory: {currentDirectory}");
    
    string[] filesInDirectory = Directory.GetFiles(currentDirectory);
    Console.WriteLine("Files in current directory:");
    foreach (string file in filesInDirectory)
    {
        Console.WriteLine(file);
    }
    end debug */

    // Check if the file exists
    if (File.Exists(fileName))
    {
        try
        {
            // Open the file and create a StreamReader to read from it
            using (StreamReader sr = new StreamReader(fileName))
            {
                // Read each line of the file
                for (int i = 0; i < NumRowCol; i++)
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        // Split the line into individual numbers
                        string[] numbers = line.Split(' ');

                        // Parse and populate the board array
                        for (int j = 0; j < NumRowCol; j++)
                        {
                            board[i, j] = short.Parse(numbers[j]);
                        }
                    }
                }
                copyBoard(board); // Keep a copy for undo
                return true; // file was read correctly
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("An error occurred while reading the file:");
            Console.WriteLine(e.Message);
        }
    }
    else
    {
        Console.WriteLine($"File '{fileName}' does not exist.");
    }
    return false; // we could not read the file
}

void displayBoardDebug(short[,] boardToDisplay)
{
    Console.WriteLine("Board Contents:");
    for (int i = 0; i < NumRowCol; i++)
    {
        for (int j = 0; j < NumRowCol; j++)
        {
            Console.Write(boardToDisplay[i, j] + " ");
        }
        Console.WriteLine();
    }
}

void copyBoard(short[,] boardToCopy)
{
    for (int i = 0; i < NumRowCol; i++)
    {
        for (int j = 0; j < NumRowCol; j++)
        {
            oriBoard[i,j] = boardToCopy[i, j];
        }
    }
}

void displayBoard()
{
    Console.WriteLine("   A B C D E F G H I");
    for (int row = 0; row < NumRowCol; row++)
    {
        Console.Write(row + 1 + "| ");
        for (int col = 0; col < NumRowCol; col++)
        {
            // do not display 0
            if (board[row, col] == 0)
            {
                Console.Write("  ");
            }
            else
            {
                if (oriBoard[row, col] != 0)
                { Console.ForegroundColor = ConsoleColor.Blue; }
                Console.Write(board[row, col] + " ");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

void tString(string t)
{
    Console.WriteLine("Value: " + t);
}

bool isValidCoordinate(Coordinates coordinates)
{
    if(coordinates.Column == invalidCol || coordinates.Row == invalidRow || coordinates.Value == invalidValue)
        return false;

    if (coordinates.Column <= 'A' || coordinates.Column >= 'I')
        return false;

    if(coordinates.Value <= 1 || coordinates.Value >= 9)
        return false;

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
    // \Sudoku\Sudoku\bin\Debug\net8.0\here is where the program is running
    readBoard("..\\..\\..\\board.txt");
    board[0, 0] = (short)1;
    // displayBoardDebug(oriBoard);
    displayBoard();
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