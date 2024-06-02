namespace Lab4
{
    internal class Program
    {
        static int Counting(string[] args)
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToLower();
                if (args[i] == "-p" || args[i] == "-e" || args[i] == "-a")
                    arguments.Add(args[i], args[++i].ToLower());
                else if (args[i] == "-h")
                {
                    Console.WriteLine("Argument\tExplanation");
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("-h\t\tDisplays the hint about the correct argument entry");
                    Console.WriteLine("-p\t\tThis argument should be followed by the path to the folder in which the files will be counted.");
                    Console.WriteLine("\t\tIf this argument is not specified, the program will work with the folder where the exe file of this program is located.");
                    Console.WriteLine("\t\tFor example, it can be written as \"C:\\Users\\Home\\Desktop\\Test\"");
                    Console.WriteLine("-e\t\tThis argument should be followed by the extension of the files with which the program will work.");
                    Console.WriteLine("\t\tIf this argument is not specified, the program will work with files of any extension.");
                    Console.WriteLine("\t\tFor example, it can be written as \"*.txt\" or as \"?.jpg\"");
                    Console.WriteLine("-a\t\tThis argument should be followed by an attribute of the files the program will work with.");
                    Console.WriteLine("\t\tIf this argument is not specified, the program will work with files with any attributes.");
                    Console.WriteLine("\t\t\"A\" (for an archive file), \"H\" (for a hidden file) or \"R\" (for a read-only file) can be written as an attribute.");
                    Console.WriteLine("Case is not important when entering arguments");
                    return 1;
                }
                else
                {
                    Console.WriteLine("Wrong command line argument");
                    Console.WriteLine("Type \"-h\" as an argument when running the program to see a hint about the correct argument entry");
                    return -1;
                }
            }

            string dir = string.Empty;
            List<string> files = [];
            if (arguments.ContainsKey("-p"))
            {
                dir = arguments["-p"];
                try
                {
                    files = Directory.EnumerateFiles(dir).ToList();
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong folder path");
                    Console.WriteLine("Type \"-h\" as an argument when running the program to see a hint about the correct argument entry");
                    return -2;
                }

            }
            else
            {
                dir = Directory.GetCurrentDirectory();
                files = Directory.EnumerateFiles(dir).ToList();
            }

            if (arguments.ContainsKey("-e"))
                files.RemoveAll(f => !f.ToLower().EndsWith(arguments["-e"].Substring(1, arguments["-e"].Length - 1)));

            if (arguments.ContainsKey("-a"))
            {
                if (arguments["-a"].ToLower().Equals("a"))
                    files.RemoveAll(f => !(File.GetAttributes(f) & FileAttributes.Archive).Equals(FileAttributes.Archive));
                else if (arguments["-a"].ToLower().Equals("h"))
                    files.RemoveAll(f => !(File.GetAttributes(f) & FileAttributes.Hidden).Equals(FileAttributes.Hidden));
                else if (arguments["-a"].ToLower().Equals("r"))
                    files.RemoveAll(f => !(File.GetAttributes(f) & FileAttributes.ReadOnly).Equals(FileAttributes.ReadOnly));
                else
                {
                    Console.WriteLine("Wrong file attribute");
                    Console.WriteLine("Type \"-h\" as an argument when running the program to see a hint about the correct argument entry");
                    return -2;
                }
            }
            
            List<string> folders = Directory.EnumerateDirectories(dir).ToList();
            if (arguments.ContainsKey("-p") && folders.Count != 0)
            {
                for (int i = 1; i < args.Length; i += 2)
                {
                    if (args[i].ToLower().Equals(dir))
                    {
                        foreach (string f in folders)
                        {
                            args[i] = f;
                            Counting(args);
                        }
                        break;
                    }
                }
            }
            else if (folders.Count != 0)
                foreach (string f in folders)
                    Counting(args.Concat(["-p", f]).ToArray());

            Console.WriteLine($"Amount of files in folder {dir}, that sutisfies all conditions: {files.Count}");

            return 0;
        }

        static int Main(string[] args)
        {
            return Counting(args);
        }
    }
}
