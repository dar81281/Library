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

        public void Display(List<CheckOutLog> CheckOutLog)
        {
            //Display column headers
            Console.WriteLine("Book ID, Title, Check Out Date, Cardholder ID, FirstName, Last Name, Library Card ID, Phone");
            //Display each book aligned with the column headers
            foreach (CheckOutLog col in CheckOutLog)
            {
                //Set the values to what is in col
                string bookId = col.BookID.ToString();
                string isbn = col.Book.ISBN;
                string bookTitle = col.Book.Title.ToString();
                string checkOutDate = col.CheckOutDate.ToString();
                string cardHolderID = col.CardholderID.ToString();
                string cardHolderFirstName = col.Cardholder.Person.FirstName;
                string cardHolderLastName = col.Cardholder.Person.LastName;
                string libraryCardID = col.Cardholder.LibraryCardID;
                string phone = col.Cardholder.Phone;

                Console.WriteLine($"{bookId}. {isbn}, {bookTitle}, {checkOutDate}, {cardHolderID}, {cardHolderFirstName}, {cardHolderLastName}, {libraryCardID}, {phone}");
            }
        }
    }
}
