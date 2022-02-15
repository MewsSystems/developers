using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExchangeRateUpdater.Structures
{
    public class Table
    {
        public ReadOnlyCollection<string> HeaderNames { get; }
        public ReadOnlyCollection<Row> Rows { get; }

        public Table(IEnumerable<string> headerNames, IEnumerable<IEnumerable<string>> rows)
        {
            HeaderNames = headerNames.ToList().AsReadOnly();
            Rows = rows.Select(rowValues => new Row(HeaderNames, rowValues)).ToList().AsReadOnly();
        }

        public class Row
        {
            private readonly ReadOnlyCollection<string> _headerNames;
            private readonly ReadOnlyCollection<string> _values;

            public Row(ReadOnlyCollection<string> headerNames, IEnumerable<string> values)
            {
                _headerNames = headerNames;
                _values = values.ToList().AsReadOnly();
                if (_headerNames.Count != _values.Count)
                {
                    throw new Exception($"Header names have count '{_headerNames.Count}' which is different to " +
                                        $"values count '{_values.Count}'");
                }
            }

            public string this[string headerName]
            {
                get
                {
                    var headerIndex = _headerNames.IndexOf(headerName);
                    if (headerIndex < 0 || headerIndex >= _headerNames.Count)
                    {
                        throw new Exception($"Header '{headerName}' was not found " +
                                            $"in headers '[{string.Join(", ", _headerNames)}]'");
                    }
                    return _values[headerIndex];
                }
            }
        }
    }
}

