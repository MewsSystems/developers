using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Structures
{
    public class TableBuilder
    {
        private readonly Dictionary<int, string> _headers = new();
        private readonly List<List<string>> _rows = new();
        private List<string> _currentRow;
        private int _currentOriginalColumnIndex;

        public void AddHeaders(IEnumerable<(string HeaderName, int OriginalHeaderColumnIndex)> headers)
        {
            foreach (var header in headers)
            {
                AddHeader(header);
            }
        }

        public void AddHeader((string HeaderName, int OriginalHeaderColumnIndex) header)
        {
            _headers.Add(header.OriginalHeaderColumnIndex, header.HeaderName);
        }

        public IEnumerable<string> HeaderNames => _headers.Values;

        public bool TryAddCellValue(Func<string> valueGetter)
        {
            try
            {
                if (!_headers.ContainsKey(_currentOriginalColumnIndex))
                {
                    return false;
                }
            
                if (IsNewRowNeeded())
                {
                    _currentRow = new List<string>();
                    _rows.Add(_currentRow);
                }
            
                _currentRow.Add(valueGetter());
                return true;
            }
            finally
            {
                if (_currentRow?.Count >= _headers.Count)
                {
                    _currentRow = default;
                    _currentOriginalColumnIndex = 0;
                }
                else
                {
                    _currentOriginalColumnIndex++;
                }
            }
        }

        private bool IsNewRowNeeded()
        {
            return _currentRow == default || _currentRow.Count == _headers.Count;
        }

        public void AddRowValue(int columnIndex, string columnValue)
        {
            _rows.Last()[columnIndex] = columnValue;
        }

        public Table Build()
        {
            return new Table(HeaderNames.ToList().AsReadOnly(), _rows);
        }
    }
}