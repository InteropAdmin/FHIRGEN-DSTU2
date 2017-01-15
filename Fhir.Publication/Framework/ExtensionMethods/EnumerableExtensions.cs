using System;
using System.Collections.Generic;
using System.Linq;

namespace Hl7.Fhir.Publication.Framework.ExtensionMethods
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<TResult> OuterJoin<TOuter, TInner, TKey, TResult>(
       this IEnumerable<TOuter> outer,
       IEnumerable<TInner> inner,
       Func<TOuter, TKey> outerKeySelector,
       Func<TInner, TKey> innerKeySelector,
       Func<TOuter, TInner, TResult> resultSelector)
        {
            return
                  outer
                  .GroupJoin(
                         inner,
                         outerKeySelector,
                         innerKeySelector,
                         (outerItem, matchingInnerItems) => resultSelector(outerItem, matchingInnerItems.SingleOrDefault()));
        }

    }
}