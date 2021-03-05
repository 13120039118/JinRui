using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Learn.Provider.Common
{
    public static class DataTableExtension
    {
        public static IList<T> DataTableToIList<T>(this DataTable dataTable)
        {
            List<T> buffer = new List<T>();

            // 遍历所有行
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                T t = Activator.CreateInstance<T>();

                // 遍历所属性
                foreach (System.Reflection.PropertyInfo property in ReflectionUtils.GetTypeProperties(typeof(T)))
                {
                    // 该属性包含在集合中
                    if (dataTable.Columns.Contains(property.Name) && !property.Name.Equals("Id"))
                    {
                        // 该属性值不是空
                        if (row[property.Name] != DBNull.Value && row[property.Name] != null)
                        {
                            // 整型
                            if (property.PropertyType.Equals(typeof(int)))
                            {
                                property.SetValue(t, Convert.ToInt32(row[property.Name]), null);
                            }
                            else if (property.PropertyType.Equals(typeof(long)))
                            {
                                property.SetValue(t, Convert.ToInt64(row[property.Name]), null);
                            }
                            else if (property.PropertyType == typeof(double))
                            {
                                property.SetValue(t, Convert.ToDouble(row[property.Name]), null);
                            }
                            else if (property.PropertyType == typeof(float))
                            {
                                property.SetValue(t, float.Parse(row[property.Name].ToString()), null);
                            }
                            else if (property.PropertyType == typeof(decimal))
                            {
                                property.SetValue(t, Convert.ToDecimal(row[property.Name]), null);
                            }
                            else if (property.PropertyType.Equals(typeof(string)))
                            {
                                property.SetValue(t, row[property.Name].ToString(), null);
                            }
                            else if (property.PropertyType.Equals(typeof(DateTime)))
                            {
                                property.SetValue(t, Convert.ToDateTime(row[property.Name]), null);
                            }
                            else if (property.PropertyType.Equals(typeof(bool)))
                            {
                                property.SetValue(t, Convert.ToBoolean(row[property.Name]), null);
                            }
                            else if (property.PropertyType.Equals(typeof(uint)))
                            {
                                property.SetValue(t, Convert.ToBoolean(row[property.Name]), null);
                            }
                            else if (property.PropertyType.Equals(typeof(Guid)))
                            {
                                property.SetValue(t, Guid.Parse(row[property.Name].ToString()), null);
                            }
                            else
                            {
                                property.SetValue(t, row[property.Name], null);
                            }
                        }
                    }
                }
                buffer.Add(t);
            }
            return buffer;
        }
    }

    public static class ReflectionUtils
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<string>> PropertyNames = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<string>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        public static List<string> GetPropertyNames(Type type)
        {
            if (PropertyNames.TryGetValue(type.TypeHandle, out var tc))
            {
                return tc.ToList();
            }

            var names = GetTypeProperties(type).Select(x => x.Name).ToList();

            PropertyNames[type.TypeHandle] = names;
            return names;
        }

        internal static List<PropertyInfo> GetTypeProperties(Type type)
        {
            if (TypeProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pis))
            {
                return pis.ToList();
            }

            var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).ToArray();

            TypeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }
    }
}
