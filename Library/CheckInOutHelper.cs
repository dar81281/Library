using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class CheckInOutHelper 
    {
        public int CardID { get; set; }
        public int Isbn { get; set; }

        public CheckInOutHelper(int cardID, int isbn )
        {
            CardID = cardID;
            Isbn = isbn;
        }
    }
    
}
