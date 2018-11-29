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

        public void Display(List<Author> authors, List<Book> books)
        {
            //Display column headers
            Console.WriteLine("Author ID, FirstName, Last Name");
            //Display each book aligned with the column headers
            foreach (Author author in authors)
            {
                //Set the values to what is in col
                int authorID = author.AuthorID;
                string authorFirstName = author.Person.FirstName;
                string authorLastName = author.Person.LastName;
                Console.WriteLine($"{authorID}, {authorFirstName}, {authorLastName}");

                List<Book> b = new List<Book>();

                if (books.Count > 0)
                {
                    foreach (Book bo in books)
                    {
                        if (bo.AuthorID == authorID)
                        {
                            b.Add(bo);
                        }
                    }
                    BookBC book = new BookBC();
                    if (b.Count > 0)
                    {
                        book.Display(b);
                    }
                    else
                    {
                        Console.WriteLine("No books by this author");
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
