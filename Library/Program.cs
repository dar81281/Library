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
                        if (data.Count > 0)
                        {
                            Book book = data[0];
                            BookBC bookBc = new BookBC();
                            bookBc.DetailedDisplay(book);
                        }
                        else
                        {
                            ConsoleColor foreground = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Book is not found in the database, please try again.");
                            Console.ForegroundColor = foreground;
                            //Console.WriteLine("\nPress enter to continue.");
                            //Console.ReadLine();
                            //Console.Clear();
                            selection = -1;
                        }
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

                    if (CheckOutData.Count > 0)
                    {
                        foreach (CheckOutLog col in CheckOutData)
                        {
                            DateTime time = DateTime.Now;
                            //check if user has overdue book (30 days)
                            if (col.CheckOutDate > time.AddDays(-30))
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
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Successfully checked out book.");
                                Console.WriteLine("\nPress enter to continue.");
                                Console.ReadLine();
                                Console.Clear();
                                break;
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
                        //user has no checked out books and is allowed to checkout the book
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
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Successfully checked out book.");
                        Console.WriteLine("\nPress enter to continue.");
                        Console.ReadLine();
                        Console.Clear();
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
        public static CheckInOutHelper CheckInOutHelper(string checkInOut)
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
                Author a = CreateAuthor(context, authorFirstName, authorLastName);
                book.AuthorID = a.AuthorID;
                //var authorData = (from e in context.Authors
                //                where e.Person.FirstName == authorFirstName
                //                && e.Person.LastName == authorLastName
                //                select e).ToList();

                //if (authorData.Count > 0)
                //{
                //    Author a = authorData[0];
                //    book.AuthorID = a.AuthorID;
                //}
                //else
                //{
                //    var peopleData = (from e in context.People
                //                      where e.FirstName == authorFirstName
                //                      && e.LastName == authorLastName
                //                      select e).ToList();
                //    Console.Write($"Author not found, please enter a Biography for {authorFirstName} {authorLastName}: ");
                //    string bio = Console.ReadLine();
                //    Author author;
                //    if (peopleData.Count > 0)
                //    {
                //        Person p = peopleData[0];
                //        author = new Author(p, bio);
                //    }
                //    else
                //    {
                //        Person p = new Person(authorFirstName, authorLastName);
                //        context.People.Add(p);
                //        context.SaveChanges();
                //        author = new Author(p, bio);
                //    }
                //    //Add the author to the database and save
                //    context.Authors.Add(author);
                //    context.SaveChanges();
                //    ConsoleColor foreground = Console.ForegroundColor;
                //    Console.ForegroundColor = ConsoleColor.Green;
                //    Console.WriteLine($"Successfully added author to the system.");
                //    Console.ForegroundColor = foreground;
                //    book.AuthorID = author.AuthorID;
                //}

                do
                {
                    Console.Write("Enter the book's page count: ");
                    string strNumPages = Console.ReadLine();
                    int.TryParse(strNumPages, out int intNumPages);
                    book.NumPages = intNumPages;
                    if (book.NumPages == 0)
                    {
                        ConsoleColor foreground = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid entry, please only use numbers.");
                        Console.ForegroundColor = foreground;
                    }
                } while (book.NumPages == 0);
                Console.Write("Enter the book's subject: ");
                book.Subject = Console.ReadLine();
                Console.Write("Enter the book's description: ");
                book.Description = Console.ReadLine();
                Console.Write("Enter the book's publisher: ");
                book.Publisher = Console.ReadLine();
                do
                {
                    Console.Write("Enter the year the book was published: ");
                    book.YearPublished = Console.ReadLine();
                    if (book.YearPublished.Length != 4)
                    {
                        ConsoleColor foreground = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Year published must be 4 digits.  Please enter a new year.");
                        Console.ForegroundColor = foreground;
                    }
                } while (book.YearPublished.Length != 4);
                Console.Write("Enter the book's language: ");
                book.Language = Console.ReadLine();
                do
                {
                    Console.Write("Enter the number of books: ");
                    string strCopies = Console.ReadLine();
                    int.TryParse(strCopies, out int intCopies);
                    book.NumberOfCopies = intCopies;
                    if (book.NumberOfCopies == 0)
                    {
                        ConsoleColor foreground = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid entry, please only use numbers.");
                        Console.ForegroundColor = foreground;
                    }
                } while (book.NumberOfCopies == 0);

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
            var bookData = (from e in context.Books
                            select e).ToList();
            BookBC book = new BookBC();
            book.Display(bookData);

            Console.Write("\nSelect a book to update (0 to exit): ");
            //Get user input
            string userInput = Console.ReadLine();
            //verify the user input is an int
            int.TryParse(userInput, out int selection);

            //search for the book that the user entered
            if (selection != 0)
            {
                var data = (from e in context.Books
                            where e.BookID == selection
                            select e).ToList();

                Book b = data[0];

                Console.Write($"ISBN currently set to {b.ISBN} enter a value if you wish to update (leave blank to ignore): ");
                string tempISBN = Console.ReadLine();
                Console.Write($"Title currently set to {b.Title} enter a value if you wish to update (leave blank to ignore): ");
                string tempTitle = Console.ReadLine();
                Console.Write($"Author currently set to {b.Author.Person.FirstName} {b.Author.Person.LastName} a new first name (leave blank to ignore): ");
                string tempFirstName = Console.ReadLine();
                Console.Write("Enter a new last name (leave blank to ignore): ");
                string tempLastName = Console.ReadLine();
                string first;
                string last;
                if (tempFirstName != "")
                {
                    first = tempFirstName;
                }
                else
                {
                    first = b.Author.Person.FirstName;
                }
                if (tempLastName != "")
                {
                    last = tempLastName;
                }
                else
                {
                    last = b.Author.Person.LastName;
                }
                Author tempAuthor = CreateAuthor(context, first, last);
                int tempIntNumPages;
                do
                {
                    Console.Write($"NumPages currently set to {b.NumPages} enter a value if you wish to update (leave blank to ignore): ");
                    string tempNumPages = Console.ReadLine();

                    if (tempNumPages != "")
                    {
                        int.TryParse(tempNumPages, out tempIntNumPages);
                        if (tempIntNumPages == 0)
                        {
                            ConsoleColor foreground = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid entry, please only use numbers.");
                            Console.ForegroundColor = foreground;
                        }
                    }
                    else
                    {
                        tempIntNumPages = -1;
                        break;
                    }
                } while (tempIntNumPages == 0);
                Console.Write($"Subject currently set to {b.Subject} enter a value if you wish to update (leave blank to ignore): ");
                string tempSubject = Console.ReadLine();
                Console.Write($"Description currently set to {b.Description} enter a value if you wish to update (leave blank to ignore): ");
                string tempDescription = Console.ReadLine();
                Console.Write($"Publisher currently set to {b.Publisher} enter a value if you wish to update (leave blank to ignore): ");
                string tempPublisher = Console.ReadLine();
                string tempYearPublished;
                do
                {
                    Console.Write($"YearPublished currently set to {b.YearPublished} enter a value if you wish to update (leave blank to ignore): ");
                    tempYearPublished = Console.ReadLine();
                    if (tempYearPublished != "")
                    {
                        if (tempYearPublished.Length != 4)
                        {
                            ConsoleColor foreground = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Year published must be 4 digits.  Please enter a new year.");
                            Console.ForegroundColor = foreground;
                        }
                    }
                    else
                    {
                        tempYearPublished = "-1";
                        break;
                    }
                } while (tempYearPublished.Length != 4);
                Console.Write($"Language currently set to {b.Language} enter a value if you wish to update (leave blank to ignore): ");
                string tempLanguage = Console.ReadLine();
                int intTempNumberOfCopies;
                do
                {
                    Console.Write($"NumberOfCopies currently set to {b.NumberOfCopies} enter a value if you wish to update (leave blank to ignore): ");
                    string tempNumberOfCopies = Console.ReadLine();
                    if (tempNumberOfCopies != "")
                    {
                        int.TryParse(tempNumberOfCopies, out intTempNumberOfCopies);
                        if (intTempNumberOfCopies == 0)
                        {
                            ConsoleColor foreground = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid entry, please only use numbers.");
                            Console.ForegroundColor = foreground;
                        }
                        else
                        {
                            BookBC bookBC = new BookBC();
                            int numberOfBooksInStock = bookBC.CountInStockBooks(b);
                            int numberOfBooksCheckedOut = b.NumberOfCopies - numberOfBooksInStock;
                            if(numberOfBooksCheckedOut > intTempNumberOfCopies)
                            {
                                ConsoleColor foreground = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Invalid entry, there are currently {numberOfBooksCheckedOut} of those books checked out.  Please enter a greater number.");
                                Console.ForegroundColor = foreground;
                                //set to 0 to stay in the loop
                                intTempNumberOfCopies = 0;
                            }
                        }
                    }
                    else
                    {
                        intTempNumberOfCopies = -1;
                        break;
                    }
                } while (intTempNumberOfCopies == 0);

                Book tempBook = new Book();
                if (tempISBN != "")
                {
                    tempBook.ISBN = tempISBN;
                }
                else
                {
                    tempBook.ISBN = b.ISBN;
                }
                if (tempTitle != "")
                {
                    tempBook.Title = tempTitle;
                }
                else
                {
                    tempBook.Title = b.Title;
                }
                tempBook.Author = tempAuthor;
                tempBook.AuthorID = tempAuthor.AuthorID;
                //tempBook.Author = b.Author;
                if (tempIntNumPages != -1)
                {
                    tempBook.NumPages = tempIntNumPages;
                }
                else
                {
                    tempBook.NumPages = b.NumPages;
                }
                if (tempSubject != "")
                {
                    tempBook.Subject = tempSubject;
                }
                else
                {
                    tempBook.Subject = b.Subject;
                }
                if (tempDescription != "")
                {
                    tempBook.Description = tempDescription;
                }
                else
                {
                    tempBook.Description = b.Description;
                }
                if (tempPublisher != "")
                {
                    tempBook.Publisher = tempPublisher;
                }
                else
                {
                    tempBook.Publisher = b.Publisher;
                }
                if (tempYearPublished != "-1")
                {
                    tempBook.YearPublished = tempYearPublished;
                }
                else
                {
                    tempBook.YearPublished = b.YearPublished;
                }
                if (tempLanguage != "")
                {
                    tempBook.Language = tempLanguage;
                }
                else
                {
                    tempBook.Language = b.Language;
                }
                if (intTempNumberOfCopies != -1)
                {
                    tempBook.NumberOfCopies = intTempNumberOfCopies;
                }
                else
                {
                    tempBook.NumberOfCopies = b.NumberOfCopies;
                }
                tempBook.BookID = b.BookID;
                string answer;
                do
                {
                    Console.WriteLine("\nYou are going to replace this book:");
                    book.DetailedDisplay(b);
                    Console.WriteLine("\nwith this book:");
                    book.DetailedDisplay(tempBook);
                    Console.Write("\nAre you sure you wish to continue? (enter 'y' to confirm): ");
                    answer = Console.ReadLine();
                    if (answer.ToUpper() == "Y")
                    {
                        //Commit the change to the database
                        //context.Books.Attach(b);
                        //b = tempBook;
                        //context.Entry(b).State = EntityState.Modified;

                        //context.Entry(b).State = b.BookID == 0 ?
                        //                         EntityState.Added :
                        //                         EntityState.Modified;
                        //b = tempBook;

                        //context.Books.Attach(context.Books.Single(c => c.BookID == tempBook.BookID));
                        //context.Books.ApplyCurrentValues(tempBook);

                        var updateBook = context.Books.FirstOrDefault(c => c.BookID == tempBook.BookID);
                        updateBook.ISBN = tempBook.ISBN;
                        updateBook.Title = tempBook.Title;
                        updateBook.AuthorID = tempBook.AuthorID;
                        updateBook.NumPages = tempBook.NumPages;
                        updateBook.Subject = tempBook.Subject;
                        updateBook.Description = tempBook.Description;
                        updateBook.Publisher = tempBook.Publisher;
                        updateBook.YearPublished = tempBook.YearPublished;
                        updateBook.Language = tempBook.Language;
                        updateBook.NumberOfCopies = tempBook.NumberOfCopies;
                        //updateBook = tempBook;

                        //b = tempBook;

                        //data[0] = tempBook;

                        try
                        {
                            context.SaveChanges();
                            ConsoleColor foreground = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Successfully updated book in the system.");
                            Console.ForegroundColor = foreground;
                        }
                        catch (Exception e)
                        {
                            ConsoleColor foreground = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Failed to update book in the system. {e.InnerException.InnerException.Message}");
                            Console.ForegroundColor = foreground;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nOperation canceled, returning to main menu");
                        break;
                    }
                } while (answer.ToUpper() != "Y");

                Console.WriteLine("\nPress enter to continue.");
                Console.ReadLine();
            }

            Console.Clear();
        }
        private static void RemoveBook(LibraryInformationEntities context)
        {
            //get ISBN
            Console.Write("Enter the book's ISBN: ");
            string isbn = Console.ReadLine();

            var bookData = (from e in context.Books
                        where e.ISBN == isbn
                        select e).ToList();
            if (bookData.Count > 0)
            {
                Book b = bookData[0];
                BookBC tempBook = new BookBC();
                int inStockBooks = tempBook.CountInStockBooks(b);
                int copiesCheckedOut = b.NumberOfCopies - inStockBooks;

                //how many to remove
                int intNumberToRemove;
                do
                {
                    Console.WriteLine($"There are currently {copiesCheckedOut} checked out and {b.NumberOfCopies} total copies.");
                    Console.Write("Please enter the number of copies that you want to remove from the system: ");
                    string strNumberToRemove = Console.ReadLine();
                    int.TryParse(strNumberToRemove, out intNumberToRemove);

                    if(b.NumberOfCopies - intNumberToRemove >= 0)
                    {
                        if (b.NumberOfCopies - intNumberToRemove >= copiesCheckedOut)
                        {
                            if (b.NumberOfCopies - intNumberToRemove == 0)
                            {
                                //call delete
                                context.Books.Remove(b);
                                try
                                {
                                    context.SaveChanges();
                                    ConsoleColor foreground = Console.ForegroundColor;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Successfully removed the book from the system.");
                                    Console.ForegroundColor = foreground;
                                }
                                catch (Exception e)
                                {
                                    ConsoleColor foreground = Console.ForegroundColor;
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Failed to remove book from the system. {e.InnerException.InnerException.Message}");
                                    Console.ForegroundColor = foreground;
                                }
                            }
                            else
                            {
                                //call update
                                var updateBook = context.Books.FirstOrDefault(c => c.BookID == b.BookID);
                                updateBook.NumberOfCopies = b.NumberOfCopies - intNumberToRemove;

                                try
                                {
                                    context.SaveChanges();
                                    ConsoleColor foreground = Console.ForegroundColor;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Successfully removed {intNumberToRemove} books from the system.");
                                    Console.ForegroundColor = foreground;
                                }
                                catch (Exception e)
                                {
                                    ConsoleColor foreground = Console.ForegroundColor;
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Failed to update book in the system. {e.InnerException.InnerException.Message}");
                                    Console.ForegroundColor = foreground;
                                }
                            }
                        }
                        else
                        {
                            //re-enter, result can't be less than number checked out
                            ConsoleColor foreground = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The number you entered is invalid. There are too many books checked out. Please try again.");
                            Console.ForegroundColor = foreground;
                            intNumberToRemove = -1;
                        }
                    }
                    else
                    {
                        //re-enter, can't be less than 0
                        ConsoleColor foreground = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The number you entered is greater than the total number of books. Please try again.");
                        Console.ForegroundColor = foreground;
                        intNumberToRemove = -1;
                    }

                } while (intNumberToRemove == -1);
                
                Console.Write("Press enter to continue.");
                Console.ReadLine();
                Console.Clear();
                //do math
                //update or delete
            }
        }
        private static void DisplayLists(LibraryInformationEntities context)
        {
            var libarianData = (from e in context.Librarians
                                orderby e.Person.LastName, e.Person.FirstName
                                select e).ToList();

            var cardholderData = (from e in context.Cardholders
                                  orderby e.Person.LastName, e.Person.FirstName
                                  select e).ToList();

            var authorData = (from e in context.Authors
                              orderby e.Person.LastName, e.Person.FirstName
                              select e).ToList();

            var bookData = (from e in context.Books
                            orderby e.Title
                            select e).ToList();

            var checkOutData = (from e in context.CheckOutLogs
                                orderby e.CheckOutDate, e.Book.Title
                                select e).ToList();

            List<Librarian> librarians = new List<Librarian>();
            List<Cardholder> cardholders = new List<Cardholder>();
            List<CheckOutLog> logs = new List<CheckOutLog>();
            List<Author> authors = new List<Author>();
            List<Book> books = new List<Book>();
            List<CheckOutLog> overdueBooks = new List<CheckOutLog>();

            foreach (Librarian lib in libarianData)
            {
                librarians.Add(lib);
            }
            foreach (Cardholder ch in cardholderData)
            {
                cardholders.Add(ch);
            }
            foreach (CheckOutLog log in checkOutData)
            {
                logs.Add(log);
            }
            foreach (Author a in authorData)
            {
                authors.Add(a);
            }
            foreach (Book book in bookData)
            {
                books.Add(book);
            }
            foreach (CheckOutLog col in checkOutData)
            {
                DateTime time = DateTime.Now;
                //check if user has overdue book (30 days)
                if (col.CheckOutDate < time.AddDays(-30))
                {
                    overdueBooks.Add(col);
                }
            }

            LibrarianBC librarian = new LibrarianBC();
            CardholderBC cardholder = new CardholderBC();
            AuthorBC author = new AuthorBC();
            //BookBC overdue = new BookBC();
            CheckOutLogBC overdue = new CheckOutLogBC();

            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nDisplaying Librarians:");
            librarian.Display(librarians);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nDisplaying Cardholders:");
            cardholder.Display(cardholders, logs);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nDisplaying Authors:");
            author.Display(authors, books);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nDisplaying Overdue Books:");
            overdue.Display(overdueBooks);

            Console.ForegroundColor = foreground;
            Console.Write("\nPress enter to continue:");
            Console.ReadLine();
            Console.Clear();
        }
        private static Author CreateAuthor(LibraryInformationEntities context, string authorFirstName, string authorLastName)
        {
            var authorData = (from e in context.Authors
                              where e.Person.FirstName == authorFirstName
                              && e.Person.LastName == authorLastName
                              select e).ToList();

            if (authorData.Count > 0)
            {
                Author a = authorData[0];
                //book.AuthorID = a.AuthorID;
                return a;
            }
            else
            {
                var peopleData = (from e in context.People
                                  where e.FirstName == authorFirstName
                                  && e.LastName == authorLastName
                                  select e).ToList();
                Console.Write($"Author not found, please enter a Biography for {authorFirstName} {authorLastName}: ");
                string bio = Console.ReadLine();
                Author author;
                if (peopleData.Count > 0)
                {
                    Person p = peopleData[0];
                    author = new Author(p, bio);
                }
                else
                {
                    Person p = new Person(authorFirstName, authorLastName);
                    context.People.Add(p);
                    context.SaveChanges();
                    author = new Author(p, bio);
                }
                //Add the author to the database and save
                context.Authors.Add(author);
                context.SaveChanges();
                ConsoleColor foreground = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Successfully added author to the system.");
                Console.ForegroundColor = foreground;
                return author;
            }
        }
    }
}
