using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NodeBlock.Engine.Storage.MariaDB.Entities
{
    [Table("wallets")]
    public class Wallet
    {
        [Key]
        public int id_wallet { get; set; }

        public decimal due_balance { get; set; }

        public string signed_jwt { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public string public_address { get; set; }
    }
}
