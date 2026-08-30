namespace MSDOSRemake
{
    public class Program
    {
        static bool running = false;
        static string currentDir = @"Z:\";
        static readonly string DOSDir = Directory.GetCurrentDirectory() + "\\";
        static string realDir = Directory.GetCurrentDirectory() + "\\";
        public static void Main()
        {
            Console.WriteLine("Starting MS-DOS...");
            Console.WriteLine("MS-DOS Remake (Version 1.0) by JustACasualGamer on GitHub.");
            running = true;
            
            while (running)
            {
                Console.Write(currentDir + ">");
                string? input = Console.ReadLine();
                if (input is null)
                {
                    continue;
                } else
                {
                    string[] command = input.Split(" ");
                    switch (command[0].ToLower())
                    {
                        case "echo":
                            string fulltext = string.Join(" ", command[1..]);
                            if (fulltext.Contains('>'))
                            {
                                int indexOfRedir = fulltext.IndexOf('>');
                                string textToRedir = fulltext[0..indexOfRedir];
                                string redirFile = fulltext[(indexOfRedir+1)..].Trim();
                                try
                                {
                                    File.WriteAllText(redirFile, textToRedir);
                                } catch (PathTooLongException)
                                {
                                    Console.WriteLine("The path is too long.");
                                } catch (ArgumentException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (DirectoryNotFoundException)
                                {
                                    Console.WriteLine("The specified path is invalid.");
                                } catch (UnauthorizedAccessException)
                                {
                                    Console.WriteLine("Access is denied.");
                                } catch (IOException)
                                {
                                    Console.WriteLine("The specified file is being used by another process or is blocked.");
                                }
                            }
                            break;
                        case "more":
                            if (command.Length > 1)
                            {
                                try
                                {
                                    Console.WriteLine($"Reading file {command[1]}. Press any key to read next line.");
                                    foreach (string i in File.ReadLines(command[1])) {
                                        Console.WriteLine(i);
                                        Console.ReadKey(true);
                                    }
                                } catch (FileNotFoundException)
                                {
                                    Console.WriteLine("File not found: " + command[1]);
                                } catch (ArgumentException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (DirectoryNotFoundException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (PathTooLongException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (UnauthorizedAccessException)
                                {
                                    Console.WriteLine("Access is denied.");
                                } catch (IOException)
                                {
                                    Console.WriteLine("An error occurred.");
                                }
                            } else
                            {
                                Console.WriteLine("The syntax of the command is incorrect.");
                            }
                            break;
                        case "type":
                            if (command.Length > 1)
                            {
                                try
                                {
                                    foreach (string i in File.ReadLines(command[1])) {
                                        Console.WriteLine(i);
                                    }
                                } catch (FileNotFoundException)
                                {
                                    Console.WriteLine("File not found: " + command[1]);
                                } catch (ArgumentException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (DirectoryNotFoundException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (PathTooLongException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (UnauthorizedAccessException)
                                {
                                    Console.WriteLine("Access is denied.");
                                } catch (IOException)
                                {
                                    Console.WriteLine("An error occurred.");
                                }
                            } else
                            {
                                Console.WriteLine("The syntax of the command is incorrect.");
                            }
                            break;
                        case "md" or "mkdir":
                            if (command.Length > 1)
                            {
                                foreach (string folderToCreate in command[1..])
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(folderToCreate);
                                    } catch (UnauthorizedAccessException)
                                    {
                                        Console.WriteLine("Access is denied.");
                                    } catch (DirectoryNotFoundException)
                                    {
                                        Console.WriteLine("The specified path is invalid.");
                                    } catch (PathTooLongException)
                                    {
                                        Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                    } catch (IOException)
                                    {
                                        Console.WriteLine($"A directory or file {folderToCreate} already exists.");
                                    } catch (ArgumentException)
                                    {
                                      Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                    }
                                }
                            } else
                            {
                                Console.WriteLine("The syntax of the command is incorrect.");
                            }
                            break;
                        case "dir":
                            foreach (string item in Directory.GetDirectories(realDir))
                            {
                                Console.WriteLine($"[DIR] {item}");
                            }
                            foreach (string item in Directory.GetFiles(realDir))
                            {
                                string[] itemPath = item.Split("\\");
                                string itemName = itemPath[^1];
                                Console.WriteLine($"[FILE] {itemName}");
                            }
                            break;
                        case "rmdir" or "rd":
                            if (command.Length > 1)
                            {
                                try
                                {
                                    Directory.Delete(command[1]);
                                } catch (DirectoryNotFoundException)
                                {
                                    Console.WriteLine("No directory found.");
                                } catch (PathTooLongException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (UnauthorizedAccessException)
                                {
                                    Console.WriteLine("Access is denied.");
                                } catch (ArgumentException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (IOException)
                                {
                                    Console.WriteLine("The directory is not empty.");
                                }
                            } else
                            {
                                Console.WriteLine("The syntax of the command is incorrect.");
                            }
                            break;
                        case "cd":
                            if (command.Length > 1)
                            {
                                try
                                {
                                    if (string.Join(" ", command[1..]) != ".." && string.Join(" ", command[1..]) != ".")
                                    {
                                        Directory.SetCurrentDirectory(realDir + string.Join(" ", command[1..]));
                                        currentDir += string.Join(" ", command[1..]);
                                        realDir += string.Join(" ", command[1..]);
                                    } else if (string.Join(" ", command[1..]) == "..")
                                    {
                                        if (currentDir != "Z:\\")
                                        {
                                            string[] direcs = currentDir.Split("\\");
                                            string[] realDirecs = realDir.Split("\\");
                                            direcs[^1] = "";
                                            realDirecs[^1] = "";
                                            Directory.SetCurrentDirectory(string.Join("\\", direcs));
                                            currentDir = string.Join("\\", direcs);
                                            realDir += string.Join("\\", realDirecs);
                                        }
                                    }
                                } catch (DirectoryNotFoundException)
                                {
                                    Console.WriteLine("No directory found.");
                                } catch (PathTooLongException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (UnauthorizedAccessException)
                                {
                                    Console.WriteLine("Access is denied.");
                                } catch (ArgumentException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                }
                            } else
                            {
                                Console.WriteLine(currentDir[^1]);
                            }
                            
                            break;
                        case "help":
                            Console.WriteLine("List of commands:\nHELP  Displays this help message.\nMKDIR/MD   Makes a new directory.\nRMDIR/RD    Removes an empty directory. Directory must be empty.\nECHO  Echoes text on the screen.\nMORE    Reads text from a file.\nCD     Changes the current working directory.");
                            break;
                        case "exit":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Bad command: " + command[0]);
                            break;
                    }
                }
            }
        }
    }
}