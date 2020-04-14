using System;
using RazorRenderer;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteMagenta("date.cshtml");
            Console.WriteLine(new Date().Render((object) null));
            WriteMagenta("ModelType.cshtml > 1");
            Console.WriteLine(new ModelType().Render(1));
            WriteMagenta("ModelType.cshtml > 'a'");
            Console.WriteLine(new ModelType().Render("a"));
            WriteMagenta("Subfolder/date.cshtml");
            Console.WriteLine(new Subfolder.Date().Render((object)null));
            WriteMagenta("Subfolder/SubSubFolder/date.cshtml");
            Console.WriteLine(new Subfolder.SubSubFolder.Date().Render((object)null));
            Console.ReadLine();
        }

        static void WriteMagenta(string write)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(write);
            Console.ResetColor();
        }
    }
}
