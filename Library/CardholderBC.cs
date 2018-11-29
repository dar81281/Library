using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class CardholderBC : PersonBC
    {
        public int CardholderId { get; set; }
        public int Phone { get; set; }
        public string LibraryCardId { get; set; }

        public void Display(List<Cardholder> cardholders, List<CheckOutLog> checkOutLogs)
        {
            //Display column headers
            Console.WriteLine("Cardholder ID, FirstName, Last Name, Library Card ID, Phone");
            //Display each book aligned with the column headers
            foreach (Cardholder ch in cardholders)
            {
                //Set the values to what is in col
                int cardholderID = ch.CardHolderID;
                string chFirstName = ch.Person.FirstName;
                string chLastName = ch.Person.LastName;
                string libraryCard = ch.LibraryCardID;
                string phone = ch.Phone;
                Console.WriteLine($"{cardholderID}, {chFirstName}, {chLastName}, {phone}");

                List<Book> books = new List<Book>();

                if (checkOutLogs.Count > 0)
                {
                    foreach (CheckOutLog col in checkOutLogs)
                    {
                        if(col.CardholderID == cardholderID)
                        {
                            books.Add(col.Book);
                        }
                    }
                    BookBC book = new BookBC();
                    if (books.Count > 0)
                    {
                        book.Display(books);
                    }
                    else
                    {
                        Console.WriteLine("No books checked out by this cardholder");
                    }
                }
                else
                {
                    Console.WriteLine("No books to display.");
                }
            }
        }
    }
}
