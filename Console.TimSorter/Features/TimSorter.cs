//-----------------------------------------------------------------------
// <copyright file="TimSorter.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.1, 08.1.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.03.2026</date>
//
// <summary>
// Umsetzung von Python 's Timsort Algorithmus in C#. Timsort ist ein hybrider stabiler Sortieralgorithmus,
// der auf Merge Sort und Insertion Sort basiert. Er wurde entwickelt, um die Vorteile beider Algorithmen
// zu nutzen und ist besonders effizient für reale Daten, die oft teilweise sortiert sind.
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    public static class TimSorter
    {
        const int MIN_MERGE = 32;

        public static void Sort<T>(List<T> list) => Sort(list, Comparer<T>.Default);

        public static void Sort<T>(List<T> list, Comparison<T> comparison) => Sort(list, Comparer<T>.Create(comparison));

        public static void Sort<T>(List<T> list, IComparer<T> comparer)
        {
            if (list == null || list.Count < 2)
            {
                return;
            }

            T[] array = list.ToArray();
            Sort(array, comparer);

            for (int i = 0; i < array.Length; i++)
            {
                list[i] = array[i];
            }
        }

        public static void Sort<T>(T[] array) => Sort(array, Comparer<T>.Default);

        public static void Sort<T>(T[] array, IComparer<T> comparer)
        {
            int n = array.Length;
            int minRun = MinRunLength(n);

            Stack<(int start, int length)> runs = new();

            int i = 0;
            while (i < n)
            {
                int runLen = CountRun(array, i, comparer);

                if (runLen < minRun)
                {
                    int force = Math.Min(minRun, n - i);
                    InsertionSort(array, i, i + force - 1, comparer);
                    runLen = force;
                }

                runs.Push((i, runLen));
                MergeCollapse(array, runs, comparer);

                i += runLen;
            }

            while (runs.Count > 1)
            {
                MergeAt(array, runs, comparer);
            }
        }

        private static int MinRunLength(int n)
        {
            int r = 0;
            while (n >= MIN_MERGE)
            {
                r |= n & 1;
                n >>= 1;
            }

            return n + r;
        }

        private static int CountRun<T>(T[] a, int start, IComparer<T> c)
        {
            int n = a.Length;
            int run = 1;

            if (start == n - 1)
            {
                return run;
            }

            if (c.Compare(a[start], a[start + 1]) <= 0)
            {
                while (start + run < n - 1 && c.Compare(a[start + run], a[start + run + 1]) <= 0)
                {
                    run++;
                }
            }
            else
            {
                while (start + run < n - 1 && c.Compare(a[start + run], a[start + run + 1]) > 0)
                {
                    run++;
                }

                Array.Reverse(a, start, run + 1);
            }

            return run + 1;
        }

        private static void MergeCollapse<T>(T[] a, Stack<(int start, int length)> runs, IComparer<T> c)
        {
            while (runs.Count >= 2)
            {
                var x = runs.Pop();
                var y = runs.Pop();

                if (runs.Count > 0)
                {
                    var z = runs.Peek();
                    if (z.length <= y.length + x.length)
                    {
                        runs.Pop();
                        Merge(a, z.start, y.start, x.start + x.length, c);
                        runs.Push((z.start, z.length + y.length + x.length));
                        continue;
                    }
                }

                Merge(a, y.start, x.start, x.start + x.length, c);
                runs.Push((y.start, y.length + x.length));
            }
        }

        private static void MergeAt<T>(T[] a, Stack<(int start, int length)> runs, IComparer<T> c)
        {
            var x = runs.Pop();
            var y = runs.Pop();

            Merge(a, y.start, x.start, x.start + x.length, c);

            runs.Push((y.start, y.length + x.length));
        }

        private static void Merge<T>(T[] a, int left, int mid, int right, IComparer<T> c)
        {
            int len1 = mid - left;
            int len2 = right - mid;

            T[] leftArr = new T[len1];
            T[] rightArr = new T[len2];

            Array.Copy(a, left, leftArr, 0, len1);
            Array.Copy(a, mid, rightArr, 0, len2);

            int i = 0, j = 0, k = left;

            while (i < len1 && j < len2)
            {
                if (c.Compare(leftArr[i], rightArr[j]) <= 0)
                {
                    a[k++] = leftArr[i++];
                }
                else
                {
                    a[k++] = rightArr[j++];
                }
            }

            while (i < len1)
            {
                a[k++] = leftArr[i++];
            }

            while (j < len2)
            {
                a[k++] = rightArr[j++];
            }
        }

        private static void InsertionSort<T>(T[] a, int left, int right, IComparer<T> c)
        {
            for (int i = left + 1; i <= right; i++)
            {
                T temp = a[i];
                int j = i - 1;

                while (j >= left && c.Compare(a[j], temp) > 0)
                {
                    a[j + 1] = a[j];
                    j--;
                }

                a[j + 1] = temp;
            }
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public static class SortHelper
    {
        public static IComparer<T> CreateComparer<T>(Comparison<T> comparison, SortDirection direction = SortDirection.Ascending)
        {
            if (direction == SortDirection.Ascending)
            {
                return Comparer<T>.Create(comparison);
            }

            return Comparer<T>.Create((a, b) => comparison(b, a));
        }

        public static void TimSort<T>(List<T> list, Comparison<T> comparison, SortDirection direction = SortDirection.Ascending)
        {
            var comparer = CreateComparer(comparison, direction);
            TimSorter.Sort(list, comparer);
        }

        public static void TimSort<T>(T[] array, Comparison<T> comparison, SortDirection direction = SortDirection.Ascending)
        {
            var comparer = CreateComparer(comparison, direction);
            TimSorter.Sort(array, comparer);
        }
    }
}
