using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class CheckInOutHelper 
    {
        public string CardID { get; set; }
        public string Isbn { get; set; }

        public CheckInOutHelper(string cardID, string isbn )
        {
            CardID = cardID;
            Isbn = isbn;
        }
    }
    
}
