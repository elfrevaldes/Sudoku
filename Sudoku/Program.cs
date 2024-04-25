using System;
using System.Drawing;

const short Column = 0;
const short Row = 1;
const short Value = 3;
const short invalidCol = 0;
const short invalidRow = 0;
const short invalidValue = 0;
const short NumRowCol = 9;
short[,] board = new short[NumRowCol, NumRowCol];
initializeBoard(board);
short[,] oriBoard = new short[NumRowCol, NumRowCol];
initializeBoard(oriBoard);
string FileToSave = "";

bool readBoard(string fileName)
{
    // Check if the file exists
    if (File.Exists(fileName))
    {
        try
        {
            // Open the file and create a StreamReader to read from it
            using (StreamReader sr = new StreamReader(fileName))
            {
                bool isSavedBoard = false;
                bool isOriginal = false;
                var numOfRowsToRead = NumRowCol;
                // Read each line of the file
                for (int i = 0; i < numOfRowsToRead; i++)
                {
                    string line = sr.ReadLine();

                    if (line != null)
                    {
                        // It is a save board
                        if (i == 0 && line == "In Progress")
                        {
                            isSavedBoard = true; // Skip first line
                            i--;
                            numOfRowsToRead = NumRowCol+1;
                            continue;
                        }

                        // Do not read this line
                        if (i == 9 && line == "Original" && isSavedBoard)
                        {
                            i = -1;
                            isOriginal = true;
                            numOfRowsToRead = NumRowCol;
                            continue;
                        }

                        if (!isOriginal && i == 0 && line != "Original" && !isSavedBoard)
                            isOriginal = true;

                        // Split the line into individual numbers
                        string[] numbers = line.Split(' ');

                        // Parse and populate the board array
                        for (int j = 0; j < NumRowCol; j++)
                        {
                            // I reading the saved one
                            if (!isOriginal && isSavedBoard)
                            {
                                short possible = short.Parse(numbers[j]);
                                // do not need to save something that it is already there
                                if (possible == 0)
                                    continue;
                                if (isValidMove(board, new Coordinates { Row = (short)i, Column = (short)j, Value = possible }))
                                {
                                    board[i, j] = possible;
                                }
                                else
                                {
                                    Console.WriteLine("Error while reading file: " + fileName + " input: " + possible + " is not correct in: " + j + i);
                                    return false;
                                }
                            }
                            if (isOriginal)
                            {
                                short possible = short.Parse(numbers[j]);
                                // do not need to save something that it is already there
                                if (possible == 0)
                                    continue;
                                if (isValidMove(oriBoard, new Coordinates { Row = (short)i, Column = (short)j, Value = possible }))
                                {
                                    oriBoard[i, j] = possible;
                                }
                                else
                                {
                                    Console.WriteLine("Error while reading file: " + fileName + " input: " + possible + " is not correct in: " + j + i);
                                    return false;
                                }
                            }
                        }
                    }
                }

                // duplicate to play if it has no saved board
                if(isOriginal && !isSavedBoard)
                    copyBoard(oriBoard, board); // Keep a copy for undo

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

void WriteBoard(StreamWriter sw, short[,] board)
{
    for (int i = 0; i < NumRowCol; i++)
    {
        for (int j = 0; j < NumRowCol; j++)
        {
            sw.Write(board[i, j]);
            if (j < NumRowCol - 1)
            {
                sw.Write(" ");
            }
        }
        sw.WriteLine();
    }
}

void SaveFile(string fileName)
{
    try
    {
        using (StreamWriter sw = new StreamWriter(fileName))
        {
            // Write the "In Progress" section
            sw.WriteLine("In Progress");
            WriteBoard(sw, board);

            // Write the "Original" section
            sw.WriteLine("Original");
            WriteBoard(sw, oriBoard);
        }
        Console.WriteLine($"Boards saved to file: {fileName}");
    }
    catch (IOException e)
    {
        Console.WriteLine("An error occurred while writing the file:");
        Console.WriteLine(e.Message);
    }
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

void copyBoard(short[,] copyFrom, short[,] copyTo)
{
    for (int i = 0; i < NumRowCol; i++)
    {
        for (int j = 0; j < NumRowCol; j++)
        {
            copyTo[i,j] = copyFrom[i, j];
        }
    }
}

void resetBoard()
{
    Console.WriteLine("Are you sure you want to reset the board? (You will lose your progress)");
    Console.Write("Type y to reset or n to not reset: ");
    string answer = Console.ReadLine();
    if (answer.ToUpper() == "YES" || answer.ToUpper() == "Y")
    {
        copyBoard(oriBoard, board);
        Console.WriteLine("Board has been reset");
    }
    else
    {
        Console.WriteLine("No changes were made");
    }    
}

bool SaveBoard()
{
    Console.WriteLine("Do you want your progress)");
    Console.Write("Type y to save or n to not save: ");
    string answer = Console.ReadLine();

    if (answer.ToUpper() == "YES" || answer.ToUpper() == "Y")
    {
        if (FileToSave == "")
        {
            FileToSave = "Sudoku";
            string dateTimeFormatted = DateTime.Now.ToString("MM-dd-yy-hh-mm");
            FileToSave = $"{FileToSave}{dateTimeFormatted}.txt";
        }
        SaveFile(FileToSave);
        return true;
    }
    else
    {
        Console.WriteLine("No changes were made");
        return false;
    }
}

void displayBoard()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("   A B C D E F G H I");
    Console.ForegroundColor = ConsoleColor.White;
    for (int row = 0; row < NumRowCol; row++)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(row + 1 + "| ");
        Console.ForegroundColor = ConsoleColor.White;
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
    // is read only
    if (oriBoard[coordinates.Row, coordinates.Column] > 0)
        return false;

    if (!isRowColInRange(coordinates.Row))
        return false;

    if (!isRowColInRange(coordinates.Column))
        return false;

    if(!isInputInRange(coordinates.Value))
        return false;

    return true;
}

bool isRowColInRange(short val) => val is >= 0 and <= 8;
bool isInputInRange(short val) => val is >= 1 and <= 9;

bool isValidCoordFrom(string coordinate, bool displayError = true)
{
    if (coordinate.Length > 4 || coordinate.Length < 3)
    {
        if(displayError)
            Console.WriteLine(coordinate + " is not a valid input");
        return false;
    }
    var newCoor = coordinate.ToUpper();
    short value = 0;
    short row = 0;
    short col = 0;
    // case of shorthand c17
    if (coordinate.Length == 3)
    {
        if (!(short.TryParse(newCoor[Value - 1].ToString(), out value) && isInputInRange(value)))
        {
            if (displayError)
                Console.WriteLine(coordinate[Value - 1] + " is not a valid input");
            return false;
        }
        else
        {
            if (short.TryParse(newCoor[Row].ToString(), out row) && isInputInRange(row) &&
                 newCoor[Column] >= 'A' && newCoor[Column] <= 'I')
                return true;
            else
            {
                if (displayError)
                    Console.WriteLine(coordinate + " column, row or both are not valid");
                return false;
            }
        }
    }
    // regular c1 7
    if (!(short.TryParse(newCoor[Value].ToString(), out value) && isInputInRange(value)))
    {
        if (displayError)
            Console.WriteLine(coordinate[Value] + " is not a valid input");
        return false;
    }
    else
    {
        if(short.TryParse(newCoor[Row].ToString(), out row) && isInputInRange(row) &&
             newCoor[Column] >= 'A' && newCoor[Column] <= 'I')
        {
            return true;
        }
        else
        {
            if (displayError)
                Console.WriteLine(coordinate + " column, row or both are not valid");
            return false;
        }
    }
}
void setAsInvalid(bool[] toSet)
{
    toSet[0] = true; // can't be used as a input
    for(int i = 1; i <= NumRowCol; i++)
    {
        toSet[i] = false;  
    }
}

void initializeBoard(short[,] boardToSet)
{
    for (int i = 0; i < NumRowCol; i++)
        for (int j = 0; j < NumRowCol; j++)
            boardToSet[i,j] = (short)0;
}

bool isValidMove(short[,] boardToValidateIn, Coordinates coor)
{
    // this can be done with only one array but for sake of clarity
    bool[] isInValidRow = new bool[NumRowCol+1];
    setAsInvalid(isInValidRow);
    bool[] isInValidCol = new bool[NumRowCol+1];
    setAsInvalid(isInValidCol);
    bool[] isInValidSquare = new bool[NumRowCol+1];
    setAsInvalid(isInValidSquare);

    // checking row
    for(int col = 0; col < NumRowCol; col++)
    {
        // row stays fixed
        isInValidRow[boardToValidateIn[coor.Row, col]] =  true; // is it used
    }
    // the number is already in the row
    if(isInValidRow[coor.Value])
        return false;

    // checking Column
    for (int row = 0; row < NumRowCol; row++)
    {
        // column stays fixed
        isInValidCol[boardToValidateIn[row, coor.Column]] = true; // is it used
    }
    // the number is already in that column
    if (isInValidCol[coor.Value])
        return false;

    int startingRow = (coor.Row / 3) * 3; // 0 1 2 = 0, 3 4 5 = 3, 6 7 8 = 6
    int startingCol = (coor.Column / 3) * 3;
    int endRow = startingRow + 3;
    int endCol = startingCol + 3;

    for (int row = startingRow; row < endRow; row++)
    {
        for(int col = startingCol; col < endCol; col++) 
        {
            isInValidSquare[boardToValidateIn[row, col]] = true;
        }
    }

    // We need to respond if it is valid
    return !isInValidSquare[coor.Value];
}

Coordinates ParseCoordinate(string coordinate)
{
    if (!isValidCoordFrom(coordinate))
    {
        Console.WriteLine(coordinate + " is not a valid input");
        return new Coordinates() { Column = invalidCol, Row = invalidRow, Value = invalidValue };
    }

    string newCoor = coordinate.ToUpper();
    short row = 0;
    short value = 0;

    if (coordinate.Length == 3)
    {
        short.TryParse(newCoor[Value - 1].ToString(), out value);
    }
    else
    {
        // regular c1 7
        short.TryParse(newCoor[Value].ToString(), out value);   
    }

    short.TryParse(newCoor[Row].ToString(), out row);
    return new Coordinates
    {
        Column = (short)(newCoor[Column] - 'A'), // Substract the value of A 
        Row = (short)(row - 1), // the user use 1 to 9
        Value = value,
    };
}

bool MakeMove(string move)
{
    Coordinates coordinates = ParseCoordinate(move);
    if (isValidCoordinate(coordinates) && isValidMove(board, coordinates))
    {
        Console.WriteLine("Good move! " + coordinates.Value + " in coordinates: " + coordinates.GetCol + coordinates.GetRow + " works");
        board[coordinates.Row, coordinates.Column] = coordinates.Value;
        return true;
    }
    else
    {
        Console.WriteLine("Value: " + coordinates.Value + " in coordinates: " + coordinates.GetCol + coordinates.GetRow + " is invalid");
        return false;
    }
}

Command isCommand(string input)
{
    var capInput = input.ToUpper();
    if (capInput == "QUIT" || capInput == "Q")
        return Command.Quit;

    if (capInput == "READ" || capInput == "READFILE" || capInput == "RF")      
        return Command.ReadFile;

    if (capInput == "SAVE" || capInput == "S")
        return Command.Save;

    if (capInput == "SAVEQUIT" || capInput == "SQ" || capInput == "QS")
    {
        return Command.SaveAndQuit;
    }
    if (capInput == "OPTIONS" || capInput ==  "OPTION" || capInput == "O")
        return Command.DisplayOptions;

    if (capInput == "BOARD" ||  capInput == "B")
        return Command.DisplayBoard;

    if (capInput == "RESET" || capInput == "RESETBOARD" || capInput == "RB")
        return Command.ReloadBoard;
 
    if(isValidCoordFrom(input))
        return Command.Valid;

    return Command.Invalid;
}

void gameIntro()
{
    Console.WriteLine("Welcome to Sudoku:");
    Console.WriteLine("The rules are simple select a column and Row followed by the number to enter");
    Console.WriteLine("Example: c19, C1 9 or c1 9");
    Console.WriteLine("If want to see the options type o or options");
}

void displayOptions()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Options are:");
    Console.WriteLine("-----------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("(c19) Input a change ex. \"c1 2\" or \"c12\"");
    Console.WriteLine("(o)   Display options ex. \"options\", \"option\" or \"o\"");
    Console.WriteLine("(b)   Display board ex. \"board\" or \"b\"");
    Console.WriteLine("(q)   Quit game without save. ex. \"quit\" or \"q\"");
    Console.WriteLine("(s)   Save game. ex. \"save\" or \"s\"");
    Console.WriteLine("(sq)  Save and Quit game. ex. \"savequit\", \"sq\" or \"qs\"");
    Console.WriteLine("(rf)  Read new file with board. ex. \"read\", \"readfile\" or \"rf\"");
    Console.WriteLine("(rb)  Reset board to initial. ex. \"reset\", \"resetboard\" or \"rb\"");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("-----------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
}                     
void menu()            
{
    string answer;     
    Command command;
    do
    {
        Console.Write("Move: ");

        answer = Console.ReadLine();
        command = isCommand(answer);
        switch (command)
        {
            case Command.Quit:
                Console.WriteLine("Thanks for playing");
                break;
            case Command.Invalid:
                Console.WriteLine("That movement is not possible try again");
                break;
            case Command.Valid:
            {
                if (MakeMove(answer))
                {
                    displayOptions();
                    displayBoard();
                }
                    break;
            }
            case Command.DisplayOptions:
                displayOptions();
                break;
            case Command.DisplayBoard:
                displayBoard();
                break;
            case Command.Save:
                SaveBoard();
                break;
            case Command.SaveAndQuit:
            {
                if(SaveBoard())
                {
                    Console.WriteLine("Thanks for playing");
                    command = Command.Quit;
                }
                break;
                }
            case Command.ReadFile:
                Console.WriteLine("Reading Not implemented yet");
                break;
            case Command.ReloadBoard:
                resetBoard();
                break;
            default:
                Console.WriteLine("That is not recognized, try again");
                break;
        }

    } while (command != Command.Quit); 
}


void main()
{
    // \Sudoku\Sudoku\bin\Debug\net8.0\here is where the program is running
    // readBoard("..\\..\\..\\Sudoku.txt");
    readBoard("..\\..\\..\\Board.txt");
    // board[0, 0] = (short)1;
    // displayBoardDebug(oriBoard);
    gameIntro();
    displayOptions();
    displayBoard();
    menu();
}


main();

enum Command
{
    Invalid = 0,
    Valid,
    DisplayOptions,
    DisplayBoard,
    Quit,
    Save, // Not implemented
    SaveAndQuit, // Not implemented
    ReadFile, // Partial needs to ask for file name
    ReloadBoard, // Not implemented
}

public struct Coordinates
{
    public short Column { get; set; }
    public short Row { get; set; }

    public readonly short GetRow => (short)(Row + 1); // because of 0 index

    public readonly char GetCol => (char)(Column + 65);

    public short Value { get; set; }
}