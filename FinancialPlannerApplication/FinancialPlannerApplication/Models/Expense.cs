﻿namespace FinancialPlannerApplication.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal Balance { get; set; }
        public string Username { get; set; }
    }
}