using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DAL
{
    public static  class datahelpers
    {
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                foreach (DataRow item in table.AsEnumerable())
                {
                    T val = new T();
                    PropertyInfo[] properties = val.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        try
                        {
                            PropertyInfo property = val.GetType().GetProperty(propertyInfo.Name);
                            property.SetValue(val, Convert.ChangeType(item[propertyInfo.Name], property.PropertyType), null);
                        }
                        catch
                        {
                        }
                    }

                    list.Add(val);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
