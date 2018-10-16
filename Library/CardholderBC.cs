using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class CardholderBC : PersonBC
    {
        public int CardholderId { get; set; }
        public int Phone { get; set; }
        public string LibraryCardId { get; set; }
    }
}
