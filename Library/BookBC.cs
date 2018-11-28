using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BookBC
    {
        public int BookId { get; set; }
        public string  Isbn { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int NumPages { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
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
            //Console.WriteLine("{0,-8}{1,-25}{2,-20}{3,-15}{4,4}", "Book ID", "Title", "Author Name", "Publisher", "Year Published");
            Console.WriteLine("Book ID, Title, Author Name, Publisher, Year Published, Available for Checkout");
            //Display each book aligned with the column headers
            foreach (Book book in books)
            {
                //Set publisher and yearPublished to N/A by default for null protection
                string publisher = "N/A";
                string yearPublished = "N/A";
                //Set the values to what is in book
                string bookId = book.BookID.ToString();
                string bookTitle = book.Title.ToString();
                string authorFirstName = book.Author.Person.FirstName.ToString();
                string authorLastName = book.Author.Person.LastName.ToString();
                //Add null protection to Publisher and YearPublished
                if (book.Publisher != null)
                    publisher = book.Publisher.ToString();
                if (book.YearPublished != null)
                    yearPublished = book.YearPublished.ToString();

                //Check to see how many copies are in stock
                int copiesInStock = CountInStockBooks(book);
                //Assume no copies available
                string availableForCheckout = "No";
                //If there are more than 0 copies available change variable to Yes
                if (copiesInStock > 0)
                    availableForCheckout = "Yes";


                //Console.WriteLine("{0,-8}{1,-25}{2,-20}{3,-15}{4,4}", book.BookID, book.Title, book.Author.Person.FirstName + " " + book.Author.Person.LastName, book.Publisher, book.YearPublished.ToString());
                Console.WriteLine($"{bookId}. {bookTitle}, {authorFirstName} {authorLastName}, {publisher}, {yearPublished}, {availableForCheckout}");
            }
        }
        public void DetailedDisplay(Book book)
        {
            Console.WriteLine($"Title: {book.Title}");
            Console.WriteLine($"Author: {book.Author.Person.FirstName} {book.Author.Person.LastName}");
            Console.WriteLine($"Number of pages: {book.NumPages}");
            Console.WriteLine($"Subject: {book.Subject}");
            Console.WriteLine($"Description: {book.Description}");
            Console.WriteLine($"Publisher: {book.Publisher}");
            Console.WriteLine($"Year Published: {book.YearPublished}");
            Console.WriteLine($"Languages Supported: {book.Language}");
            Console.WriteLine($"Number of copies available: {this.CountInStockBooks(book)}");
            Console.WriteLine($"Total number of copies: {book.NumberOfCopies}");
        }
        public int CountInStockBooks(Book book)
        {
            int numCopies = book.NumberOfCopies;
            var context = new LibraryInformationEntities();
            var data = (from e in context.CheckOutLogs
                       select e).ToList();
            foreach (var d in data)
            {
                if (d.BookID == book.BookID)
                {
                    numCopies--;
                }
            }
            return numCopies;
        }
        //public static implicit operator BookBC(Book v)
        //{
        //    BookBC bookBC = new BookBC();
        //    bookBC = v;
        //    return bookBC;
        //}
    }
}
