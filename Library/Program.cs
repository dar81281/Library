using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Library
{
    class Program
    {
        private static void Main(string[] args)
        {
            //connect to database context
            var context = new LibraryInformationEntities();
            //var data = (from e in context.Authors
            //           select e).ToList();           

            //variables main will use
            int mainChoice;
            int librarianChoice;

            //do while loop to display the main menu
            do
            {
                //display the main menu to allow a user to input an option
                mainChoice = MainMenu();

                //clear the console
                Console.Clear();

                //main switch to decide which option a user has chosen
                switch (mainChoice)
                {
                    case 1:
                        //Find feature
                        FindMenu(context);
                            break; //case 1 break
                    case 2:
                        //Librarian login feature
                        bool login = Login(context);

                        if (login)
                        {
                            //do while to display the librarian menu
                            do
                            {
                                //display the librarian menu to allow a user to input an option
                                librarianChoice = LibrarianMenu();

                                //clear the console
                                Console.Clear();

                                //inner switch to decide which option a librarian has chosen
                                switch (librarianChoice)
                                {
                                    case 1:
                                        FindMenu(context);
                                        break; //case 1 break
                                    case 2:
                                        CheckOutBook(context);
                                        break; //case 2 break
                                    case 3:
                                        CheckInBook(context);
                                        break; //case 3 break
                                    case 4:
                                        Console.WriteLine("Implement option 4");
                                        break; //case 4 break
                                    case 5:
                                        Console.WriteLine("Implement option 5");
                                        break; //case 5 break
                                    case 6:
                                        Console.WriteLine("Implement option 6");
                                        break; //case 6 break
                                    case 7:
                                        Console.WriteLine("Implement option 7");
                                        break; //case 7 break
                                    case 8:
                                        //log-off
                                        break; //case 8 break
                                    default:
                                        //Display error
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid option, please try again.");
                                        break; //default break
                                }
                            } while (librarianChoice != 8);
                        }
                        else
                        {
                            //Login failed output message to user
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Login Failed");
                        }
                            break; //case 2 break
                    case 3:
                        //Close the program
                            break; //case 3 break
                    default:
                        //Display error
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option, please try again.");
                            break; // default break
                }
            } while (mainChoice != 3);
        }
        private static int MainMenu()
        {
            string userInput;

            //Change the console color to white to indicate a patron user
            Console.ForegroundColor = ConsoleColor.White;

            //Display main menu
            Console.WriteLine("1. Find a book by Title, Author, Subject, or ISBN.");
            Console.WriteLine("2. Librarian Login.");
            Console.WriteLine("3. Exit the program.");
            Console.Write("Please Choose An Option (1-3): ");

            //Get user input
            userInput = Console.ReadLine();

            //verify the user input is an int
            int.TryParse(userInput, out int selection);
            return selection;
        }
        private static int LibrarianMenu()
        {
            string userInput;

            //Change the console color to yellow to indicate a librarian user
            Console.ForegroundColor = ConsoleColor.Yellow;

            //Display librarian menu
            Console.WriteLine("1. Find a book by Title, Author, Subject, or ISBN.");
            Console.WriteLine("2. Check out a book.");
            Console.WriteLine("3. Check in a book.");
            Console.WriteLine("4. Add book(s) to the system.");
            Console.WriteLine("5. Update an existing book in the system.");
            Console.WriteLine("6. Remove book(s) from the system.");
            Console.WriteLine("7. Display list of Librarians, Cardholders, Authors, and Overdue Books.");
            Console.WriteLine("8. Log-off.");
            Console.Write("Please Choose An Option (1-8): ");

            //Get user input
            userInput = Console.ReadLine();

            //verify the user input is an int
            int.TryParse(userInput, out int selection);
            return selection;
        }
        private static void FindMenu(LibraryInformationEntities context)
        {
            //get the search term from the user
            Console.Write("Enter a search term: ");
            string search = Console.ReadLine();
            int selection = 0;
            do
            {
                bool booksFound = FindBook(context, search);
                //Console.WriteLine("Implement the find feature");
                if (booksFound)
                {
                    Console.Write("\nSelect a book to display more information (0 to exit): ");
                    //Get user input
                    string userInput = Console.ReadLine();
                    //verify the user input is an int
                    int.TryParse(userInput, out selection);

                    //search for the book that the user entered
                    if (selection != 0)
                    {
                        var data = (from e in context.Books
                                    where e.BookID == selection
                                    select e).ToList();
                        Book book = data[0];
                        BookBC bookBc = new BookBC();
                        bookBc.DetailedDisplay(book);
                        Console.WriteLine("\nPress enter to continue.");
                        Console.ReadLine();
                    }
                }
                Console.Clear();
            } while (selection > 0);
        }

        private static bool FindBook(LibraryInformationEntities context, string SearchTerm)
        {
            BookBC book = new BookBC();
            List<Book> foundBooks = book.Search(context, SearchTerm);
            if (foundBooks.Count > 0)
            {
                //Display the found books
                book.Display(foundBooks);
                return true;
            }
            else
            {
                //Display a message when no books are found
                Console.WriteLine($"No books with {SearchTerm} found");
                Console.WriteLine("\nPress enter to continue.");
                Console.ReadLine();
                return false;
            }
        }
        //maybe want to create login class so I can return object with user name and loginStatus?
        private static bool Login(LibraryInformationEntities context)
        {
            //assume login fails
            bool successfulLogin = false;

            //Get user name and password from librarian
            Console.Write("Enter user name: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            Console.Clear();

            //Query SQL for login information
            var data = (from e in context.Librarians
                        where e.UserID == username
                        select e).ToList();

            //Check to see if the query above returned a value
            if (data.Count != 0)
            {
                //if user name and password entered equal something from the database return true
                if (username == data[0].UserID && password == data[0].Password)
                {
                    //set return value to true
                    successfulLogin = true;
                }
            }

            //return value to caller
            return successfulLogin;
        }
        private static void CheckInBook(LibraryInformationEntities context)
        {
            string checkIn = "in";
            //Creates a helper to get the ISBN and CardID from the librarian
            CheckInOutHelper helper = CheckInOutHelper(checkIn);

            //Finds the book of the ISBN entered
            var bookData = (from e in context.Books
                            where e.ISBN == helper.Isbn
                            select e).ToList();

            //Finds the card holder of the CardID entered
            var cardHolderData = (from e in context.Cardholders
                                  where e.LibraryCardID == helper.CardID
                                  select e).ToList();

            if (bookData.Count > 0)
            {
                //Finds the log entry(s) of the books checked out
                int bookID = bookData[0].BookID;
                var CheckOutData = (from e in context.CheckOutLogs
                                    where e.BookID == (bookID)
                                    select e).ToList();

                if (cardHolderData.Count > 0)
                {
                    //Loop through each checkout log
                    foreach (CheckOutLog cod in CheckOutData)
                    {
                        //verify the CardHolderID and the Book ID match in the log
                        if (cod.CardholderID == cardHolderData[0].CardHolderID && cod.BookID == bookID)
                        {
                            //send delete to CheckOutLog
                            var d = new CheckOutLog { CheckOutLogID = cod.CardholderID };
                            context.CheckOutLogs.Remove(d);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Card holder not found in the database, please try again");
                    Console.ReadLine();
                }

                if (CheckOutData.Count > 0)
                {
                    CheckOutLogBC cil = new CheckOutLogBC();
                }
            }
            else
            {
                Console.WriteLine("Book not found in the database, please try again");
                Console.ReadLine();
            }
        }
        private static void CheckOutBook(LibraryInformationEntities context)
        {
            string checkOut = "out";
            CheckInOutHelper helper = CheckInOutHelper(checkOut);
        }
        private static CheckInOutHelper CheckInOutHelper(string checkInOut)
        {
            Console.Write("Enter the card holder's ID: ");
            string cardId = Console.ReadLine();
            Console.Write($"Enter the ISBN of the book to be checked {checkInOut}: ");
            string isbn = Console.ReadLine();

            CheckInOutHelper helper = new CheckInOutHelper(cardId, isbn);

            return helper;
        }
    }
}
