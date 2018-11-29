using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class LibrarianBC : PersonBC
    {
        public int LibrariansId { get; set; }
        public int Phone { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

        public void Display(List<Librarian> librarians)
        {
            //Display column headers
            Console.WriteLine("Librarian ID, FirstName, Last Name, Phone");
            //Display each book aligned with the column headers
            foreach (Librarian lib in librarians)
            {
                //Set the values to what is in col
                string userID = lib.UserID;
                string librarianFirstName = lib.Person.FirstName;
                string librarianLastName = lib.Person.LastName;
                string phone = lib.Phone;

                Console.WriteLine($"{userID}, {librarianFirstName}, {librarianLastName}, {phone}");
            }
        }
    }
}
