using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class LibrarianBC : PersonBC
    {
        public int LibrariansId { get; set; }
        public int Phone { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
