using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using System.Linq;

namespace deadrat22
{
    public class CsvReader<T> where T : new()
    {
        private StreamReader _reader;
        private CsvHeader<T> _header;

        private CsvHeader<T> CsvHeader
        {
            get
            {
                if (_header == null)
                    _header = CreateHeader();
                return _header;
            }
        }

        private string _separator;

        public CsvReader(StreamReader reader, string separator)
        {
            _reader = reader;
            _separator = separator;
        }
        

        public T Read()
        {
            string[] line = _reader.ReadLine().Split(_separator);
            return Map(line);
        }
        private CsvHeader<T> CreateHeader()
        {
            return new CsvHeader<T>(_reader.ReadLine().Split(_separator));
        }

        private T Map(string[] values)
        {
            var result = new T();
            for (int i = 0; i < values.Length; i++)
            {
                if (CsvHeader.CollumnIndexToProperty.TryGetValue(i, out string propName))
                    continue;

                FieldInfo[] fields = result.GetType().GetFields();
                foreach(var field in fields)
                {
                    if (field.IsPrivate)
                    {
                        continue;
                    }
                    if (field.FieldType.IsEquivalentTo(typeof(int)))
                    {
                        field.SetValue(result, int.Parse(values[i]));
                    }
                    else if (field.FieldType.IsEquivalentTo(typeof(double)))
                    {
                        field.SetValue(result, double.Parse(values[i]));
                    }
                    else if (field.FieldType.IsEquivalentTo(typeof(string)))
                    {
                        field.SetValue(result, values[i]);
                    }
                    else if (field.FieldType.IsEquivalentTo(typeof(DateTime)))
                    {
                        field.SetValue(result, DateTime.Parse(values[i]));
                    }
                }
            }

            return new T();
        }
    }

    public class CsvHeader<T>
    {
        public string[] Collumns { get; private set; }
        public Dictionary<int, string> CollumnIndexToProperty { get; private set; }

        public CsvHeader(string[] header)
        {
            Collumns = header
                .Select(str => str.Replace(" ", "").Trim())
                .ToArray();
            CollumnIndexToProperty = CreateDictionary();
        }

        private Dictionary<int, string> CreateDictionary()
        {
            var result = new Dictionary<int, string>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                string invariant = prop.Name.ToLowerInvariant();

                var collumns = GetLowerCaseInvariantCollumns();

                int index = -1;

                for (int i = 0; i < collumns.Length; i++)
                {
                    if (invariant == collumns[i])
                    {
                        index = i;
                        break;
                    }
                }

                if (index != -1)
                {
                    result[index] = prop.Name;
                }
            }
            return result;
        }
        private string[] GetLowerCaseInvariantCollumns()
        {
            return Collumns.Select(col => col.ToLowerInvariant()).ToArray();
        }

    }
}
