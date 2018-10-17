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
            ////connect to database context
            //var context = new LibraryInformationEntities();
            //var data = from e in context.Authors
            //           select e;
            //data.ToList();

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
                        Console.WriteLine("Implement option 1");
                        FindMenu();
                            break; //case 1 break
                    case 2:
                        //Librarian login feature
                        bool login = Login();

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
                                        Console.WriteLine("Implement option 1");
                                        FindMenu();
                                        break; //case 1 break
                                    case 2:
                                        Console.WriteLine("Implement option 2");
                                        break; //case 2 break
                                    case 3:
                                        Console.WriteLine("Implement option 3");
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
                                        //logoff
                                        break; //case 8 break
                                    default:
                                        //Display error
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid option, please try again.");
                                        break; //default break
                                }
                            } while (librarianChoice != 8);
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

            //Change the console color to white to indicate a paetron user
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
            Console.WriteLine("8. Logoff.");
            Console.Write("Please Choose An Option (1-8): ");

            //Get user input
            userInput = Console.ReadLine();

            //verify the user input is an int
            int.TryParse(userInput, out int selection);
            return selection;
        }
        private static void FindMenu()
        {
            //get the search term from the user
            Console.Write("Enter a search term: ");
            string search = Console.ReadLine();
            Console.WriteLine("Implement the find feature");
            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
            Console.Clear();
        }

        //maybe want to create login class so I can return object with username and loginStatus?
        private static bool Login()
        {
            //assume login fails
            bool successfulLogin = false;

            //Get username and password from librarian
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            Console.Clear();

            //Check sql for login
            //if (username == && password ==)
            //{
                successfulLogin = true;
            //}

            return successfulLogin;
        }
    }
}
