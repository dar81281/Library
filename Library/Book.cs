//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library
{
    using System;
    using System.Collections.Generic;
    
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.CheckOutLogs = new HashSet<CheckOutLog>();
        }
    
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public Nullable<int> NumPages { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public string Language { get; set; }
        public int NumberOfCopies { get; set; }
    
        public virtual Author Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckOutLog> CheckOutLogs { get; set; }

        //public static implicit operator Book(BookBC v)
        //{
        //    Book book = new Book();
        //    book = v;
        //    return book;
        //}
    }
}
