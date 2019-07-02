using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace deadrat22
{
    public class CsvReader<T> where T : new()
    {
        private StreamReader _reader;
        private CsvHeader<T> _csvHeader;

        private string _separator;

        private PropertyInfo[] _properties; 

        public CsvReader(StreamReader reader, string separator)
        {
            _reader = reader;
            _separator = separator;
            _csvHeader = CreateHeader();
            _properties = GetPropertyInfos();
        }
        
        public IEnumerable<T> ReadAll()
        {
            while (!_reader.EndOfStream)
                yield return Read();
        }

        private T Read()
        {
            string[] line = _reader.ReadLine().Split(_separator);
            return MapToObject(line);
        }
        private CsvHeader<T> CreateHeader()
        {
            return new CsvHeader<T>(_reader.ReadLine().Split(_separator));
        }

        private PropertyInfo[] GetPropertyInfos()
        {
            return typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }


        private T MapToObject(string[] values)
        {
            var result = new T();
            var indexValuePairs = Enumerable
                .Range(0, values.Length)
                .Select(index =>
                    new
                    {
                        Index = index,
                        Value = values[index]
                    })
                .ToList();

            indexValuePairs
                .ForEach(pair =>
                    {
                        _csvHeader.CollumnIndexToProperty.TryGetValue(pair.Index, out PropertyInfo property);
                        if (property != null)
                            Map(ref result, pair.Value, property);
                    }
                );

            return result;
        }

        private void Map(ref T obj, string value, PropertyInfo property)
        {
            
            if (property.PropertyType.IsEquivalentTo(typeof(string)))
            {
                property.SetValue(obj, value);
            }
            else if (property.PropertyType.IsEquivalentTo(typeof(double)))
            {
                double.TryParse(value, out double v);
                property.SetValue(obj, v);
            }
            else if (property.PropertyType.IsEquivalentTo(typeof(int)))
            {
                int.TryParse(value, out int v);
                property.SetValue(obj, v);
            }
            else if (property.PropertyType.IsEquivalentTo(typeof(DateTime)))
            {
                DateTime.TryParse(value, out DateTime v);
                property.SetValue(obj, v);
            }
        }
    }

    public class CsvHeader<T>
    {
        public string[] Collumns { get; private set; }
        public Dictionary<int, PropertyInfo> CollumnIndexToProperty { get; private set; }

        public CsvHeader(string[] header)
        {
            Collumns = header
                .Select(str => str.Replace(" ", "").Trim())
                .ToArray();
            CollumnIndexToProperty = CreateDictionary();
        }

        private Dictionary<int, PropertyInfo> CreateDictionary()
        {
            var result = new Dictionary<int, PropertyInfo>();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string[] collumns = GetLowerCaseInvariantCollumns();
            foreach (var prop in properties)
            {
                string invariant = prop.Name.ToLowerInvariant();

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
                    result[index] = prop;
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
