using System;

namespace Libro.Data
{
    class DbColumn
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string DbType  { get; set; }
        public string DefaultValue { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string Options { get; set; }

        public object Cast(object o)
        {
            if (o == null) return GetDefault(o);
            return o;
        }

        private object GetDefault(object o)
        {
            if (o is string) return string.Empty;
            if (o is double) return 0f;
            if (o is long) return 0L;
            return o;
        }
    }

    class ColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string DbType { get; set; }
        public string DefaultValue { get; set; }
        public bool IsPrimaryKey { get; set; }
    }

    class PrimaryKeyAttribute : Attribute
    {
        
    }

    class DefaultValueAttribute : Attribute
    {
        public DefaultValueAttribute(string defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public string DefaultValue { get; set; }
    }

    class ColumnTypeAttribute:Attribute
    {
        public ColumnTypeAttribute(string dbType)
        {
            DbType = dbType;
        }
        public string DbType { get; set; }
    }

    class ForeignKeyAttribute:Attribute
    {
        public ForeignKeyAttribute(string table)
        {
            Table = table;
        }

        public string Table { get; set; }
    }

    class IgnoreAttribute : Attribute { }
}
