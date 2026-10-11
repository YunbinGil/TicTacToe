using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Xml;
using System.Xml.Schema;

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
            playAgain = StartGame(modeSelected, oxBoard);
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

    static bool ComputerTurn(char ox, int mode, char[] board)
    {
        Console.WriteLine($"컴퓨터({ox})의 차례입니다.");
        
        int pos=0;
        if (mode == 2)
        {
            pos = GetRandomEmptyPos(board);
            board[pos] = ox;

        }
        Thread.Sleep(1000);
        PrintScreen(mode, board);
        Console.WriteLine($"컴퓨터: {pos+1}번 선택");
        return CheckWinCondition(board, ox);

    }

    static bool CheckWinCondition(char[] board, char ox)
    {
        string boardStr = new string(board);
        string winStr = new string(ox, 3);
        bool isOver = false;

        if (boardStr.Substring(0, 3) == winStr || boardStr.Substring(3, 3) == winStr || boardStr.Substring(6, 3) == winStr
            || $"{board[0]}{board[4]}{board[8]}" == winStr || $"{board[2]}{board[4]}{board[6]}" == winStr)
        {
            isOver = true;
        }

        for (int i = 0; i < 3; i++)
        {
            string colStr = $"{board[i]}{board[i + 3]}{board[i + 6]}";
            if (colStr == winStr)
            {
                isOver = true;
                break;
            }
        }

        return isOver;
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


    static bool PlayerTurn(char ox, int mode, char[] board)
    {
        Console.WriteLine($"플레이어 {ox}의 차례입니다.");

        while (true)
        {
            int pos = ReadIntegerCheck("위치를 선택하세요", 1, 9);

            if (board[pos - 1] == '.')
            {
                board[pos - 1] = ox;
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

    static bool StartGame(int mode, char[] board)
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
                    isOver = ComputerTurn('O', mode, board);
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



