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

    public static class TimSortExtensions
    {
        public static void TimSort<T>(this List<T> list) where T : IComparable<T>
        {
            var array = list.ToArray();

            TimSorter.Sort<T>(array);

            for (int i = 0; i < array.Length; i++)
            {
                list[i] = array[i];
            }
        }
    }

    public static class TimSorter
    {
        private const int MIN_RUN = 32;

        public static void Sort<T>(List<T> list) where T : IComparable<T>
        {
            var array = list.ToArray();

            Sort(array, Comparer<T>.Default);

            for (int i = 0; i < array.Length; i++)
            {
                list[i] = array[i];
            }
        }

        public static void Sort<T>(List<T> list, IComparer<T> comparer) where T : IComparable<T>
        {
            var array = list.ToArray();

            Sort(array, comparer);

            for (int i = 0; i < array.Length; i++)
            {
                list[i] = array[i];
            }
        }

        public static void Sort<T>(T[] array)
        {
            Sort(array, Comparer<T>.Default);
        }

        public static void Sort<T>(T[] array, IComparer<T> comparer)
        {
            int n = array.Length;

            for (int i = 0; i < n; i += MIN_RUN)
            {
                InsertionSort(array, i, Math.Min(i + MIN_RUN - 1, n - 1), comparer);
            }

            for (int size = MIN_RUN; size < n; size *= 2)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = Math.Min(left + 2 * size - 1, n - 1);

                    if (mid < right)
                        Merge(array, left, mid, right, comparer);
                }
            }
        }

        private static void InsertionSort<T>(T[] array, int left, int right, IComparer<T> comparer)
        {
            for (int i = left + 1; i <= right; i++)
            {
                T temp = array[i];
                int j = i - 1;

                while (j >= left && comparer.Compare(array[j], temp) > 0)
                {
                    array[j + 1] = array[j];
                    j--;
                }

                array[j + 1] = temp;
            }
        }

        private static void Merge<T>(T[] array, int left, int mid, int right, IComparer<T> comparer)
        {
            int len1 = mid - left + 1;
            int len2 = right - mid;

            T[] leftArr = new T[len1];
            T[] rightArr = new T[len2];

            Array.Copy(array, left, leftArr, 0, len1);
            Array.Copy(array, mid + 1, rightArr, 0, len2);

            int i = 0, j = 0, k = left;

            while (i < len1 && j < len2)
            {
                if (comparer.Compare(leftArr[i], rightArr[j]) <= 0)
                    array[k++] = leftArr[i++];
                else
                    array[k++] = rightArr[j++];
            }

            while (i < len1)
                array[k++] = leftArr[i++];

            while (j < len2)
                array[k++] = rightArr[j++];
        }
    }
}
