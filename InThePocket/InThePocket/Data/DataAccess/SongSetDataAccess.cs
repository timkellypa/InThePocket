using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using InThePocket.Data.Model;
using SQLite;

namespace InThePocket.Data
{
    public partial class DataAccess
    {
        public static async Task<List<SongSet>> GetSongSetList()
        {
            AsyncTableQuery<SongSet> songSet = Database.Table<SongSet>();
            return await songSet.ToListAsync();
        }

        public static async Task<SongSet> GetSongSetById(Guid id)
        {
            return await Database.GetAsync<SongSet>(id);
        }
    }
}
