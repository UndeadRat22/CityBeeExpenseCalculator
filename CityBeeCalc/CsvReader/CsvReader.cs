using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            values
                .GetIndexedIEnumerable()
                .ForEach(pair =>
                    {
                        _csvHeader.CollumnIndexToProperty.TryGetValue(pair.Index, out PropertyInfo property);
                        if (property != null)
                        {
                            Map(ref result, pair.Value, property);
                        }
                    }
                );

            return result;
        }

        private void Map(ref T obj, string value, PropertyInfo property)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
            object converted = converter.ConvertFromString(value);
            property.SetValue(obj, converted);
        }
    }

    public class CsvHeader<T>
    {
        public string[] Collumns { get; private set; }
        public Dictionary<int, PropertyInfo> CollumnIndexToProperty { get; private set; }

        public CsvHeader(string[] header)
        {
            Collumns = header
                .Select(str => str.Replace(" ", "").Trim().ToLowerInvariant())
                .ToArray();
            CollumnIndexToProperty = CreateDictionary();
        }

        private Dictionary<int, PropertyInfo> CreateDictionary()
        {
            var result = new Dictionary<int, PropertyInfo>();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
            {
                string invariant = prop.Name.ToLowerInvariant();

                int index = Collumns.IndexOfOrDefault(invariant);

                if (index != -1)
                {
                    result[index] = prop;
                }
            }
            return result;
        }

    }
}
