using IQueryableFilter.Interfaces;

namespace HomeStorage.Logic.Filters
{
    public static class HSFilterComparerCollection
    {
        public static readonly IFilterComparer Equal = new HSFilterComparer(HSFilterComparerEnum.Equal);
        public static readonly IFilterComparer NotEqual = new HSFilterComparer(HSFilterComparerEnum.Equal);
        public static readonly IFilterComparer StringContains = new HSFilterComparer(HSFilterComparerEnum.StringContains);

        private enum HSFilterComparerEnum
        {
            Undefined = 0,
            Equal = 1,
            NotEqual = 2,
            StringContains = 3,
        }

        private class HSFilterComparer : IFilterComparer
        {
            public HSFilterComparer(HSFilterComparerEnum filterComparer) => _filterComparer = filterComparer;
            public int Identifier => (int)_filterComparer;
            public string FilterComparerName => nameof(HSFilterComparer);
            private readonly HSFilterComparerEnum _filterComparer;

            public int Compare(IFilterComparer? x, IFilterComparer? y)
            {
                if (x is null)
                    throw new ArgumentNullException(nameof(x));
                if (y is null)
                    throw new ArgumentNullException(nameof(y));

                return x.Identifier - y.Identifier;
            }
        }
    }
}
