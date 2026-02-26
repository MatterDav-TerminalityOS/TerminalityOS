using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class TerminalityOS
{
    static string osVersion = "v0.1-beta";
    static string currentDirectory = Directory.GetCurrentDirectory();
    static bool skipCurrentCommand = false;

    static readonly Dictionary<char, string[]> bigAsciiFont = new Dictionary<char, string[]>
    {
        {'A', new string[]{"   ███   ","  █   █  "," ██████ "," █    █ "," █    █ ","        "}},
        {'B', new string[]{" █████ "," █    █"," █████ "," █    █"," █████ ","       "}},
        {'C', new string[]{"  ████ "," █     "," █     "," █     ","  ████ ","       "}},
        {'D', new string[]{" ████  "," █   █ "," █    █"," █   █ "," ████  ","       "}},
        {'E', new string[]{" █████ "," █     "," ████  "," █     "," █████ ","       "}},
        {'F', new string[]{" █████ "," █     "," ████  "," █     "," █     ","       "}},
        {'G', new string[]{"  ████ "," █     "," █  ██ "," █    █","  ████ ","       "}},
        {'H', new string[]{" █   █ "," █   █ "," █████ "," █   █ "," █   █ ","       "}},
        {'I', new string[]{" ███ ","  █  ","  █  ","  █  "," ███ ","     "}},
        {'J', new string[]{"   ███ ","    █  ","    █  "," █  █  ","  ██   ","       "}},
        {'K', new string[]{" █   █"," █  █ "," ███  "," █  █ "," █   █","      "}},
        {'L', new string[]{" █     "," █     "," █     "," █     "," █████ ","       "}},
        {'M', new string[]{" █   █ "," ██ ██ "," █ █ █ "," █   █ "," █   █ ","       "}},
        {'N', new string[]{" █   █ "," ██  █ "," █ █ █ "," █  ██ "," █   █ ","       "}},
        {'O', new string[]{"  ███  "," █   █ "," █   █ "," █   █ ","  ███  ","       "}},
        {'P', new string[]{" ████ "," █   █"," ████ "," █    "," █    ","      "}},
        {'Q', new string[]{"  ███  "," █   █ "," █   █ "," █  █  ","  ████ ","       "}},
        {'R', new string[]{" ████ "," █   █"," ████ "," █  █ "," █   █","      "}},
        {'S', new string[]{"  ████ "," █     ","  ███  ","     █ "," ████  ","       "}},
        {'T', new string[]{" █████ ","   █   ","   █   ","   █   ","   █   ","       "}},
        {'U', new string[]{" █   █ "," █   █ "," █   █ "," █   █ ","  ███  ","       "}},
        {'V', new string[]{" █   █ "," █   █ "," █   █ ","  █ █  ","   █   ","       "}},
        {'W', new string[]{" █   █ "," █   █ "," █ █ █ "," ██ ██ "," █   █ ","       "}},
        {'X', new string[]{" █   █ ","  █ █  ","   █   ","  █ █  "," █   █ ","       "}},
        {'Y', new string[]{" █   █ ","  █ █  ","   █   ","   █   ","   █   ","       "}},
        {'Z', new string[]{" █████ ","    █  ","   █   ","  █    "," █████ ","       "}},
        {'0', new string[]{"  ███  "," █  ██ "," █ █ █ "," ██  █ ","  ███  ","       "}},
        {'1', new string[]{"  █  "," ██  ","  █  ","  █  "," ███ ","     "}},
        {'2', new string[]{" ███ ","    █","  █  "," █   "," ████","     "}},
        {'3', new string[]{" ███ ","    █","  ██ ","    █"," ███ ","     "}},
        {'4', new string[]{"   ██ ","  █ █ "," █  █ "," █████","    █ ","      "}},
        {'5', new string[]{" ████"," █   "," ███ ","    █"," ███ ","     "}},
        {'6', new string[]{"  ███"," █   "," ███ "," █  █","  ██ ","     "}},
        {'7', new string[]{" ████","    █","   █ ","  █  ","  █  ","     "}},
        {'8', new string[]{"  ███ "," █   █","  ███ "," █   █","  ███ ","      "}},
        {'9', new string[]{"  ███ "," █   █","  ████","    █ ","  ██  ","      "}},
        {' ', new string[]{"    ","    ","    ","    ","    ","    "}},
        {'!', new string[]{"  █  ","  █  ","  █  ","     ","  █  ","     "}},
        {'.', new string[]{"     ","     ","     ","     ","  █  ","     "}},
        {',', new string[]{"     ","     ","     ","  █  ","  █  "," █   "}},
        {'?', new string[]{" ███ ","    █","  ██ ","     ","  █  ","     "}},
        {'-', new string[]{"     ","     "," ███ ","     ","     ","     "}},
        {'_', new string[]{"     ","     ","     ","     ","     "," ███ "}},
        {'@', new string[]{" █████ ","█     █","█ █ █ █","█     █"," █████ ","       "}},
        {'#', new string[]{" █ █ ","█████"," █ █ ","█████"," █ █ ","     "}},
        {'$', new string[]{"  █  "," ███ ","  █  "," ███ ","  █  ","     "}},
        {'%', new string[]{"██   █","   █ ","  █  "," █   ","█   ██","     "}},
        {'&', new string[]{"  ██ "," █  █","  ██ "," █  █","  ██ ","     "}},
        {'*', new string[]{"     "," █ █ ","  █  "," █ █ ","     ","     "}},
        {'(', new string[]{"  █  "," █   "," █   "," █   ","  █  ","     "}},
        {')', new string[]{"  █  ","   █ ","   █ ","   █ ","  █  ","     "}},
        {'+', new string[]{"     ","  █  "," ███ ","  █  ","     ","     "}},
        {'=', new string[]{"     "," ███ ","     "," ███ ","     ","     "}}
    };

    static void Main()
    {
        Console.Title = "TerminalityOS";
        Console.Clear();

        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true; 
            Console.WriteLine("^C");
            skipCurrentCommand = true;
        };

        BIOSBoot();                
        ClearConsole();            
        PrintAsciiLogoCentered();  
        PrintMiniLineCentered();
        DrawBannerCentered();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("user@");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"TerminalityOS:{currentDirectory}$ ");
            Console.ResetColor();

            string input = Console.ReadLine();
            string command = input?.Trim();
            if (string.IsNullOrWhiteSpace(command)) continue;

            string[] parts = command.Split(' ', 2);
            string cmd = parts[0].ToLower();
            string arg = parts.Length > 1 ? parts[1] : "";

            skipCurrentCommand = false; 

            switch (cmd)
            {
                case "exit":
                    AnimatedWriteLine("Goodbye user!", 5);
                    return;

                case "help":
                    PrintHelp();
                    break;

                case "date":
                    AnimatedWriteLine($"{DateTime.Now:ddd MMM dd HH:mm:ss.fff yyyy} {TimeZoneInfo.Local.StandardName}");
                    break;

                case "version":
                    AnimatedWriteLineColored($"TerminalityOS {osVersion}", ConsoleColor.Cyan, 10);
                    break;

                case "echo":
                    PrintAsciiBigAnimated(arg);
                    break;

                case "ascii":
                    PrintFullAsciiAnimated();
                    break;

                case "ls":
                    ListDirectoryAnimated();
                    break;

                case "cd":
                    ChangeDirectory(arg);
                    break;

                case "clear":
                    ClearConsole();
                    break;

                case "follow":
                    AnimatedWriteLineColored("Follow on TikTok: pyramidlover123minecraft", ConsoleColor.Magenta, 10);
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    AnimatedWriteLine($"bash: {command}: command not found");
                    Console.ResetColor();
                    break;
            }
        }
    }

    // ------------------- BOOT & BANNER -------------------
    static void BIOSBoot()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        AnimatedWriteLine("TerminalityOS BIOS v0.1", 15);

        string[] steps = {
            "[ OK ] Initializing kernel...",
            "[ OK ] Loading system modules...",
            "[ OK ] Checking memory...",
            "[ OK ] Mounting virtual drives...",
            "[ OK ] Starting core services...",
            "[ OK ] Establishing secure environment..."
        };

        foreach (var s in steps)
        {
            Console.WriteLine(s);
            Thread.Sleep(150);
        }

        string bootMsg = "Booting";
        Console.Write(bootMsg);
        for (int i = 0; i < 5; i++)
        {
            Console.Write(".");
            Thread.Sleep(150);
        }
        Thread.Sleep(150);
        Console.ResetColor();
    }

    static void PrintAsciiLogoCentered()
    {
        string[] logo = new string[]
        {
            "████████╗███████╗██████╗ ███╗   ███╗██╗███╗   ██╗ █████╗ ██╗     ██╗████████╗██╗   ██╗",
            "╚══██╔══╝██╔════╝██╔══██╗████╗ ████║██║████╗  ██║██╔══██╗██║     ██║╚══██╔══╝╚██╗ ██╔╝",
            "   ██║   █████╗  ██████╔╝██╔████╔██║██║██╔██╗ ██║███████║██║     ██║   ██║    ╚████╔╝ ",
            "   ██║   ██╔══╝  ██╔══██╗██║╚██╔╝██║██║██║╚██╗██║██╔══██║██║     ██║   ██║     ╚██╔╝  ",
            "   ██║   ███████╗██║  ██║██║ ╚═╝ ██║██║██║ ╚████║██║  ██║███████╗██║   ██║      ██║   ",
            "   ╚═╝   ╚══════╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝╚══════╝╚═╝   ╚═╝      ╚═╝   "
        };
        int width = Console.WindowWidth;
        int topPadding = (Console.WindowHeight - logo.Length - 4) / 2;
        if (topPadding < 0) topPadding = 0;
        for (int i = 0; i < topPadding; i++) Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (var line in logo)
        {
            int pad = (width - line.Length) / 2;
            if (pad < 0) pad = 0;
            Console.WriteLine(new string(' ', pad) + line);
            Thread.Sleep(50);
        }
        Console.ResetColor();
    }

    static void PrintMiniLineCentered()
    {
        string mini = "TERMINALITY OS";
        int width = Console.WindowWidth;
        int pad = (width - mini.Length) / 2;
        if (pad < 0) pad = 0;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(new string(' ', pad) + mini + "\n");
        Console.ResetColor();
    }

    static void DrawBannerCentered()
    {
        string[] banner = new string[]
        {
            "+------------------------------------------------+",
            "|                                                |",
            "|          T E R M I N A L I T Y   O S           |",
            "|            ========================            |",
            "|                                                |",
            "|  Welcome to TerminalityOS v0.1 -beta           |",
            "|  Please note that this OS is just in Terminal  |",
            "|  Type 'help' to see commands                   |",
            "|                                                |",
            "+------------------------------------------------+"
        };
        int width = Console.WindowWidth;
        foreach (var line in banner)
        {
            int pad = (width - line.Length) / 2;
            if (pad < 0) pad = 0;
            Console.WriteLine(new string(' ', pad) + line);
        }
    }

    // ------------------- ANIMATION -------------------
    static void AnimatedWriteLine(string text, int delay = 10)
    {
        foreach (char ch in text)
        {
            if (skipCurrentCommand) break;
            Console.Write(ch);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }

    static void AnimatedWriteLineColored(string text, ConsoleColor color, int delay = 10)
    {
        Console.ForegroundColor = color;
        AnimatedWriteLine(text, delay);
        Console.ResetColor();
    }

    static void AnimatedWriteInline(string text, int delay = 20)
    {
        foreach (char c in text)
        {
            if (skipCurrentCommand) break;
            Console.Write(c);
            Thread.Sleep(delay);
        }
    }

    // ------------------- ASCII -------------------
    static void PrintAsciiBigAnimated(string msg)
    {
        msg = msg.ToUpper();
        string[] words = msg.Split(' ');

        Random rand = new Random();
        int maxWidth = Console.WindowWidth - 2;
        List<string> currentLineWords = new List<string>();
        int currentWidth = 0;

        foreach (var word in words)
        {
            int wordWidth = GetWordWidth(word);
            if (currentWidth + wordWidth >= maxWidth)
            {
                PrintAsciiLine(currentLineWords, rand);
                currentLineWords.Clear();
                currentWidth = 0;
            }
            currentLineWords.Add(word);
            currentWidth += wordWidth + GetWordWidth(" ");
        }
        if (currentLineWords.Count > 0) PrintAsciiLine(currentLineWords, rand);
    }

    static int GetWordWidth(string word)
    {
        int width = 0;
        foreach (char c in word)
            if (bigAsciiFont.ContainsKey(c)) width += bigAsciiFont[c][0].Length + 1;
        return width;
    }

    static void PrintAsciiLine(List<string> words, Random rand)
    {
        for (int i = 0; i < 6; i++)
        {
            foreach (var word in words)
            {
                Console.ForegroundColor = (ConsoleColor)rand.Next(1, 15);
                foreach (char c in word)
                    if (bigAsciiFont.ContainsKey(c)) Console.Write(bigAsciiFont[c][i] + " ");
                Console.Write("  ");
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    static void PrintFullAsciiAnimated()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        AnimatedWriteLine("FULL ASCII DIRECTORY:", 10);
        Console.ResetColor();

        Random rand = new Random();
        List<string> letters = new List<string>();
        foreach (var c in bigAsciiFont.Keys)
            letters.Add(c.ToString());

        List<string> currentLine = new List<string>();
        int maxPerLine = 8;
        foreach (var l in letters)
        {
            currentLine.Add(l);
            if (currentLine.Count >= maxPerLine)
            {
                PrintAsciiLine(currentLine, rand);
                currentLine.Clear();
            }
        }
        if (currentLine.Count > 0)
            PrintAsciiLine(currentLine, rand);
    }

    // ------------------- LS & CD -------------------
    static void ListDirectoryAnimated()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        AnimatedWriteLine($"Listing directory: {currentDirectory}");
        Console.ResetColor();

        var dirs = Directory.GetDirectories(currentDirectory);
        var files = Directory.GetFiles(currentDirectory);
        List<string> items = new List<string>();
        foreach (var d in dirs) items.Add(Path.GetFileName(d) + "/");
        foreach (var f in files) items.Add(Path.GetFileName(f));

        int col = 0;
        int maxCols = 5;

        foreach (var item in items)
        {
            string fullPath = Path.Combine(currentDirectory, item.TrimEnd('/'));
            if (Directory.Exists(fullPath))
                Console.ForegroundColor = ConsoleColor.Cyan;
            else if (Path.GetExtension(item) == ".exe")
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            AnimatedWriteInline(item + "  ", 5);
            col++;
            if (col >= maxCols) { Console.WriteLine(); col = 0; }
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    static void ChangeDirectory(string path)
    {
        string newDir;
        if (string.IsNullOrWhiteSpace(path) || path == "~")
            newDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        else
            newDir = Path.IsPathRooted(path) ? path : Path.Combine(currentDirectory, path);

        if (Directory.Exists(newDir))
        {
            currentDirectory = Path.GetFullPath(newDir);
            Console.ForegroundColor = ConsoleColor.Green;
            AnimatedWriteLine($"Changed directory to: {currentDirectory}", 5);
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            AnimatedWriteLine($"cd: {path}: No such file or directory", 5);
            Console.ResetColor();
        }
    }

    // ------------------- HELP -------------------
    static void PrintHelp()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        string[] helpLines = new string[]{
            "/=======================================\\",
            "|         TERMINALITYOS COMMANDS        |",
            "\\=======================================/",
            "| help   - Show this help message       |",
            "| date   - Show current date and time   |",
            "| clear  - Clear the terminal           |",
            "| echo   - Print ASCII message          |",
            "| ascii  - Show full ASCII letters      |",
            "| ls     - List files & directories     |",
            "| cd     - Change directory             |",
            "| follow - Show TikTok follow message   |",
            "| exit   - Exit the terminal            |",
            "| version- Shows TerminalityOS version  |",
            "\\=======================================/"
        };
        foreach (var line in helpLines)
            Console.WriteLine(line);
        Console.ResetColor();
    }

    static void ClearConsole()
    {
        int width = Console.BufferWidth;
        int height = Console.BufferHeight;

        // Shrink buffer to window height to remove scrollback
        Console.SetBufferSize(width, Console.WindowHeight);

        // Clear visible console
        Console.Clear();

        // Restore original buffer height
        Console.SetBufferSize(width, height);

        // Move cursor to top-left
        Console.SetCursorPosition(0, 0);
    }
}