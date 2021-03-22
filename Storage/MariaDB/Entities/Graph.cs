using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NodeBlock.Engine.Storage.MariaDB.Entities
{
    [Table("graphs")]
    public class Graph
    {
        [Key]
        public int id_graph { get; set; }

        public string hash_graph { get; set; }

        public string alias { get; set; }

        public int wallet_owner { get; set; }

        public string last_loaded_bytes { get; set; }

        public DateTime created_at { get; set; }

        public DateTime loaded_at { get; set; }

        public decimal total_cost { get; set; }

        public decimal last_execution_cost { get; set; }
    }
}
