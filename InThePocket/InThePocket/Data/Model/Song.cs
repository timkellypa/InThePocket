using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using SQLite;

namespace InThePocket.Data.Model
{
    [Table("Song")]
    public class Song : ModelBase
    {
        [Column("Name")]
        public string Name { get; set; }

        [Column("Notes")]
        public string Notes { get; set; }
    }
}
