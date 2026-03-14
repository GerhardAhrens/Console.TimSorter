//-----------------------------------------------------------------------
// <copyright file="TimSortExtensions.cs" company="Lifeprojects.de">
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
// Extension Klasse für den Tim Sorter
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    /* Imports from NET Framework */
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
}
