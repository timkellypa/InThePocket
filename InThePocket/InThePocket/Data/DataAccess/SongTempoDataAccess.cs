using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using System.Linq;

using InThePocket.Data.Model;
using SQLite;

namespace InThePocket.Data
{
    public partial class DataAccess
    {
        public static async Task<List<SongTempo>> GetSongTemposForSong(Guid songId)
        {
            List<SongTempo> tempoSet = (from tempo in (await Database.Table<SongTempo>().ToListAsync())
                                                   where tempo.SongId == songId
                                                   orderby tempo.OrderNdx
                                                   select tempo).ToList();
            return tempoSet;
        }

        public static async Task<SongTempo> GetSongTempoById(Guid id)
        {
            return await Database.GetAsync<SongTempo>(id);
        }
    }
}
