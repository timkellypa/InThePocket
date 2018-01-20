using SQLite;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace InThePocket.Data.Model
{
    public class SongSetSong : SortableModelBase
    {
        [Column("SongId")]
        public Guid SongId { get; set; }

        [Column("SongSetId")]
        public Guid SongSetId { get; set; }

        [Ignore]
        public Song Song { get; set; }

        public async Task LoadSong()
        {
            if (Song == null)
            {
                Song = await DataAccess.GetSongById(SongId);
            }
        }

        public override async Task Save()
        {
            await base.Save();
        }

        public override async Task Delete()
        {
            await base.Delete();

            // If all song set associations with this song are gone, remove the song reference as well
            if ((await DataAccess.GetSongSetSongs(SongId, null)).Count() == 0)
            {
                await LoadSong();
                await Song.Delete();
            }
        }
    }
}
