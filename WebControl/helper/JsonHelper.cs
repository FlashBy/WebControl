using System;
using System.Data;
using System.IO;
using WebControl.paging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebControl.helper
{
    /// <summary>
    /// 提供对象的序列化和反序列化方法
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 序列化对象并输出json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Serialize(object value)
        {
            Type type = value.GetType();
            var serializer = new JsonSerializer
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (type == typeof(DataTable))
            {
                serializer.Converters.Add(new DataTableConverter());
            }
            else if (type == typeof(DataResult))
            {
                serializer.Converters.Add(new DataResultConverter());
            }

            // JSON.NET默认会将DateTime类型转换为UTC Time的total millsecond
            // 这里设置时间的转换格式，让DateTime看起来更为直观
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

            serializer.Converters.Add(dateTimeConverter);

            string output = string.Empty;
            using (var sw = new StringWriter())
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.QuoteChar = '"';
                    serializer.Serialize(writer, value);

                    output = sw.ToString();
                }
            }
            return output;
        }
    }

    public class DataResultConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dataResult = value as DataResult;

            #region 序列化DataResult对象

            writer.WriteStartObject();
            // 写total属性
            writer.WritePropertyName("total");
            serializer.Serialize(writer, dataResult.Total);
            // 写pagecount属性
            writer.WritePropertyName("pagecount");
            serializer.Serialize(writer, dataResult.PageCount);
            // 写pageindex属性
            writer.WritePropertyName("pageindex");
            serializer.Serialize(writer, dataResult.PageIndex);

            // 写rows属性
            var converter = new DataTableConverter();
            writer.WritePropertyName("rows");
            converter.WriteJson(writer, dataResult.Data, serializer);
            writer.WriteEndObject();

            #endregion
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 是否可以转换
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(DataResult).IsAssignableFrom(objectType);
        }
    }

    /// <summary>
    /// DataRow转换器
    /// </summary>
    public class DataRowConverter : JsonConverter
    {
        /// <summary>
        /// 序列化DataRow
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var row = value as DataRow;
            writer.WriteStartObject();
            foreach (DataColumn column in row.Table.Columns)
            {
                writer.WritePropertyName(column.ColumnName);
                serializer.Serialize(writer, row[column]);
            }
            writer.WriteEndObject();
        }

        /// <summary>
        /// 反序列化DataRow
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 是否可以转换
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(DataRow).IsAssignableFrom(objectType);
        }
    }

    /// <summary>
    /// DataTable转换器
    /// </summary>
    public class DataTableConverter : JsonConverter
    {
        /// <summary>
        /// 序列化DataTable
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var table = value as DataTable;
            var converter = new DataRowConverter();

            writer.WriteStartArray();

            foreach (DataRow row in table.Rows)
            {
                converter.WriteJson(writer, row, serializer);
            }
            writer.WriteEndArray();
        }

        /// <summary>
        /// 反序列化DataTable
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 是否可以转换
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(DataTable).IsAssignableFrom(objectType);
        }
    }
}
