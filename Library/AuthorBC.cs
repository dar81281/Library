using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class AuthorBC : PersonBC
    {
        public int AuthorID { get; set; }
        public string Bio { get; set; }
    }
}
