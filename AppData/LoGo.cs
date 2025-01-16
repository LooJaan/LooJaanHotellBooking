using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class LoGo
    {
        public int ID { get; set; }
     
        public string? Logo  { get; set; }
        public bool TrangThai { get; set; }

    }
}
