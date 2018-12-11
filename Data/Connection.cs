using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using FastMember;
using Libro.Models;

namespace Libro.Data
{
    internal static class Db
    {
        private static SQLiteConnection _connection;

        internal static SQLiteConnection Connection
        {
            get
            {
                if(_connection != null && (
                       _connection.State != ConnectionState.Closed &&
                       _connection.State != ConnectionState.Broken))
                    return _connection;

                var con = new SQLiteConnection("Data Source=System.Libro.dll");
                var pwd = new List<string>();
                pwd.AddRange(new[] { "Pro", "JEct", "PE", "pE" });
                var bPwd = new List<byte>();
                bPwd.AddRange(Encoding.ASCII.GetBytes(pwd[0]));
                bPwd.Add(4);
                bPwd.AddRange(Encoding.ASCII.GetBytes(pwd[1]));
                bPwd.Add(7);
                bPwd.AddRange(Encoding.ASCII.GetBytes(pwd[2]));
                bPwd.Add(8);
                bPwd.AddRange(Encoding.ASCII.GetBytes(pwd[3]));
                bPwd.Add(6);

#if ENCRPYT_DB
                try
                {
                    con.Open();
                    con.ChangePassword(bPwd.ToArray());
                }
                catch (Exception)
                {
                    con.Close();
                    con.SetPassword(bPwd.ToArray());
                    con.Open();                    
                }

                bPwd.Clear();

                var c = con.CreateCommand();
                c.CommandText = "PRAGMA synchronous=OFF;PRAGMA foreign_keys=on";
                c.ExecuteNonQuery();
#else
                try
                {
                    con.Open();
                    var c = con.CreateCommand();
                    c.CommandText = "PRAGMA synchronous=OFF;PRAGMA foreign_keys=on";
                    c.ExecuteNonQuery();

                } catch(Exception)
                {
                    con.Close();
                    con.SetPassword(bPwd.ToArray());
                    con.Open();
                    con.ChangePassword(string.Empty);
                    var c = con.CreateCommand();
                    c.CommandText = "PRAGMA synchronous=OFF;PRAGMA foreign_keys=on";
                    c.ExecuteNonQuery();
                }
#endif
                

                _connection = con;
                return con;
            }
        }

        internal static bool TableExists(string table)
        {
            var c = Connection.CreateCommand();
            c.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@table;";
            c.Parameters.AddWithValue("@table", table);
            var r = (long) c.ExecuteScalar();
            return r > 0;
        }
        
        internal static List<T> GetAll<T>() where T : ModelBase<T>
        {
            var list = new List<T>();
            var table = GetTable<T>();
            var c = Connection.CreateCommand();
            c.CommandText = $"SELECT * FROM {table.Name} WHERE NOT IsDeleted LIMIT 0,471;";
            var r = c.ExecuteReader();
            while (r.Read())
            {
                list.Add(CreateFromReader<T>(r,table));
            }
            
            return list;
        }
        
        private static Dictionary<string, DbTable> _tables;
        private static object _tablesLock;
        
        private static DbTable GetTable<T>() where T : ModelBase<T>
        {
            var type = typeof(T);

            if(_tablesLock == null)
                _tablesLock = new object();
            lock(_tablesLock)
            {
                if(_tables==null) _tables = new Dictionary<string, DbTable>();
                if(_tables.ContainsKey(type.FullName))
                    return _tables[type.FullName];
                var tbl = new DbTable(type);
                _tables.Add(type.FullName, tbl);

                var c = Connection.CreateCommand();
                c.CommandText = tbl.CreateSql;
                c.ExecuteNonQuery();

                return tbl;
            }
        }

        internal static void DropTable<T>() where T : ModelBase<T>
        {
            var table = GetTable<T>();
            var c = Connection.CreateCommand();
            c.CommandText = $"DROP TABLE IF EXIST {table.Name};";
            c.ExecuteNonQuery();
        }
        
        internal static long Insert<T>(this T item) where T : ModelBase<T>
        {
            var table = GetTable<T>();
            var con = Connection;
            var c = con.CreateCommand();
            c.CommandText = table.InsertSql;

            c.Parameters.Clear();
            var obj = ObjectAccessor.Create(item);
            foreach (var col in table.Columns)
            {
                c.Parameters.AddWithValue($"@{col.Name}", obj[col.Name]);
            }
            
            return (long) c.ExecuteScalar();
        }
        private static Dictionary<Type,SQLiteCommand> _updateCommands = new Dictionary<Type, SQLiteCommand>();
        private static object _updateLock = new object();
        internal static void Update<T>(this T item) where T : ModelBase<T>
        {
            var table = GetTable<T>();
            SQLiteCommand c;
            lock(_updateLock)
            {
                if(_updateCommands.ContainsKey(item.GetType()) && _updateCommands[item.GetType()].Connection.State!=ConnectionState.Closed)
                    c = _updateCommands[item.GetType()];
                else
                {
                    c = Connection.CreateCommand();
                    c.CommandText = table.UpdateSql;
                    _updateCommands.Add(item.GetType(), c);
                }
            }
            c.Parameters.Clear();
            var obj = ObjectAccessor.Create(item);
            foreach (var col in table.Columns)
                c.Parameters.AddWithValue($"@{col.Name}", obj[col.Name]);

            c.Parameters.AddWithValue($"@{table.PrimaryKey.Name}", obj[table.PrimaryKey.Name]);
            c.ExecuteNonQuery();
        }

