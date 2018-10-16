using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class BookBC
    {
        public int BookId { get; set; }
        public int  Isbn { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int NumPages { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int YearPublished { get; set; }
        public string Language { get; set; }
        public int NumberOfCopies { get; set; }
    }
}
