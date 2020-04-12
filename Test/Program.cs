using System;
using RazorRenderer;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new Date().Render((object) null));
            Console.WriteLine(new ModelType().Render(1));
            Console.WriteLine(new ModelType().Render("a"));
            Console.WriteLine(new Subfolder.Date().Render((object)null));
            Console.WriteLine(new Subfolder.SubSubFolder.Date().Render((object)null));
            Console.ReadLine();
        }
    }
}
