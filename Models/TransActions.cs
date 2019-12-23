using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccount.Models
{
    public class TransActions
    {
        [Key]
        public int TransActionId {get; set;}
        public double Amount {get; set;}
        public int UserId {get; set;}
        public User AccountHolder {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

    }
}