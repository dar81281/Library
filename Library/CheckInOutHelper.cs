using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class CheckInOutHelper 
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
