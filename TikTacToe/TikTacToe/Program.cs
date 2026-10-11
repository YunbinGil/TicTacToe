using System;
using System.Linq;
using System.Threading;

class Program
{
    private static Random random = new Random();

    static void Main()
    {
        char[] oxBoard = new char[9];
        int modeSelected = 0;
        int level = 0;
        bool playAgain = true;
        
        while (playAgain)
        {
            Console.Clear();
            SetupGame(oxBoard, out modeSelected, out level);
            playAgain = StartGame(modeSelected, oxBoard, level);
        }

    }
    static void SetupGame(char[] board, out int modeSelected, out int levelSelected)
    {
        Array.Fill(board, '.');
        modeSelected = SelectMode();
        levelSelected = 0;
        if (modeSelected == 2)
        {
            levelSelected = SelectLevel();
        }
    }

    static bool ComputerTurn(char ox, int mode, int level, char[] board)
    {
        Console.WriteLine($"컴퓨터({ox})의 차례입니다.");
        
        int pos=0;
        if (level == 1)
            pos = GetRandomEmptyPos(board);
        else
            pos = GetComputerPos(board);
        board[pos] = ox;
        Thread.Sleep(1000);
        PrintScreen(mode, board);
        Console.WriteLine($"컴퓨터: {pos+1}번 선택");
        return CheckWinCondition(board, ox);

    }

    static bool CheckWinCondition(char[] board, char ox)
    {
        string[] boardStrs = GetStringsFromBoard(board);
        string winStr = new string(ox, 3);
        bool isOver = false;
        
        for(int i = 0; i<boardStrs.Length; i++)
        {
            if (boardStrs[i] == winStr) isOver = true;
        }
        

        return isOver;
    }

