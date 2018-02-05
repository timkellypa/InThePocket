using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using InThePocket.Data.Model;

using System.Linq;
using SQLite;

namespace InThePocket.Data
{
    public partial class DataAccess
    {
        public static async Task<List<SongSet>> GetSongSetList()
        {
            // Sort by event lists first, then master lists.
            List<SongSet> songSetList = (from songSet in await (Database.Table<SongSet>()).ToListAsync()
                                         orderby songSet.IsMaster, songSet.OrderNdx
                                        select songSet).ToList();
            return songSetList;
        }

        public static async Task<SongSet> GetSongSetById(Guid id)
        {
            return await Database.GetAsync<SongSet>(id);
        }
    }
}
