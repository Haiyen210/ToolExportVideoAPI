
using ToolExportVideo.Library;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Dynamic;
using MySqlX.XDevAPI.Common;
using ToolExportVideo.Models;
using System.Linq;
using ToolExportVideo.Common;

namespace ToolExportVideo.DL
{
    public class DLBase
    {
        private MySqlConnection _connection { get; set; }
        public DLBase()
        {
            _connection = new MySqlConnection(ConfigUtil.GetAppSettings<string>(AppSettingKeys.ConnectionString));
        }
        public bool SaveData<T>(List<T> datas)
        {
            var success = false;
            foreach (var item in datas)
            {
                string storedName = GetStoredName(item);

                if (!string.IsNullOrWhiteSpace(storedName))
                {
                    success = ExecuteNonQuery(storedName, item);
                }
            }
            return success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ExecuteNonQuery<T>(string storedName, T data)
        {
            var success = false;
            try
            {
                _connection.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = _connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var item = data;
                    if (item != null)
                    {
                        var value = item.GetType().GetProperty("EditMode")?.GetValue(item, null);
                        if (value != null)
                        {
                            item.GetType().GetProperty("CreatedBy")?.SetValue(item, "Admin");
                            item.GetType().GetProperty("ModifiedBy")?.SetValue(item, "Admin");

                            switch (value)
                            {
                                case EditMode.Add:
                                    item.GetType().GetProperty("CreatedDate")?.SetValue(item, DateTime.Now);
                                    var idProp = item.GetType().GetProperty("Id");
                                    if (idProp != null && idProp.GetValue(item) is Guid g && g == Guid.Empty)
                                    {
                                        idProp.SetValue(item, Guid.NewGuid());
                                    }
                                    item.GetType().GetProperty("ModifiedDate")?.SetValue(item, DateTime.Now);
                                    break;
                                case EditMode.Update:
                                    item.GetType().GetProperty("ModifiedDate")?.SetValue(item, DateTime.Now);
                                    break;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(storedName))
                        {
                            cmd.CommandText = storedName;
                            MySqlCommandBuilder.DeriveParameters(cmd);
                            if (cmd.Parameters?.Count > 0)
                            {
                                foreach (MySqlParameter p in cmd.Parameters)
                                {
                                    if (!p.ParameterName.Equals("@RETURN_VALUE"))
                                    {
                                        var proName = p.ParameterName.Replace("@", "");
                                        var val = item.GetType().GetProperty(proName)?.GetValue(item, null);
                                        p.Value = val != null ? val : DBNull.Value;
                                    }
                                }
                            }
                            success = cmd.ExecuteNonQuery() > 0 ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _connection.Close();
            }
            return success;
        }
        /// <summary>
        /// Hàm lấy all dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> SelectAll<T>()
        {
            List<T> result = new List<T>();
            try
            {
                T t = (T)Activator.CreateInstance(typeof(T));
                _connection.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = _connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    string storedName = $"Proc_SelectAll{t.GetType().Name}";
                    if (!string.IsNullOrWhiteSpace(storedName))
                    {
                        cmd.CommandText = storedName;
                        var reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                t = (T)Activator.CreateInstance(typeof(T));
                                for (int inc = 0; inc < reader.FieldCount; inc++)
                                {
                                    Type type = t.GetType();
                                    string name = reader.GetName(inc);
                                    PropertyInfo prop = type.GetProperty(name);
                                    if (prop != null)
                                    {
                                        if (name == prop.Name)
                                        {
                                            var value = reader.GetValue(inc);
                                            if (value != DBNull.Value)
                                            {
                                                if (prop.PropertyType.IsEnum)
                                                {
                                                    // nếu là kiểu enum thì convert ra enuum
                                                    prop.SetValue(t, System.Enum.ToObject(prop.PropertyType, value), null);
                                                }
                                                else
                                                {
                                                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                                                    {
                                                        // nếu là kiểu Nullable thì nhảy vào đây convert
                                                        var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                                                        var val = Convert.ChangeType(value, underlyingType);
                                                        prop.SetValue(t, val, null);
                                                    }
                                                    else
                                                    {
                                                        // những trường hợp còn lại thì vào đây
                                                        prop.SetValue(t, Convert.ChangeType(value, prop.PropertyType), null);
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                result.Add(t);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _connection.Close();
            }
            return result;
        }
        private string GetStoredName<T>(T obj)
        {
            string storedName = string.Empty;
            if (obj != null)
            {
                var value = obj.GetType().GetProperty("EditMode")?.GetValue(obj, null);
                if (value != null)
                {
                    switch (value)
                    {
                        case EditMode.None:
                            break;
                        case EditMode.Add:
                            storedName = $"Proc_Insert{obj.GetType().Name}";
                            break;
                        case EditMode.Update:
                            storedName = $"Proc_Update{obj.GetType().Name}";
                            break;
                        case EditMode.Delete:
                            storedName = $"Proc_Delete{obj.GetType().Name}";
                            break;
                        default:
                            break;
                    }
                }
            }
            return storedName;
        }
        /// <summary>
        /// Hàm lấy all dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> ExecuteReader<T>(string storedName, object param)
        {
            List<T> result = new List<T>();
            try
            {
                T t = (T)Activator.CreateInstance(typeof(T));
                _connection.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = _connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (!string.IsNullOrWhiteSpace(storedName))
                    {
                        cmd.CommandText = storedName;
                        if (param != null)
                        {
                            MySqlCommandBuilder.DeriveParameters(cmd);
                            if (cmd.Parameters?.Count > 0)
                            {
                                foreach (MySqlParameter p in cmd.Parameters)
                                {
                                    if (!p.ParameterName.Equals("@RETURN_VALUE"))
                                    {
                                        var proName = p.ParameterName.Replace("@", "");
                                        var val = param.GetType().GetProperty(proName)?.GetValue(param, null);
                                        p.Value = val != null ? val : DBNull.Value;
                                    }
                                }
                            }
                        }
                        var reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                t = (T)Activator.CreateInstance(typeof(T));
                                for (int inc = 0; inc < reader.FieldCount; inc++)
                                {
                                    Type type = t.GetType();
                                    string name = reader.GetName(inc);
                                    PropertyInfo prop = type.GetProperty(name);
                                    if (prop != null)
                                    {
                                        if (name == prop.Name)
                                        {
                                            var value = reader.GetValue(inc);
                                            if (value != DBNull.Value)
                                            {
                                                if (prop.PropertyType.IsEnum)
                                                {
                                                    // nếu là kiểu enum thì convert ra enuum
                                                    prop.SetValue(t, System.Enum.ToObject(prop.PropertyType, value), null);
                                                }
                                                else
                                                {
                                                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                                                    {
                                                        // nếu là kiểu Nullable thì nhảy vào đây convert
                                                        var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                                                        var val = Convert.ChangeType(value, underlyingType);
                                                        prop.SetValue(t, val, null);
                                                    }
                                                    else
                                                    {
                                                        // những trường hợp còn lại thì vào đây
                                                        prop.SetValue(t, Convert.ChangeType(value, prop.PropertyType), null);
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                result.Add(t);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _connection.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string storedName, object param)
        {
            try
            {
                _connection.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = _connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    if (!string.IsNullOrWhiteSpace(storedName))
                    {
                        cmd.CommandText = storedName;
                        if (param != null)
                        {
                            MySqlCommandBuilder.DeriveParameters(cmd);
                            if (cmd.Parameters?.Count > 0)
                            {
                                foreach (MySqlParameter p in cmd.Parameters)
                                {
                                    if (!p.ParameterName.Equals("@RETURN_VALUE"))
                                    {
                                        var proName = p.ParameterName.Replace("@", "");
                                        var val = param.GetType().GetProperty(proName)?.GetValue(param, null);
                                        p.Value = val != null ? val : DBNull.Value;
                                    }
                                }
                            }
                        }
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Commonfunc.ConvertToType<T>(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _connection.Close();
            }
            return Commonfunc.ConvertToType<T>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T SelectNewCode<T>(string tableName)
        {
            return ExecuteScalar<T>($"Proc_SelectNew{tableName}Code", null);
        }
        public T GetById<T>(long id)
        {
            return ExecuteReader<T>($"Proc_Select{typeof(T).Name}ById", new { Id = id }).FirstOrDefault();
        }
        public T SelectEmployeeByEmail<T>(string email)
        {
            return ExecuteReader<T>($"Proc_SelectEmployeeByEmail",new { Email = email }).FirstOrDefault();
        }
    }

}