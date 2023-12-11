using System.Diagnostics;
using System.IO.Compression;

Console.BackgroundColor = ConsoleColor.White;
Console.ForegroundColor = ConsoleColor.Black;

Console.WriteLine("*************************************");
Console.WriteLine("|    ReedBooks Updater, Standart    |");
Console.WriteLine("*************************************");

if(args.Length != 1 || args[0] == string.Empty)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: The updater is running without parameters, or with incorrect parameters!");
} 

else
{
    try
    {
        using(var sr = new FileStream(args[0], FileMode.Open))
        {
            var zip = new ZipArchive(sr, ZipArchiveMode.Read);
            zip.ExtractToDirectory(Directory.GetCurrentDirectory(), true);
        }

        File.Delete(args[0]);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Update succesfully installed!");

        Process.Start($"{Directory.GetCurrentDirectory()}\\ReedBooks.exe");
    }
    catch(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: Error occured while updating {ex.Message}");
        Console.ReadLine();
    }
}