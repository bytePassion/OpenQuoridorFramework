using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lib.FrameworkExtension
{
	public static class ObservableCollectionExtensions
    {
        public static void Sort<T>(this ObservableCollection<T> observableList) where T : IComparable<T>, IEquatable<T>
        {
            var sorted = observableList.OrderBy(x => x).ToList();
            ApplySorting(observableList, sorted);
        }

        public static void Sort<T>(this ObservableCollection<T> observableList, IComparer<T> comparer)
        {
            var sorted = observableList.OrderBy(x => x, comparer).ToList();
            ApplySorting(observableList, sorted);
        }

        private static void ApplySorting<T>(IList<T> observableList, IList<T> sortedList)
        {
            int index = 0;
            while (index < sortedList.Count)
            {
                if (!observableList[index].Equals(sortedList[index]))
                {
                    var item = observableList[index];
                    observableList.RemoveAt(index);
                    observableList.Insert(sortedList.IndexOf(item), item);
                }
                else
                {
                    index++;
                }
            }
        }
    }
}
