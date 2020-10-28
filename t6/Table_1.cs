namespace t6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Table_1
    {

        public int id { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public bool? sex { get; set; }

        [StringLength(50)]
        public string address { get; set; }

        public byte? type { get; set; }
    }
}
