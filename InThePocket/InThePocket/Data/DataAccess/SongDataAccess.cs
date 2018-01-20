using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using System.Text;

using InThePocket.Data.Model;
using SQLite;

namespace InThePocket.Data
{
    public partial class DataAccess
    {
        public static async Task<List<Song>> GetSongListBySetId(Guid? songSetID)
        {
            List<Guid> songIdList = (from songSetSong in (await GetSongSetSongs(null, songSetID))
                                   select songSetSong.SongId).ToList();

            List<Song> songList = (from song in (await Database.Table<Song>().ToListAsync())
                                   where songIdList.Contains(song.Id)
                                   select song).ToList();
            return songList;
        }

        public static async Task<Song> GetSongById(Guid id)
        {
            return await Database.GetAsync<Song>(id);
        }
    }
}