    static int GetComputerPos(char[] board)
    {
        string[] possible = { ".OO", "O.O", "OO." };
        string[] blockable = { ".XX", "X.X", "XX." };
        int n = -1; 
        string[] strings = GetStringsFromBoard(board);

        //1. 컴퓨터 O가 바로 승리할 수 있는 칸
        n = GetPos(strings, possible);
        if (n >= 0) return n;

        //2.다음수에 X가 바로 승리할 수 있는 칸을 막기
        n = GetPos(strings, blockable);
        if (n >= 0) return n;

        //3. 5번 비어있으면 5
        if (board[4] == '.') return 4;

        //4. 모서리(1,3,7,9) 중에
        for (int i = 1; i < 10; i+=2)
        {
            if (board[i] == '.') return i;
        }
        //5. 남은 빈칸 중 하나.
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == '.') return i;
        }

        return n;
    }

    static int GetPos(string[] boardstrings, string[] checkStrings)
    {
        int stringIndex = -1;
        int shape = -1;
        for (int i = 0; i < boardstrings.Length; i++)
        {
            if (checkStrings.Contains(boardstrings[i]))
            {
                stringIndex = i;
                string matcehdStr = boardstrings[i];
                    for (int j = 0; j < 3; j++)
                {
                    if (matcehdStr[j] == '.')
                    {
                        shape = j;
                        break;
                    }
                }
                break;
            }
        }
        if (stringIndex < 3)
        {
            return stringIndex * 3 + shape;
        }
        else if (stringIndex < 6)
        {
            return shape * 3 + (stringIndex - 3);
        }
        else if (stringIndex == 6)
        {
            return shape * 4;
        }
        else if (stringIndex == 7)
        {
            return (shape + 1) * 2;
        }
        return -1;
    }
    static int GetRandomEmptyPos(char[] board)
    {
        int n = random.Next(board.Length);
        while (board[n] != '.')
        {
            n = random.Next(board.Length);
        }
        return n;
    }
    static string[] GetStringsFromBoard(char[] board)
    {
        string str = new string(board);
        string[] strings = new string[8];
        for (int i = 0; i < 3; i++)
        {
            strings[i] = str.Substring(i*3, 3);
            strings[i + 3] = $"{board[i]}{board[i + 3]}{board[i+6]}";
        }
        strings[6] = $"{board[0]}{board[4]}{board[8]}";
        strings[7] = $"{board[2]}{board[4]}{board[6]}";

        return strings;
    }


    static bool PlayerTurn(char ox, int mode, char[] board)
    {
        Console.WriteLine($"플레이어 {ox}의 차례입니다.");

        while (true)
        {
            int pos = ReadIntegerCheck("위치를 선택하세요", 1, 9)-1;

            if (board[pos] == '.')
            {
                board[pos] = ox;
                break;
            }
            else Console.WriteLine("이미 선택한 칸입니다 빈칸을 선택하세요.");
        }

        PrintScreen(mode, board);

        return CheckWinCondition(board, ox);
       
    }

    static int ReadIntegerCheck(string msg, int startRange, int endRange)
    {
        int res = -1;
        bool isParsed = false;
        while (!isParsed || res < startRange || res > endRange)
        {
            Console.Write($"{msg} ({startRange}~{endRange}): ");
            isParsed = int.TryParse(Console.ReadLine(), out res);
        }
        return res;
    }
    static int SelectLevel()
    {
        string str = @"=== 난이도 ===
1) 쉬움
2) 어려움
==============
";

        Console.WriteLine("\n틱택토 게임 - 컴퓨터 대전");
        Console.Write(str);

        int level = ReadIntegerCheck("난이도를 선택하세요", 1, 2);

        return level;
    }
    static int SelectMode()
    {
        string str = @"=== 모드 ===
1) 두 사람 대전  
2) 컴퓨터 대전
==============
";


        Console.WriteLine("틱택토 게임");
        Console.Write(str);

        return ReadIntegerCheck("모드를 선택하세요", 1, 2);

    }

    static bool StartGame(int mode, char[] board, int level)
    {
        PrintScreen(mode, board);
        bool isOver = false;
        int turn = 0;

        for (turn = 0; turn < 9; turn++)
        {
            if (turn % 2 == 0)
            {
                isOver = PlayerTurn('X', mode, board);
            }
            else
            {
                if (mode == 1)
                    isOver = PlayerTurn('O', mode, board);
                else if (mode == 2)
                    isOver = ComputerTurn('O', mode, level, board);
                ;
            }
            if (isOver) break;
        }

        PrintScreen(mode, board);
        string winner = turn % 2 == 0 ? "X" : "O";
        if (!isOver)
            Console.WriteLine("무승부!");
        else if (mode == 1)
        {
            Console.WriteLine($"플레이어 {winner} 승리 !");
        }
        else if (mode == 2)
        {
            if (winner == "X")
            {
                Console.WriteLine($"플레이어 {winner} 승리 !");
            }
            else
            {
                Console.WriteLine("컴퓨터 승리!");
            }
        }
        Console.Write("새 게임을 시작하시겠습니까? (Y/N): ");
        string input = Console.ReadLine().Trim().ToUpper();
        while (input.Length > 1 || (input!="Y" && input != "N"))
        {
            Console.Write("새 게임을 시작하시겠습니까? (Y/N):");
            input = Console.ReadLine().Trim().ToUpper();
        }
        if (input == "Y") return true;
        else return false;
    }

    static void PrintScreen(int mode, char[] board)
    {
        Console.Clear();
        Console.WriteLine($"=== 틱택토 게임·{(mode==1?"두 사람 대전":"컴퓨터대전")} ===");
        if(mode ==1)
            Console.WriteLine("두 사람이 X와 O를 번갈아 놓습니다. X가 먼저 시작합니다.");
        else
            Console.WriteLine("플레이어와 컴퓨터가 X와 O를 번갈아 놓습니다. X(플레이어)가 먼저 시작합니다.");
        PrintBoard("123456789".ToCharArray(), "위치 번호");
        PrintBoard(board, "현재 보드");
    }

    static void PrintBoard(char[] board, string msg)
    {
        string str = @$"{msg}:
{board[0]} | {board[1]} | {board[2]}
---------
{board[3]} | {board[4]} | {board[5]}
---------
{board[6]} | {board[7]} | {board[8]}
";
        Console.WriteLine(str);
    }
}



