using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using SQLite;
using System.Threading.Tasks;

namespace InThePocket.Data.Model
{
    [Table("Song")]
    public class Song : ModelBase
    {
        [Column("Name")]
        public string Name { get; set; }

        public override async Task Delete()
        {
            await base.Delete();

            // If all song set associations with this song are gone, remove the song reference as well
            List<SongTempo> songTempos = await DataAccess.GetSongTemposForSong(Id);
            foreach (SongTempo tempo in songTempos)
            {
                await tempo.Delete();
            }
        }
    }
}
