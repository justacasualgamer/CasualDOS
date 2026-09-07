#pragma warning disable CA1862
using System.ComponentModel;
using System.Diagnostics;

namespace CasualDOS
{
    public class Program
    {
        static bool running = false;
        static string currentDir = @"Z:\";
        static readonly string DOSDir = Directory.GetCurrentDirectory() + "\\";
        static string realDir = Directory.GetCurrentDirectory() + "\\";
        static Dictionary<string, string> variables = new();
        static readonly string __version__ = "CasualDOS [Version 2.0]";
        static readonly string __changelogs__ = @"Version 2.0 changelogs
- Added this changelogs option.
- Added copy (copy con will be added in a later update).
- Added variables (set and %variable% for echo)
- 480 lines
- Extra line just for good measure
- Extra line just for good measure
- Extra line just for good measure";
        public static void Main()
        {
            Console.WriteLine(__version__);
            Console.WriteLine("Project at https://github.com/justacasualgamer/MS-DOS.");
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
                        case "ver":
                            Console.WriteLine(__version__);
                            break;
                        case "changelogs":
                            Console.WriteLine(__changelogs__);
                            break;
                        case "echo":
                            string fulltext = string.Join(" ", command[1..]);
                            // ah shit, here comes the variable checks (super annoying to make)
                            if (fulltext.Contains('%'))
                            {
                                string modifiedText = "";
                                string proposedVarKey = "";
                                string test = fulltext;
                                if (fulltext.Contains('>')) test = fulltext[..fulltext.IndexOf('>')];
                                for (int i=0; i<test.Length; i++)
                                {
                                    proposedVarKey = "";
                                    if (fulltext[i] != '%') modifiedText += fulltext[i];
                                    else
                                    {
                                        bool err = false;
                                        i++;
                                        try
                                        {
                                            for (; fulltext[i] != '%'; i++) proposedVarKey += fulltext[i];
                                        } catch (IndexOutOfRangeException)
                                        {
                                            err = true;
                                        }
                                        if (!err) modifiedText += variables.GetValueOrDefault(proposedVarKey, $"%{proposedVarKey}%");
                                        else modifiedText += $"%{proposedVarKey}";
                                    }
                                }
                                if (fulltext.Contains('>')) fulltext = modifiedText + fulltext[fulltext.IndexOf('>')..]; 
                                else fulltext = modifiedText;
                            }
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
                            } else
                            {
                                Console.WriteLine(fulltext);
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
                                string[] itemPath = item.Split("\\");
                                string itemName = itemPath[^1];
                                Console.WriteLine($"[DIR] {itemName}");
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
                        case "cd" or "chdir":
                            if (command.Length > 1)
                            {
                                try
                                {
                                    if (string.Join(" ", command[1..]) != ".." && string.Join(" ", command[1..]) != ".")
                                    {
                                        Directory.SetCurrentDirectory(realDir + string.Join(" ", command[1..]));
                                        currentDir += string.Join(" ", command[1..]) + "\\";
                                        realDir += string.Join(" ", command[1..]) + "\\";
                                    } else if (string.Join(" ", command[1..]) == "..")
                                    {
                                        if (currentDir != "Z:\\")
                                        {
                                            string[] direcs = currentDir.Split("\\");
                                            string[] realDirecs = realDir.Split("\\");
                                            Directory.SetCurrentDirectory(string.Join("\\", realDirecs[..^2]));
                                            currentDir = string.Join("\\", direcs[..^2]) + "\\";
                                            realDir = string.Join("\\", realDirecs[..^2]) + "\\";
                                        }
                                    }
                                } catch (DirectoryNotFoundException)
                                {
                                    try
                                    {
                                        string[] intendedDirArray = string.Join(" ", command[1..]).Split("\\");
                                        if (intendedDirArray[0] == "Z:")
                                        {
                                            intendedDirArray[0] = "";
                                        } else
                                        {
                                            throw new DirectoryNotFoundException();
                                        }
                                        string intendedDir = DOSDir + string.Join("\\", intendedDirArray);
                                        Directory.SetCurrentDirectory(intendedDir);
                                    } catch (DirectoryNotFoundException)
                                    {
                                        Console.WriteLine("No directory found.");
                                    }
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
                                    try
                                    {
                                        string[] intendedDirArray = string.Join(" ", command[1..]).Split("\\");
                                        if (intendedDirArray[0] == "Z:")
                                        {
                                            intendedDirArray[0] = "";
                                        } else
                                        {
                                            throw new DirectoryNotFoundException();
                                        }
                                        string intendedDir = DOSDir + string.Join("\\", intendedDirArray);
                                        Directory.SetCurrentDirectory(intendedDir);
                                    } catch (DirectoryNotFoundException)
                                    {
                                        Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                    }
                                }
                            } else
                            {
                                Console.WriteLine(currentDir);
                            }
                            break;
                        case "del":
                            foreach (string i in command[1..])
                            {
                                try
                                {
                                    if (File.Exists(i))
                                    {
                                        File.Delete(i);
                                    } else
                                    {
                                        throw new DirectoryNotFoundException();
                                    }
                                } catch (ArgumentException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (DirectoryNotFoundException)
                                {
                                    Console.WriteLine("The specified path is invalid.");
                                } catch (UnauthorizedAccessException)
                                {
                                    Console.WriteLine("Access is denied.");
                                } catch (PathTooLongException)
                                {
                                    Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                                } catch (IOException)
                                {
                                    Console.WriteLine("An unknown error occurred.");
                                }
                            }
                            break;
                        case "copy":
                            try
                            {
                                File.Copy(command[1], command[2]);
                            } catch (UnauthorizedAccessException)
                            {
                                Console.WriteLine("Access is denied.");
                            } catch (ArgumentException)
                            {
                                Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                            } catch (NotSupportedException)
                            {
                                Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                            } catch (PathTooLongException)
                            {
                                Console.WriteLine("The file name, directory name, or volume label syntax is incorrect.");
                            } catch (DirectoryNotFoundException)
                            {
                                Console.WriteLine("The specified path is invalid.");
                            } catch (FileNotFoundException)
                            {
                                Console.WriteLine("The specified path is invalid.");
                            } catch (IOException)
                            {
                                Console.WriteLine("An unknown error occurred.");
                            }
                            break;
                        case "set":
                            try
                            {
                                string varKey = input[4..input.IndexOf('=')];
                                string varValue = input[(input.IndexOf('=') + 1)..];
                                variables[varKey] = varValue;
                            } catch (ArgumentOutOfRangeException)
                            {
                                if (input.Trim().ToLower()=="set")
                                {
                                    foreach (string key in variables.Keys)
                                    {
                                        Console.WriteLine($"{key}={variables.GetValueOrDefault(key, "")}");
                                    }
                                }
                            } catch (ArgumentNullException)
                            {
                                Console.WriteLine("The syntax of the command is incorrect.");
                            }
                            break;
                        case "start":
                            try
                            {
                                Process process = new()
                                {
                                    StartInfo = new ProcessStartInfo()
                                    {
                                        FileName = command[1],
                                        Arguments = string.Join(" ", command[2..])
                                    }
                                };
                                process.Start();
                            } catch (InvalidOperationException)
                            {
                                Console.WriteLine("Specify a program.");
                            } catch (Win32Exception w32ex)
                            {
                                if (w32ex.NativeErrorCode == 2)
                                {
                                    Console.WriteLine("The system cannot find the file specified.");
                                } else if (w32ex.NativeErrorCode == 3)
                                {
                                    Console.WriteLine("The specified path is invalid.");
                                } else if (w32ex.NativeErrorCode == 5)
                                {
                                    Console.WriteLine("Access is denied.");
                                } else if (w32ex.NativeErrorCode == 193)
                                {
                                    Console.WriteLine("Attempted to run a non-runnable file.");
                                } else if (w32ex.NativeErrorCode == 1155)
                                {
                                    Console.WriteLine("The file extension is not supported.");
                                }
                            } catch (IndexOutOfRangeException)
                            {
                                Console.WriteLine("Specify a program.");
                            } catch (Exception)
                            {
                                Console.WriteLine("An unknown error occurred.");
                            }
                            break;
                        case "help":
                            Console.WriteLine("List of commands:\nHELP  Displays this help message.\nMKDIR/MD   Makes a new directory.\nRMDIR/RD    Removes an empty directory. Directory must be empty.\nECHO  Echoes text on the screen.\nMORE    Reads text from a file, one line at a time.\nTYPE       Writes all text from a file to the screen.\nCD     Changes the current working directory.\nEXIT\nDEL       Deletes a file.\nDIR     Shows all files and directories in the working directory.\nSTART        Starts a program.");
                            break;
                        case "exit":
                            running = false;
                            break;
                        default:
                            if (command[0] != "")
                            {
                                if (File.Exists(command[0]) || File.Exists(command[0] + ".exe") || File.Exists(command[0] + ".bat"))
                                {
                                    var process = new Process
                                    {
                                        StartInfo = new ProcessStartInfo
                                        {
                                            FileName = "cmd",
                                            Arguments = "/c " + input,
                                            CreateNoWindow = true,
                                            UseShellExecute = false,
                                            RedirectStandardOutput = true,
                                            RedirectStandardError = true
                                        }
                                    };
                                    process.OutputDataReceived += (sender, e) =>
                                    {
                                        if (e.Data != null) Console.WriteLine(e.Data);
                                    };
                                    process.ErrorDataReceived += (sender, e) =>
                                    {
                                        if (e.Data != null) Console.WriteLine(e.Data);
                                    };
                                    process.Start();
                                    process.BeginOutputReadLine();
                                    process.BeginErrorReadLine();
                                    process.WaitForExit();
                                } else
                                {
                                    Console.WriteLine("Bad command: " + command[0]);
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