        internal static void Update<T>(long? id, string column, object value) where T : ModelBase<T>
        {
            if ((id ?? 0) == 0) return;
            var c = Connection.CreateCommand();
            c.CommandText = $"UPDATE {GetTable<T>().Name} SET [{column}]=@{column} WHERE Id=@id;";
            c.Parameters.AddWithValue("@id", id);
            c.Parameters.AddWithValue($"@{column}", value);
            c.ExecuteNonQuery();
        }

        private static T CreateFromReader<T>(SQLiteDataReader reader, DbTable table) where T: ModelBase<T>
        {
            var model = TypeAccessor.Create(typeof(T));
            var obj = model.CreateNew();
            foreach (var dbColumn in table.Columns)
            {
                if (reader[dbColumn.Name] != DBNull.Value)
                    model[obj, dbColumn.Name] = dbColumn.Cast(reader[dbColumn.Name]);
            }
            return (T) obj;
        }

        internal static bool Exist<T>(string column, object value) where T : ModelBase<T>
        {
            var table = GetTable<T>();
            var c = Connection.CreateCommand();
            c.CommandText = $"SELECT * FROM {table.Name} WHERE [{column}]=@{column} LIMIT 0,1;";
            c.Parameters.AddWithValue($"@{column}", value);
            var r = c.ExecuteReader();
            if (r.Read())
                return true;
            return false;
        }

        internal static T GetById<T>(long id) where T : ModelBase<T>
        {
            return GetBy<T>("Id", id);
        }

        internal static T GetBy<T>(string column, object value) where T: ModelBase<T>
        {
            var table = GetTable<T>();
            var c = Connection.CreateCommand();
            c.CommandText = $"SELECT * FROM {table.Name} WHERE [{column}]=@{column};";
            c.Parameters.AddWithValue($"@{column}", value);
            var r = c.ExecuteReader();
            if (r.Read())
            {
                return CreateFromReader<T>(r, table);
            }
            return default(T);
        }
        
        internal static List<T> Where<T>(string where, Dictionary<string, object> args) where T : ModelBase<T>
        {
            var tbl = GetTable<T>();
            return Query<T>($"SELECT * FROM [{tbl.Name}] WHERE {where};", args);
        }
     
        internal static void SaveItem<T>(T model) where T : ModelBase<T>
        {
            if (model.Id > 0)
                model.Update();
            else
            {
                model.Id = model.Insert();
            }
            model.OnSaved();
        }

        internal static List<T> Query<T>(string sql, Dictionary<string,object> args) where T: ModelBase<T>
        {
            var list = new List<T>();
            var c = Connection.CreateCommand();
            c.CommandText = sql;
            foreach (var o in args)
            {
                c.Parameters.AddWithValue("@" + o.Key, o.Value);
            }
            
            var r = c.ExecuteReader();
            var table = GetTable<T>();
            while (r.Read())
            {
                list.Add(CreateFromReader<T>(r,table));
                if(list.Count==471) break;
            }
            return list;
        }

        internal static T ExecuteScalar<T>(string sql, Dictionary<string, object> param)
        {
            
                var c = Connection.CreateCommand();
                c.CommandText = sql;
                foreach (var o in param)
                {
                    c.Parameters.AddWithValue($"@{o.Key}", o.Value);
                }
                var r = c.ExecuteReader();
                if (r.Read())
                {
                    var result = r[0];
                    if (result == DBNull.Value) return default(T);
                    return (T) result;
                }
                else
                {
                    return default(T);
                }
        }

        private static DbType GetDbType(Type type)
        {
            DbType dbType;
            if (Enum.TryParse(type.Name, out dbType))
                return dbType;
            if(type == typeof(int))
                return DbType.Int32;
            if(type== typeof(long))
                return DbType.Int64;
            
            return DbType.Object;
        }
        
        internal static void Delete<T>(long id, bool permanent = false) where T : ModelBase<T>
        {
            if (id == 0) return;
            var table = GetTable<T>();
            var c = Connection.CreateCommand();
            //c.CommandText = $"DELETE FROM {table.Name} WHERE [Id]=@Id;";
            c.CommandText = $"UPDATE [{table.Name}] SET IsDeleted=@d WHERE [Id]=@id;";
            if (permanent) c.CommandText = $"DELETE FROM {table.Name} WHERE [Id]=@id;";
            c.Parameters.AddWithValue("@d", true);
            c.Parameters.AddWithValue($"@Id",id);
            c.ExecuteNonQuery();
        }

        internal static void DeleteItem<T>(T model) where T : ModelBase<T>
        {
            if (model.Id == 0) return;
                Delete<T>(model.Id);
        }


        public static List<string> Distinct<T>(string column) where T:ModelBase<T>
        {
            var c = Connection.CreateCommand();
            c.CommandText = $"SELECT DISTINCT({column}) FROM {GetTable<T>().Name}";
            var list = new List<string>();
            var r = c.ExecuteReader();
            while (r.Read())
            {
                list.Add(r[0].ToString());
            }
            r.Close();
            return list;
        }
    }
}
