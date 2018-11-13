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

        public List<Book> Search(LibraryInformationEntities context, string lookupValue)
        {
            //Search for the lookup value in Title, ISBN, Subject, FirstName or LastName
            var bookDataList = (from e in context.Books
                                where e.Title.Contains(lookupValue)
                                || e.ISBN.Contains(lookupValue)
                                || e.Subject.Contains(lookupValue)
                                || e.Author.Person.FirstName.Contains(lookupValue)
                                || e.Author.Person.LastName.Contains(lookupValue)
                                select e).ToList();
            return bookDataList;
        }
        public void Display(List<Book> books)
        {
            //Display column headers
            Console.WriteLine("{0,-8}{1,-25}{2,-20}{3,-15}{4,4}", "Book ID", "Title", "Author Name", "Publisher", "Year Published");
            //Display each book aligned with the column headers
            foreach (Book book in books)
            {
                Console.WriteLine("{0,-8}{1,-25}{2,-20}{3,-15}{4,4}", book.BookID, book.Title, book.Author.Person.FirstName + " " + book.Author.Person.LastName, book.Publisher, book.YearPublished.ToString());
            }
        }
    }
}
