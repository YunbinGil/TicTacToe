using System;

class Program
{
    static void Main()
    {
        char[] oxBoard = ".........".ToCharArray();

        int levelSelected = SelectDifficulty();
        StartGame(levelSelected, oxBoard);

        bool isOver = false;
        int turn = 0;

        for (turn = 0; turn < 9; turn++)
        {
            if (turn % 2 == 0)
            {
                isOver = Turn('X', levelSelected, oxBoard);
            }
            else
            {
                isOver = Turn('O', levelSelected, oxBoard);
            }
            if (isOver) break;
        }

        PrintScreen(levelSelected, oxBoard);
        Console.WriteLine(isOver ? $"플레이어 {(turn % 2 == 0 ? "X" : "O")} 승리 !" : "무승부");
        Console.WriteLine("종료하려면 Enter를 누르세요.");
        Console.ReadLine();
    }



    static bool Turn(char ox, int level, char[] board)
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

        PrintScreen(level, board);


        // ------- 승패확인 ---------
        string boardStr = board.ToString();
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

    static int SelectDifficulty()
    {
        string str = @"=== 난이도 ===
1) 기본 - 두 사람이 번갈아 플레이하는 게임
2) 응용 - 무작위로 두는 컴퓨터와 대전
3) 도전 - 승리와 방어를 판단하는 컴퓨터와 대전
==============
";


        Console.WriteLine("틱택토 게임");
        Console.Write(str);

        int level = ReadIntegerCheck("난이도를 선택하세요", 1, 3);

        return level;

    }

    static void StartGame(int level, char[] board)
    {
        PrintScreen(level, board);
    }

    static void PrintScreen(int level, char[] board)
    {
        Console.Clear();
        Console.WriteLine($"=== 틱택토 게임·{level}단계 ===");
        Console.WriteLine("두 사람이 X와 O를 번갈아 놓습니다. X가 먼저 시작합니다.");
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



