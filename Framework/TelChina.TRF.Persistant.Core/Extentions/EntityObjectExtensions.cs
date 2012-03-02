using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.Common;
using System.Data.Metadata.Edm;

namespace TelChina.TRF.Domain.Core.Extentions
{
    /// <summary>
    /// 实体类型扩展方法
    /// </summary>
    internal static class EntityObjectExtensions
    {
        /// <summary>
        /// 动态设置实体字段的值
        /// </summary>
        /// <param name="entry">ObjectStateEntry 实体封装对象</param>
        /// <param name="osm">对象缓冲</param>
        /// <param name="FieldName">字段名</param>
        /// <param name="EDMType">字段类型</param>
        /// <param name="Value">字段值</param>
        internal static void FixupModifiedDates
            (this ObjectStateEntry entry, ObjectStateManager osm, string FieldName, PrimitiveTypeKind EDMType, object Value)
        {
            var fieldMetaData = entry.CurrentValues.DataRecordInfo.FieldMetadata;
            FieldMetadata modifiedField = fieldMetaData
                .Where(f => f.FieldType.Name == FieldName)
                .FirstOrDefault();
            if (modifiedField.FieldType != null)
            {
                string fieldTypeName = modifiedField.FieldType.TypeUsage.EdmType.Name;
                if (fieldTypeName == EDMType.ToString())
                {
                    entry.CurrentValues.SetValue(modifiedField.Ordinal, Value);
                }
            }
        }

        /// <summary>
        /// EDM 与 CLR类型映射
        /// </summary>
        internal class EDMTypeMap
        {
            private static List<string> EDMPrimitiveTypeNames = new List<string>() { "Binary", "Boolean", "Byte", "DateTime", "Decimal", "Double", "Guid", 
                              "Single", "SByte", "Int16", "Int32", "Int64", "String", "Time", "DateTimeOffset" };
            private static List<string> CLRValueTypeNames = new List<string>() { typeof(System.Int32).FullName, typeof(System.Byte[]).FullName, 
                                         typeof(System.Boolean).FullName, typeof(System.Byte).FullName, 
                                         typeof(System.DateTime).FullName, typeof(System.DateTimeOffset).FullName, 
                                         typeof(System.Decimal).FullName, typeof(System.Double).FullName, 
                                         typeof(System.Guid).FullName, typeof(System.Int16).FullName, 
                                         typeof(System.Int64).FullName, typeof(System.SByte).FullName, 
                                         typeof(System.Single).FullName, typeof(System.TimeSpan).FullName };

            /// <summary>
            /// 获取CLR类型名称
            /// </summary>
            /// <param name="EDMPrimitiveTypeName"></param>
            /// <returns></returns>
            public static string GetCLRValueTypeName(string EDMPrimitiveTypeName)
            {
                if (string.IsNullOrEmpty(EDMPrimitiveTypeName))
                {
                    throw new ArgumentNullException("EDMPrimitiveTypeName");
                }
                int index = EDMPrimitiveTypeNames.IndexOf(EDMPrimitiveTypeName);
                if (index < 0)
                {
                    throw new Exception(string.Format("未知的EDM简单类型,名称:{0}", EDMPrimitiveTypeName));
                }
                return CLRValueTypeNames[index];
            }
        }
    }
}
