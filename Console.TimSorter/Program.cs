//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.1, 08.1.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.03.2026 14:26:39</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.TimSorter
{
    /* Imports from NET Framework */
    using System;
    using System.Timers;

    public class Program
    {
        public Program()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
        }
        private static void Main(string[] args)
        {
            CMenu mainMenu = new CMenu("Hauptmenü");
            mainMenu.AddItem("TimSorter; einfache Anwendung", MenuPoint1);
            mainMenu.AddItem("TimSorter; als Extension", MenuPoint2);
            mainMenu.AddItem("TimSorter; mit Custom Object", MenuPoint3);
            mainMenu.AddItem("Performance Standard Sort vs. Tim Sort", MenuPoint4);
            mainMenu.AddItem("Beenden", () => ApplicationExit());
            mainMenu.Show();
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            List<int> data = GenerateListOfInt(10, 1, 20);

            CMenu.Print(string.Join(", ", data));

            TimSorter.Sort(data);

            CMenu.Print(string.Join(", ", data));

            CMenu.Wait();
        }

        private static void MenuPoint2()
        {
            Console.Clear();

            List<int> data = GenerateListOfInt(10, 1, 20);

            CMenu.Print(string.Join(", ", data));

            data.TimSort();

            CMenu.Print(string.Join(", ", data));

            CMenu.Wait();
        }

        private static void MenuPoint3()
        {
            Console.Clear();

            List<Person> people = new()
            {
                new Person { Name = "Anna", Age = 32 },
                new Person { Name = "Tom", Age = 21 },
                new Person { Name = "Julia", Age = 45 }
            };

            TimSorter.Sort(people, new PersonAgeComparer());

            foreach (var person in people)
            {
                CMenu.Print($"{person.Name} ({person.Age} Jahre)");
            }

            CMenu.Print(new string('-',40), ConsoleColor.Green);
            SortHelper.TimSort(people, (a, b) => a.Age.CompareTo(b.Age), SortDirection.Descending);
            foreach (var person in people)
            {
                CMenu.Print($"{person.Name} ({person.Age} Jahre)");
            }

            CMenu.Wait();
        }

        private static void MenuPoint4()
        {
            Console.Clear();

            List<int> data = GenerateListOfInt(1_000_00, 1, 1_000_00);

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            Array.Sort(data.ToArray()); // Standard Sortierung

            stopwatch.Stop();
            CMenu.Print($"Standard Sort;Zeitdifferenz : {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Restart();
            TimSorter.Sort(data);
            stopwatch.Stop();
            CMenu.Print($"Tim Sort;Zeitdifferenz : {stopwatch.ElapsedMilliseconds} ms");

            CMenu.Wait();
        }

        private static List<int> GenerateListOfInt(int count = 1_0, int min = 1, int max = 1_000_000)
        {
            Random rand = new Random();
            List<int> list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(rand.Next(min, max + 1));
            }

            return list;
        }
    }

    public class Person : IComparable<Person>
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public int CompareTo(Person other)
        {
            return Age.CompareTo(other.Age);
        }
    }

    public class PersonAgeComparer : IComparer<Person>
    {
        public int Compare(Person x, Person y)
        {
            return x.Age.CompareTo(y.Age);
        }
    }
}
