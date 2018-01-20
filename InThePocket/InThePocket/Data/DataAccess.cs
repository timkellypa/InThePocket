using System.IO;
using SQLite;

using System;

using System.Threading.Tasks;

using InThePocket.Data.Model;

using InThePocket.Utils;
using System.Collections.Generic;

namespace InThePocket.Data
{
    public static partial class DataAccess
    {
        private static SQLiteAsyncConnection _database;
        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (_database == null)
                {
                    var sqliteFilename = "InThePocket6.db3";
                    string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                    var path = Path.Combine(documentsPath, sqliteFilename);
                    // Create the connection
                    _database = new SQLiteAsyncConnection(path);
                }
                return _database;
            }
        }

        public async static Task Init()
        {
            await InitTables();
        }

        static async Task InitTables()
        {
            List<Type> types = LanguageUtils.GetTypesInNamespace("InThePocket.Data.Model");
            types.Remove(typeof(ModelBase));
            types.Remove(typeof(IModel));
            types.Remove(typeof(ISortableModel));
            types.Remove(typeof(SortableModelBase));
            await Database.CreateTablesAsync(CreateFlags.None, types.ToArray());
            foreach (Type type in types)
            {
                object instance = Activator.CreateInstance(type, null);
                type.GetMethod("InitTable")?.Invoke(instance, null);
            }
        }
    }
}
