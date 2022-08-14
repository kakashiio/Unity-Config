using System;
using System.Collections.Generic;
using System.Reflection;
using IO.Unity3D.Source.Reflection;
using UnityEngine;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 16:11
    //******************************************
    public class ExcelParserContext
    {
        private IExcelRowParser _Default;
        
        private Dictionary<Type, IExcelRowParser> _RowParsers = new Dictionary<Type, IExcelRowParser>();
        private Dictionary<Type, IExcelFieldParser> _FieldParsers = new Dictionary<Type, IExcelFieldParser>();
        private Dictionary<Type, IStringParser> _StringParsers = new Dictionary<Type, IStringParser>();
        
        private Dictionary<Type, List<IPropertyOrField>> _Type2PropertyOrFields = new Dictionary<Type, List<IPropertyOrField>>();
        private Dictionary<Type, Dictionary<string, int>> _TypeName2Index = new Dictionary<Type, Dictionary<string, int>>();

        private string[] _Separator = new string[1];
        private object[] _KVArgs = new object[2];
        
        public ExcelParserContext()
        {
            SetDefaultParser(new ExcelRowReflectionParser());
            SetFieldParser(typeof(int), typeof(IntFieldParser));
            SetFieldParser(typeof(bool), typeof(BoolFieldParser));
            SetFieldParser(typeof(byte), typeof(ByteFieldParser));
            SetFieldParser(typeof(double), typeof(DoubleFieldParser));
            SetFieldParser(typeof(float), typeof(FloatFieldParser));
            SetFieldParser(typeof(long), typeof(LongFieldParser));
            SetFieldParser(typeof(short), typeof(ShortFieldParser));
            SetFieldParser(typeof(string), typeof(StringFieldParser));
            SetFieldParser(typeof(sbyte), typeof(SignedByteFieldParser));
            SetFieldParser(typeof(uint), typeof(UnsignedIntegerFieldParser));
            SetFieldParser(typeof(ulong), typeof(UnsignedLongFieldParser));
            SetFieldParser(typeof(ushort), typeof(UnsignedShortFieldParser));
            
            SetStringParser(typeof(int), typeof(IntStringParser));
            SetStringParser(typeof(bool), typeof(BoolStringParser));
            SetStringParser(typeof(byte), typeof(ByteStringParser));
            SetStringParser(typeof(double), typeof(DoubleStringParser));
            SetStringParser(typeof(float), typeof(FloatStringParser));
            SetStringParser(typeof(long), typeof(LongStringParser));
            SetStringParser(typeof(short), typeof(ShortStringParser));
            SetStringParser(typeof(string), typeof(StringStringParser));
            SetStringParser(typeof(sbyte), typeof(SignedByteStringParser));
            SetStringParser(typeof(uint), typeof(UnsignedIntegerStringParser));
            SetStringParser(typeof(ulong), typeof(UnsignedLongStringParser));
            SetStringParser(typeof(ushort), typeof(UnsignedShortStringParser));
        }

        public void SetDefaultParser(IExcelRowParser defaultParser)
        {
            _Default = defaultParser;
        }

        public void SetStringParser(Type type, IStringParser stringParser)
        {
            if (type == null || stringParser == null)
            {
                throw new Exception($"{nameof(type)} and {nameof(stringParser)} can not be null");
            }

            if (_StringParsers.ContainsKey(type))
            {
                _StringParsers[type] = stringParser;
            }
            else
            {
                _StringParsers.Add(type, stringParser);
            }
        }

        public void SetStringParser(Type type, Type stringParserType)
        {
            if (type == null || stringParserType == null)
            {
                throw new Exception($"{nameof(type)} and {nameof(stringParserType)} can not be null");
            }

            if (!typeof(IStringParser).IsAssignableFrom(stringParserType))
            {
                throw new Exception($"stringParserType must implement {nameof(IStringParser)}");
            }

            var stringParser = (IStringParser) Activator.CreateInstance(stringParserType);
            if (_StringParsers.ContainsKey(type))
            {
                _StringParsers[type] = stringParser;
            }
            else
            {
                _StringParsers.Add(type, stringParser);
            }
        }

        public void SetFieldParser(IExcelFieldParser fieldParser)
        {
            if (fieldParser == null)
            {
                throw new Exception($"Argument `{nameof(fieldParser)}` can not be null");
            }

            _SetFieldParser(fieldParser.GetType(), fieldParser);
        }

        private void _SetFieldParser(Type type, IExcelFieldParser fieldParser)
        {
            if (_FieldParsers.ContainsKey(type))
            {
                _FieldParsers[type] = fieldParser;
            }
            else
            {
                _FieldParsers.Add(type, fieldParser);
            }
        }

        public void SetFieldParser(Type fieldType, Type parserType)
        {
            if (fieldType == null || parserType == null)
            {
                throw new Exception($"Argument `{nameof(fieldType)}` and `{nameof(parserType)}` can not be null");
            }
            if (!typeof(IExcelFieldParser).IsAssignableFrom(parserType))
            {
                throw new Exception($"Field parser type must implement {nameof(IExcelFieldParser)}, but {parserType} does not.");
            }
            var parser = (IExcelFieldParser) Activator.CreateInstance(parserType);
            _SetFieldParser(fieldType, parser);
        }

        private IExcelFieldParser _GetFieldParser(Type fieldOrPropertyType, Type customParserType)
        {
            if (customParserType != null)
            {
                if (_FieldParsers.ContainsKey(customParserType))
                {
                    return _FieldParsers[customParserType];
                }

                if (!typeof(IExcelFieldParser).IsAssignableFrom(customParserType))
                {
                    throw new Exception($"Custom field parser type must implement {nameof(IExcelFieldParser)}, but {customParserType} does not.");
                }

                var parser = (IExcelFieldParser) Activator.CreateInstance(customParserType);
                _FieldParsers.Add(customParserType, parser);
                return parser;
            }
            
            if (_FieldParsers.ContainsKey(fieldOrPropertyType))
            {
                return _FieldParsers[fieldOrPropertyType];
            }
            
            throw new Exception($"Can not find field parser for field type {fieldOrPropertyType}");
        }

        public void SetRowParser(Type type, IExcelRowParser excelRowParser)
        {
            if (type == null || excelRowParser == null)
            {
                throw new Exception($"Argument `{nameof(type)}` and `{nameof(excelRowParser)}` can not be null");
            }

            if (_RowParsers.ContainsKey(type))
            {
                _RowParsers[type] = excelRowParser;
            }
            else
            {
                _RowParsers.Add(type, excelRowParser);
            }
        }

        public void RemoveRowParser(Type type)
        {
            if (type == null)
            {
                throw new Exception($"Argument `type` can not be null");
            }
            _RowParsers.Remove(type);
        }

        private IExcelRowParser _GetRowParser(Type type)
        {
            if (_RowParsers.TryGetValue(type, out IExcelRowParser parser))
            {
                return parser;
            }
            return _Default;
        }

        public List<IPropertyOrField> GetPropertiesAndFields(Type type)
        {
            if (_Type2PropertyOrFields.ContainsKey(type))
            {
                return _Type2PropertyOrFields[type];
            }

            var propertiesAndFields = Reflections.GetPropertiesAndFields<ExcelColumn>(type);
            _Type2PropertyOrFields.Add(type, propertiesAndFields);
            return propertiesAndFields;
        }

        public object Parse(IExcelRow row, Type type)
        {
            var parser = _GetRowParser(type);
            return parser.Parse(this, row, type);
        }

        public void MapColumnName2Index(Type type, Dictionary<string, int> dictionary)
        {
            if (_TypeName2Index.ContainsKey(type))
            {
                _TypeName2Index[type] = dictionary;
            }
            else
            {
                _TypeName2Index.Add(type, dictionary);
            }
        }

        public int ColumnName2Index(Type type, string name)
        {
            if (!_TypeName2Index.ContainsKey(type))
            {
                var name2Index = _TypeName2Index[type];
                if (!name2Index.ContainsKey(name))
                {
                    foreach (var excelColumnMapping in type.GetCustomAttributes<ExcelColumnMapping>())
                    {
                        if (!name2Index.ContainsKey(excelColumnMapping.Name))
                        {
                            name2Index[excelColumnMapping.Name] = excelColumnMapping.Index;
                        }
                    }       
                }
            }
            
            if (_TypeName2Index.ContainsKey(type))
            {
                var name2Index = _TypeName2Index[type];
                if (name2Index.ContainsKey(name))
                {
                    return name2Index[name];
                }
            }
            throw new Exception($"Can not find mapping for column name `{name}` in type `{type}`, check {nameof(ExcelColumnMapping)} attribute on type {type} or using {nameof(MapColumnName2Index)} in the context");
        }

        public object GetValue(IExcelRow row, int index, IPropertyOrField propertiesAndField, ExcelColumn excelColumn)
        {
            var fieldOrPropertyType = propertiesAndField.GetFieldOrPropertyType();
            if (fieldOrPropertyType.IsArray)
            {
                var str = row.GetString(index);
                var separatorAttr = propertiesAndField.GetCustomAttribute<ExcelColumnArraySeparator>();
                var separator = separatorAttr == null ? ExcelColumnArraySeparator.DEFAULT_SEPARATOR : separatorAttr.Separator;
                _Separator[0] = separator;
                
                var itemStrings = str.Split(_Separator, StringSplitOptions.RemoveEmptyEntries);
                var elementType = fieldOrPropertyType.GetElementType();
                Array array = Array.CreateInstance(elementType, itemStrings.Length);

                for (int i = 0; i < itemStrings.Length; i++)
                {
                    object val = _String2Value(itemStrings[i], elementType, propertiesAndField, excelColumn);
                    array.SetValue(val, i);
                }
                return array;
            }
            else if (fieldOrPropertyType.IsGenericType)
            {
                if (typeof(Dictionary<,>) == fieldOrPropertyType.GetGenericTypeDefinition())
                {
                    var dict = Activator.CreateInstance(fieldOrPropertyType);
                    var addMethod = fieldOrPropertyType.GetMethod("Add");

                    ExcelColumnDictionary excelColumnDictionary = fieldOrPropertyType.GetCustomAttribute<ExcelColumnDictionary>();
                    string itemSeparator = excelColumnDictionary == null ? ExcelColumnDictionary.DEFAULT_ITEM_SEPARATOR : excelColumnDictionary.ItemSeparator;
                    string kvSeparator = excelColumnDictionary == null ? ExcelColumnDictionary.DEFAULT_KV_SEPARATOR : excelColumnDictionary.KVSeparator;

                    var str = row.GetString(index);
                    
                    _Separator[0] = itemSeparator;
                    var itemStrings = str.Split(_Separator, StringSplitOptions.RemoveEmptyEntries);

                    var keyType = fieldOrPropertyType.GetGenericArguments()[0];
                    var valueType = fieldOrPropertyType.GetGenericArguments()[1];
                    _Separator[0] = kvSeparator;
                    for (int i = 0; i < itemStrings.Length; i++)
                    {
                        var itemString = itemStrings[i];
                        var kvString = itemString.Split(_Separator, StringSplitOptions.RemoveEmptyEntries);
                        
                        object k = _String2Value(kvString[0], keyType, propertiesAndField, excelColumn);
                        object v = _String2Value(kvString[1], valueType, propertiesAndField, excelColumn);
                        _KVArgs[0] = k;
                        _KVArgs[1] = v;
                        addMethod.Invoke(dict, _KVArgs);
                    }
                    return dict;   
                }

                throw new Exception($"Unsupported field type {fieldOrPropertyType}");
            }
            else
            {
                IExcelFieldParser excelFieldParser = _GetFieldParser(fieldOrPropertyType, excelColumn.ParserType);
                return excelFieldParser.ParseValue(row, index, propertiesAndField);    
            }
        }

        private object _String2Value(string str, Type type, IPropertyOrField propertiesAndField, ExcelColumn excelColumn)
        {
            ExcelColumnStringParser excelColumnStringParser = propertiesAndField.GetCustomAttribute<ExcelColumnStringParser>();
            if (excelColumnStringParser != null && excelColumnStringParser.ParserType != null)
            {
                if (_StringParsers.TryGetValue(excelColumnStringParser.ParserType, out IStringParser parser))
                {
                    return parser.ParseString2Value(str);
                }
            }

            if (type.IsEnum)
            {
                return Enum.ToObject(type, int.Parse(str));
            }

            {
                if (_StringParsers.TryGetValue(type, out IStringParser parser))
                {
                    return parser.ParseString2Value(str);                          
                }   
            }

            throw new Exception($"Can not found string parser for field `{propertiesAndField.GetFieldOrPropertyType()}` `{propertiesAndField.Name}`, use {nameof(SetStringParser)} or add {nameof(ExcelColumnStringParser)} in the field or property ");
        }
    }
}