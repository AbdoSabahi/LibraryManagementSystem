using System;

namespace LibraryManagementSystem.Models
{
    public class PremiumMember : Member
    {
        public int MaxBorrowLimit { get; set; }
        public int LoanDays { get; set; }

        public PremiumMember(int id, string name, string email)
            : base(id, name, email)
        {
            MaxBorrowLimit = 10;
            LoanDays = 30;
        }

        public override string GetInfo()
        {
            return $"[{Id}] {Name} (Premium) - {Email} - Joined: {JoinDate:yyyy-MM-dd} " +
                   $"- Limit: {MaxBorrowLimit} books - Loan period: {LoanDays} days";
        }
    }
}