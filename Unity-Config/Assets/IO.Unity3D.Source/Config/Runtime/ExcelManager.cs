using System;
using System.Collections.Generic;
using IO.Unity3D.Source.Reflection;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 15:16
    //******************************************
    public class ExcelManager
    {
        private Dictionary<Type, Dictionary<object, object>> _Type2KeyDict = new Dictionary<Type, Dictionary<object, object>>();
        private Dictionary<Type, Dictionary<object,List<object>>> _Type2KeyList = new Dictionary<Type, Dictionary<object,List<object>>>();
        
        private Dictionary<Type, List<object>> _Type2List = new Dictionary<Type, List<object>>();

        public void Load<T>(IExcelStream excelStream)
        {
            var type = typeof(T);

            List<KeyInfo> forDict = new List<KeyInfo>();
            List<KeyInfo> forList = new List<KeyInfo>();
            _GetKeyInfos(type, forDict, forList);

            List<object> objs = new List<object>();
            _Type2List.Add(type, objs);
            
            Dictionary<object, List<object>> key2Lists = null;
            if (forList.Count > 0)
            {
                key2Lists = new Dictionary<object, List<object>>();
                _Type2KeyList.Add(type, key2Lists);  
            }

            Dictionary<object, object> key2Dict = null;
            if (forDict.Count > 0)
            {
                key2Dict = new Dictionary<object, object>();
                _Type2KeyDict.Add(type, key2Dict);    
            }

            ExcelUtils.Parse<T>(excelStream, (t) =>
            {
                for (int i = 0; i < forDict.Count; i++)
                {
                    var keyInfo = forDict[i];
                    var key = keyInfo.New(t);
                    key2Dict.Add(key, t);
                }

                for (int i = 0; i < forList.Count; i++)
                {
                    var keyInfo = forList[i];
                    var key = keyInfo.New(t);
                    List<object> list;
                    if (key2Lists.ContainsKey(key))
                    {
                        list = key2Lists[key];
                    }
                    else
                    {
                        list = new List<object>();
                        key2Lists.Add(key, list);
                    }
                    list.Add(t);
                }

                objs.Add(t);
            });
        }

        private void _GetKeyInfos(Type type, List<KeyInfo> forDict, List<KeyInfo> forList)
        {
            var propertiesAndFields = Reflections.GetPropertiesAndFields<ExcelKey>(type);
            Dictionary<int, KeyInfo> dictKeyInfos = new Dictionary<int, KeyInfo>();
            Dictionary<int, KeyInfo> listKeyInfos = new Dictionary<int, KeyInfo>();
            
            for (int i = 0; i < propertiesAndFields.Count; i++)
            {
                var propertiesAndField = propertiesAndFields[i];
                var excelDictKeys = propertiesAndField.GetCustomAttributes<ExcelKey>();

                foreach (var excelDictKey in excelDictKeys)
                {
                    Dictionary<int, KeyInfo> cacheKeyInfos;
                    
                    if (excelDictKey.GetType() == typeof(ExcelDictKey))
                    {
                        cacheKeyInfos = dictKeyInfos;
                    }
                    else if (excelDictKey.GetType() == typeof(ExcelListKey))
                    {
                        cacheKeyInfos = listKeyInfos;
                    }
                    else
                    {
                        throw new Exception($"Unsupported {nameof(ExcelKey)}");
                    }

                    KeyInfo keyInfo;
                    if (cacheKeyInfos.ContainsKey(excelDictKey.ID))
                    {
                        keyInfo = cacheKeyInfos[excelDictKey.ID];
                    }
                    else
                    {
                        keyInfo = new KeyInfo(excelDictKey.ID);
                        cacheKeyInfos.Add(excelDictKey.ID, keyInfo);
                    }
                    keyInfo.Add(propertiesAndField, excelDictKey); 
                }
            }
            
            foreach (var value in dictKeyInfos.Values)
            {
                value.Make();
                forDict.Add(value);
            }
            
            foreach (var value in listKeyInfos.Values)
            {
                value.Make();
                forList.Add(value);
            }
        }

        public T Get<T>(object key)
        {
            return (T)Get(typeof(T), key);
        }

        public List<T> GetAndConvertList<T>(object key)
        {
            return _Type2KeyList[typeof(T)][key].ConvertAll((o)=>(T)o);
        }

        public List<object> GetList(Type type, object key)
        {
            return _Type2KeyList[type][key];
        }

        public void IterateList<T>(object key, Action<T> onIterate)
        {
            var list = _Type2KeyList[typeof(T)][key];
            foreach (T t in list)
            {
                onIterate(t);
            }
        }

        public List<object> GetAll(Type type)
        {
            return _Type2List[type];
        }

        public void IterateAll<T>(Action<T> onIterate)
        {
            var list = _Type2List[typeof(T)];
            foreach (T item in list)
            {
                onIterate(item);
            }
        }

        public List<T> GetAndConvertAll<T>()
        {
            return _Type2List[typeof(T)].ConvertAll(o=> (T)o);
        }

        public object Get(Type type, object o)
        {
            if (!_Type2KeyDict.ContainsKey(type))
            {
                return null;
            }

            var config = _Type2KeyDict[type];
            if (config.TryGetValue(o, out object v))
            {
                return v;
            }
            return null;
        }

        class KeyInfo
        {
            public class DictKeyField
            {
                public readonly IPropertyOrField PropertiesAndField;
                public readonly ExcelKey ExcelKey;

                public DictKeyField(IPropertyOrField propertiesAndField, ExcelKey excelKey)
                {
                    PropertiesAndField = propertiesAndField;
                    ExcelKey = excelKey;
                }
            }

            public readonly int KeyID;
            public readonly List<DictKeyField> _DictKeyFieldInfos = new List<DictKeyField>();
            public Type KeyType;
            private List<IPropertyOrField> _KeyPropertyOrFields;

            public KeyInfo(int keyID)
            {
                KeyID = keyID;
            }

            public void Add(IPropertyOrField propertiesAndField, ExcelKey excelKey)
            {
                if (excelKey.KeyType != null)
                {
                    if (KeyType == null)
                    {
                        KeyType = excelKey.KeyType;
                    }
                    else if(KeyType != excelKey.KeyType)
                    {
                        throw new Exception($"Key type is same dict must the same. Found {KeyType} and {excelKey.KeyType}, DictID={KeyID}");
                    }
                }

                _DictKeyFieldInfos.Add(new DictKeyField(propertiesAndField, excelKey));
            }

            public void Make()
            {
                if (_DictKeyFieldInfos.Count == 1)
                {
                    if (KeyType == null)
                    {
                        KeyType = _DictKeyFieldInfos[0].PropertiesAndField.GetFieldOrPropertyType();
                        return;
                    }
                }

                if (KeyType == null)
                {
                    throw new Exception($"Dict have more than 1 property must specify the key type. DictID={KeyID}");    
                }
                
                _KeyPropertyOrFields = new List<IPropertyOrField>(_DictKeyFieldInfos.Count);
                for (int i = 0; i < _DictKeyFieldInfos.Count; i++)
                {
                    var info = _DictKeyFieldInfos[i];
                    var name = info.ExcelKey.KeyPropNameAlias ?? info.PropertiesAndField.Name;
                    _KeyPropertyOrFields.Add(Reflections.GetPropertyOrField(KeyType, name));
                }
            }

            public object New<T>(T obj)
            {
                if (KeyType.IsPrimitive)
                {
                    return _DictKeyFieldInfos[0].PropertiesAndField.GetValue(obj);
                }

                var key = Activator.CreateInstance(KeyType);
                for (int i = 0; i < _DictKeyFieldInfos.Count; i++)
                {
                    var dictKeyFieldInfo = _DictKeyFieldInfos[i];
                    var keyPropertyOrField = _KeyPropertyOrFields[i];
                    
                    keyPropertyOrField.SetValue(key, dictKeyFieldInfo.PropertiesAndField.GetValue(obj));
                }
                return key;
            }
        }
    }
}