using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using InThePocket.Data.Model;

namespace InThePocket.Data
{
    public partial class DataAccess
    {
        public static async Task<List<SongSetSong>> GetSongSetSongs(Guid? songId, Guid? songSetId, bool recurse = false)
        {
            List<SongSetSong> songSetSongList = (from songSetSong in (await Database.Table<SongSetSong>().ToListAsync())
                                                 where (
                                                    (songId == null || songSetSong.SongId == songId) &&
                                                    (songSetId.HasValue || songSetSong.SongSetId == songSetId)
                                                 )
                                                 orderby songSetSong.OrderNdx
                                                 select songSetSong).ToList();

            if (recurse)
            {
                foreach (SongSetSong songSetSong in songSetSongList)
                {
                    await songSetSong.LoadSong();
                }
            }
            return songSetSongList;
        }
    }
}
