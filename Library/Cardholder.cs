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
    
    public partial class Cardholder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cardholder()
        {
            this.CheckOutLogs = new HashSet<CheckOutLog>();
        }
    
        public int CardHolderID { get; set; }
        public string Phone { get; set; }
        public string LibraryCardID { get; set; }
    
        public virtual Person Person { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckOutLog> CheckOutLogs { get; set; }
    }
}
