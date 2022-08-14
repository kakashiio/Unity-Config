using System;
using IO.Unity3D.Source.Reflection;

namespace IO.Unity3D.Source.Config
{
    //******************************************
    // 
    //
    // @Author: Kakashi
    // @Email: john.cha@qq.com
    // @Date: 2022-08-13 16:18
    //******************************************
    public class ExcelRowReflectionParser : IExcelRowParser
    {
        public object Parse(ExcelParserContext ctx, IExcelRow row, Type type)
        {
            var obj = Activator.CreateInstance(type);
            var propertiesAndFields = ctx.GetPropertiesAndFields(type);
            for (int i = 0; i < propertiesAndFields.Count; i++)
            {
                var propertiesAndField = propertiesAndFields[i];
                propertiesAndField.SetValue(obj, _GetVal(ctx, type, row, propertiesAndField));
            }
            return obj;
        }

        private object _GetVal(ExcelParserContext ctx, Type type, IExcelRow row, IPropertyOrField propertiesAndField)
        {
            ExcelColumn excelColumn = propertiesAndField.GetCustomAttribute<ExcelColumn>();
            int index = -1;
            switch (excelColumn.MappingMode)
            {
                case ExcelColumn.Mode.Index:
                {
                    var exceColumnIndex = propertiesAndField.GetCustomAttribute<ExceColumnIndex>();
                    if (exceColumnIndex == null)
                    {
                        throw new Exception($"A field or property with {nameof(ExcelColumn)} which {nameof(ExcelColumn.MappingMode)}={ExcelColumn.Mode.Index} must have {nameof(ExceColumnIndex)} attribute at the same time.");
                    }
                    index = exceColumnIndex.Index;
                    break;
                }
                case ExcelColumn.Mode.Name:
                {
                    var excelColumnName = propertiesAndField.GetCustomAttribute<ExcelColumnName>();
                    if (excelColumnName == null)
                    {
                        throw new Exception($"A field or property with {nameof(ExcelColumn)} which {nameof(ExcelColumn.MappingMode)}={ExcelColumn.Mode.Name} must have {nameof(ExcelColumnName)} attribute at the same time.");
                    }
                    index = ctx.ColumnName2Index(type, excelColumnName.Name);
                    break;   
                }
                default:
                    throw new Exception($"Unsupported mode {excelColumn.MappingMode}");
            }

            return ctx.GetValue(row, index, propertiesAndField, excelColumn);
        }
    }
}