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
                                        AddBook(context);
                                        break; //case 4 break
                                    case 5:
                                        UpdateBook(context);
                                        break; //case 5 break
                                    case 6:
                                        RemoveBook(context);
                                        break; //case 6 break
                                    case 7:
                                        DisplayLists(context);
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
                                    where e.BookID == bookID
                                    select e).ToList();

                if (cardHolderData.Count > 0)
                {
                    if (CheckOutData.Count > 0)
                    {
                        //Loop through each checkout log
                        foreach (CheckOutLog cod in CheckOutData)
                        {
                            //verify the CardHolderID and the Book ID match in the log
                            if (cod.CardholderID == cardHolderData[0].CardHolderID && cod.BookID == bookID)
                            {
                                //send delete to CheckOutLog
                                //var d = new CheckOutLog { CheckOutLogID = cod.CheckOutLogID };
                                context.CheckOutLogs.Remove(cod);
                                context.SaveChanges();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Successfully checked in book.");
                                Console.WriteLine("\nPress enter to continue.");
                                Console.ReadLine();
                                Console.Clear();
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The selected book is not checked out to the card holder.");
                        Console.WriteLine("\nPress enter to continue.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Card holder not found in the database, please try again.");
                    Console.WriteLine("\nPress enter to continue.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Book not found in the database, please try again.");
                Console.WriteLine("\nPress enter to continue.");
                Console.ReadLine();
                Console.Clear();
            }
        }
        private static void CheckOutBook(LibraryInformationEntities context)
        {
            string checkOut = "out";
            //Creates a helper to get the ISBN and CardID from the librarian
            CheckInOutHelper helper = CheckInOutHelper(checkOut);

            //Finds the book of the ISBN entered
            var bookData = (from e in context.Books
                            where e.ISBN == helper.Isbn
                            select e).ToList();

            if (bookData.Count > 0)
            {
                //check if copies of the book are in stock
                BookBC book = new BookBC();
                int booksInStock = book.CountInStockBooks(bookData[0]);
                if (booksInStock > 0)
                {
                    //Finds the card holder of the CardID entered
                    var cardHolderData = (from e in context.Cardholders
                                          where e.LibraryCardID == helper.CardID
                                          select e).ToList();

                    //finds the checkout log entries for the card holder
                    int cardholderID = cardHolderData[0].CardHolderID;
                    var CheckOutData = (from e in context.CheckOutLogs
                                        where e.CardholderID == cardholderID
                                        select e).ToList();

                    foreach (CheckOutLog col in CheckOutData)
                    {
                        DateTime time = DateTime.Now;
                        //check if user has overdue book (90 days)
                        if (col.CheckOutDate > time.AddDays(-90))
                        {
                            //Allow book to be checked out

                            //Stage the changes to the bookToBeCheckedOut CheckOutLog object
                            CheckOutLog bookToBeCheckedOut = new CheckOutLog
                            {
                                BookID = bookData[0].BookID,
                                CardholderID = cardholderID,
                                CheckOutDate = DateTime.Now
                            };

                            //Add the record to the database and save
                            context.CheckOutLogs.Add(bookToBeCheckedOut);
                            context.SaveChanges();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{cardHolderData[0].Person.FirstName} {cardHolderData[0].Person.LastName} has an overdue book and is not allowed to checkout books at this time.\n");
                            Console.WriteLine("\nPress enter to continue.");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{book.Title} is not in stock, please check back later.");
                    Console.WriteLine("\nPress enter to continue.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Book not found in the database, please try again.");
                Console.WriteLine("\nPress enter to continue.");
                Console.ReadLine();
                Console.Clear();
            }
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
        private static void AddBook(LibraryInformationEntities context)
        {
            Book book = new Book();
            Console.Write("Enter the ISBN: ");
            book.ISBN = Console.ReadLine();

            var bookData = (from e in context.Books
                            where e.ISBN == book.ISBN
                            select e).ToList();

            if (bookData.Count > 0)
            {
                Console.WriteLine("Book is already in the system.");
                Console.Write("Enter the number of copies you wish to add: ");
                string strCopies = Console.ReadLine();
                int.TryParse(strCopies, out int intCopies);
                bookData[0].NumberOfCopies = bookData[0].NumberOfCopies + intCopies;
                context.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Successfully added {intCopies} copies.");
                Console.WriteLine("\nPress enter to continue.");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.Write("Enter the book title: ");
                book.Title = Console.ReadLine();
                Console.Write("Enter the author's first name: ");
                string authorFirstName = Console.ReadLine();
                Console.Write("Enter the author's Last name: ");
                string authorLastName = Console.ReadLine();
                //Do something to find existing author or create new author
                book.AuthorID = 8;
                Console.Write("Enter the book's page count: ");
                string strNumPages = Console.ReadLine();
                int.TryParse(strNumPages, out int intNumPages);
                book.NumPages = intNumPages;
                Console.Write("Enter the book's subject: ");
                book.Subject = Console.ReadLine();
                Console.Write("Enter the book's description: ");
                book.Description = Console.ReadLine();
                Console.Write("Enter the book's publisher: ");
                book.Publisher = Console.ReadLine();
                Console.Write("Enter the year the book was published: ");
                book.YearPublished = Console.ReadLine();
                //need to validate that only 4 digits are entered for year published
                Console.Write("Enter the book's language: ");
                book.Language = Console.ReadLine();
                Console.Write("Enter the number of books: ");
                string strCopies = Console.ReadLine();
                int.TryParse(strCopies, out int intCopies);
                book.NumberOfCopies = intCopies;

                //Add the book to the database and save
                context.Books.Add(book);
                context.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Successfully added book to the system.");
                Console.WriteLine("\nPress enter to continue.");
                Console.ReadLine();
                Console.Clear();
            }
        }
        private static void UpdateBook(LibraryInformationEntities context)
        {
            throw new NotImplementedException();
        }
        private static void RemoveBook(LibraryInformationEntities context)
        {
            throw new NotImplementedException();
        }
        private static void DisplayLists(LibraryInformationEntities context)
        {
            throw new NotImplementedException();
        }
    }
}
