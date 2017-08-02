//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CIMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Instruction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Instruction()
        {
            this.Actions = new HashSet<Action>();
        }
    
        public int InstructionID { get; set; }
        public int InstructionTypeID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public int StatusID { get; set; }
        public int FromUser { get; set; }
        public int ToUser { get; set; }
        public double Amount { get; set; }
        public int ClientID { get; set; }
        public int CurrencyFrom { get; set; }
        public int CurrencyTo { get; set; }
        public int BranchID { get; set; }
        public string EERef { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Action> Actions { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual Client Client { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Currency Currency1 { get; set; }
        public virtual InstructionType InstructionType { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
    }
}