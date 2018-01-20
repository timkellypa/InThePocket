using System;
using System.Collections.Generic;
using System.Text;

using SQLite;

namespace InThePocket.Data.Model
{
    [Table("Artist")]
    class Artist : ModelBase
    {
        [Column("Name")]
        public string Name { get; set; }
    }
}
