using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class CheckOutLogBC
    {
        public int CheckOutLogId { get; set; }
        public int CardholderId { get; set; }
        public int BookId { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
