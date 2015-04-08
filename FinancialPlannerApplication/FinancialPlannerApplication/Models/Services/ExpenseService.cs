using System;
using System.Collections.Generic;
using System.Linq;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IFinancialPlannerRepository FinancialPlannerRepository;
        private readonly ISetViewModelsService SetViewModelsService;

        public ExpenseService(IFinancialPlannerRepository financialPlannerRepository, ISetViewModelsService setViewModelsService)
        {
            FinancialPlannerRepository = financialPlannerRepository;
            SetViewModelsService = setViewModelsService;
        }

        public ExpenseIndexViewModel MapExpenseIndexViewModel(string username)
        {
            var expenses = FinancialPlannerRepository.GetExpenses()
               .Where(m => m.Username == username);

            var vm = new ExpenseIndexViewModel
            {
                Expenses = SetViewModelsService.SetExpenseViewModels(expenses)
            };

            return vm;
        }

        public ExpenseTransactionsViewModel MapExpenseTransactionsViewModel(int id)
        {
            var transactions = FinancialPlannerRepository.GetTransactions()
                .Where(m => m.ExpenseId == id).ToList();

            var vm = new ExpenseTransactionsViewModel
            {
                Transactions = SetViewModelsService.SetTransactionViewModels(transactions)
            };
            var withdrawalAmount = transactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var depositAmount = transactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            vm.Total = depositAmount - withdrawalAmount;

            return vm;
        }

        public EditExpenseViewModel MapEditExpenseViewModel(int id)
        {
            var expense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == id);

            if (expense == null)
                return null;

            var vm = new EditExpenseViewModel
            {
                ExpenseId = expense.Id,
                Name = expense.Name,
                Amount = expense.Amount,
                InterestRate = expense.InterestRate
            };

            return vm;
        }

        public void AddExpense(EditExpenseViewModel vm, string username)
        {
            var expense = new Expense
            {
                Name = vm.Name,
                Amount = vm.Amount,
                Balance = vm.Amount,
                InterestRate = vm.InterestRate,
                Username = username
            };

            FinancialPlannerRepository.AddExpense(expense);
            FinancialPlannerRepository.Save();
        }

        public bool EditExpense(EditExpenseViewModel vm)
        {
            var expense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == vm.ExpenseId);

            if (expense == null)
                return false;

            var transactions = FinancialPlannerRepository.GetTransactions().Where(m => m.ExpenseId == expense.Id).ToList();

            var withdrawalAmount = transactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var depositAmount = transactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            expense.Name = vm.Name;
            expense.Amount = vm.Amount;
            expense.InterestRate = vm.InterestRate;
            expense.Balance = vm.Amount + (depositAmount - withdrawalAmount);

            FinancialPlannerRepository.EditExpense(expense);
            FinancialPlannerRepository.Save();

            return true;
        }

        public EditExpenseTransactionViewModel MapEditTransactionViewModelForEdit(int id, int expenseId, string username)
        {
            var transaction = FinancialPlannerRepository.GetTransactions()
              .FirstOrDefault(m => m.Id == id);

            if (transaction == null)
                return null;

            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);
            var accounts = FinancialPlannerRepository.GetAccounts()
                .Where(m => m.UserName == username);
            var budgets = FinancialPlannerRepository.GetBudgets()
                .Where(m => m.Username == username).ToList();

            var vm = new EditExpenseTransactionViewModel
            {
                Accounts = SetViewModelsService.SetAccountViewModels(accounts),
                ExpenseTransactionId = transaction.Id,
                Amount = transaction.Amount,
                IsWithdrawal = transaction.IsWithdrawal,
                Name = transaction.Name,
                PaymentDate = transaction.PaymentDate,
                SelectedExpenseId = expenseId != 0 ? expenseId : 0,
                OldExpenseId = expenseId != 0 ? expenseId : 0,
                WasWithdrawal = transaction.IsWithdrawal,
                OldAmount = transaction.Amount,
                Budgets = SetViewModelsService.SetBudgetViewModels(budgets),
                Expenses = SetViewModelsService.SetExpenseViewModels(expenses),
                SelectedAccountId = transaction.AccountId,
                SelectedBudgetItemId = transaction.BudgetItemId
            };

            if (budgets.Any())
            {
                var budgetIds = budgets.Select(m => m.Id);
                var budgetItems = FinancialPlannerRepository.GetBudgetItems().Where(m => budgetIds.Contains(m.BudgetId));

                vm.BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems);
            }

            var budgetItemForExpense = FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == transaction.BudgetItemId);

            if (budgetItemForExpense != null)
                vm.SelectedBudgetId = budgetItemForExpense.BudgetId;

            return vm;
        }

        public EditExpenseTransactionViewModel MapEditTransactionViewModelForAdd(int id, string username)
        {
            var accounts = FinancialPlannerRepository.GetAccounts()
               .Where(m => m.UserName == username);
            var budgets = FinancialPlannerRepository.GetBudgets()
                .Where(m => m.Username == username).ToList();
            budgets = budgets.Where(m => m.BudgetItems.Any()).ToList();
            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);

            var vm = new EditExpenseTransactionViewModel
            {
                Accounts = SetViewModelsService.SetAccountViewModels(accounts),
                Budgets = budgets.Any() ? SetViewModelsService.SetBudgetViewModels(budgets) : null,
                SelectedAccountId = id != 0 ? id : 0,
                PaymentDate = DateTime.Now,
                NewTransaction = true,
                Expenses = SetViewModelsService.SetExpenseViewModels(expenses)
            };

            return vm;
        }

        public void MapEditTransactionViewModel(EditExpenseTransactionViewModel vm, string username)
        {
            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);
            var accounts = FinancialPlannerRepository.GetAccounts().Where(m => m.UserName == username);
            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == username);
            var budgetItems =
                FinancialPlannerRepository.GetBudgetItems().Where(m => m.BudgetId == vm.SelectedBudgetId);
            vm.BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems);
            vm.Accounts = SetViewModelsService.SetAccountViewModels(accounts);
            vm.Budgets = SetViewModelsService.SetBudgetViewModels(budgets);
            vm.Expenses = SetViewModelsService.SetExpenseViewModels(expenses);
        }

        public void AdjustAccountBalance(Account account, EditExpenseTransactionViewModel vm)
        {
            if (vm.IsWithdrawal)
                account.Amount = account.Amount - vm.Amount;
            else
                account.Amount = account.Amount + vm.Amount;

            FinancialPlannerRepository.EditAccount(account);
            FinancialPlannerRepository.Save();
        }

        public Transaction AddTransaction(Expense expense, EditExpenseTransactionViewModel vm)
        {
            var newTransaction = new Transaction
            {
                Name = vm.Name,
                Amount = vm.Amount,
                IsWithdrawal = vm.IsWithdrawal,
                ExpenseId = vm.SelectedExpenseId != -1 ? vm.SelectedExpenseId : 0,
                PaymentDate = vm.PaymentDate,
                Balance = expense.Amount,
                BudgetItemId = vm.SelectedBudgetItemId
            };

            FinancialPlannerRepository.AddTransaction(newTransaction);
            FinancialPlannerRepository.Save();

            return newTransaction;
        }

        public void AdjustTransactionBalances(Account account, EditExpenseTransactionViewModel vm)
        {
            var transactions = FinancialPlannerRepository.GetTransactions().Where(m => m.AccountId == vm.SelectedAccountId).ToList();

            var accountTransactions =
                transactions
                    .OrderBy(m => m.PaymentDate);

            var firstTransaction = accountTransactions.FirstOrDefault();

            if (firstTransaction != null)
            {
                var previousTransaction = new Transaction();

                foreach (var tran in accountTransactions)
                {
                    if (tran.PaymentDate == firstTransaction.PaymentDate)
                    {
                        tran.Balance = tran.IsWithdrawal
                            ? account.InitialAmount - tran.Amount
                            : account.InitialAmount + tran.Amount;
                    }
                    else
                    {
                        tran.Balance = tran.IsWithdrawal
                            ? previousTransaction.Balance - tran.Amount
                            : previousTransaction.Balance + tran.Amount;
                    }

                    //FinancialPlannerRepository.EditTransaction(tran);
                    FinancialPlannerRepository.Save();

                    previousTransaction = tran;
                }
            }
        }

        public void AdjustBudgetItemBalance(BudgetItem budgetItem, EditExpenseTransactionViewModel vm)
        {
            budgetItem.Balance = vm.IsWithdrawal
                ? budgetItem.Balance - vm.Amount
                : budgetItem.Balance + vm.Amount;

            if (budgetItem.Balance > budgetItem.Amount)
                budgetItem.Amount = budgetItem.Balance;

            FinancialPlannerRepository.EditBudgetItem(budgetItem);
            FinancialPlannerRepository.Save();
        }

        public Transaction EditTransaction(EditExpenseTransactionViewModel vm)
        {
            var transaction = FinancialPlannerRepository.GetTransactions().FirstOrDefault(m => m.Id == vm.ExpenseTransactionId);

            if (transaction != null)
            {
                transaction.Amount = vm.Amount;
                transaction.Name = vm.Name;
                transaction.PaymentDate = vm.PaymentDate;
                transaction.IsWithdrawal = vm.IsWithdrawal;
                transaction.BudgetItemId = vm.SelectedBudgetItemId == -1 ? null : vm.SelectedBudgetItemId;
                transaction.AccountId = vm.SelectedAccountId == -1 ? null : vm.SelectedAccountId;
                transaction.ExpenseId = vm.SelectedExpenseId;

                FinancialPlannerRepository.EditTransaction(transaction);
                FinancialPlannerRepository.Save();
            }

            return transaction;
        }

        public void AdjustOldBudgetItemBalance(int? oldBudgetItemId)
        {
            var oldBudgetItem = FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == oldBudgetItemId);

            if (oldBudgetItem != null)
            {
                EditBudgetItemBalance(oldBudgetItem);
            }
        }

        public void AdjustNewBudgetItemBalance(EditExpenseTransactionViewModel vm)
        {
            var budgetItem = FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == vm.SelectedBudgetItemId);

            if (budgetItem != null)
            {
                EditBudgetItemBalance(budgetItem);
            }
        }

        private void EditBudgetItemBalance(BudgetItem budgetItem)
        {
            var budgetItemTransactions =
                FinancialPlannerRepository.GetTransactions().Where(m => m.BudgetItemId == budgetItem.Id).ToList();

            var budgetItemWithdrawalSum = budgetItemTransactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var budgetItemDepositSum = budgetItemTransactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            budgetItem.Balance = budgetItem.Amount + budgetItemDepositSum - budgetItemWithdrawalSum;

            FinancialPlannerRepository.EditBudgetItem(budgetItem);
            FinancialPlannerRepository.Save();
        }

        public void AdjustTransactionBalances(Account account)
        {
            var transactions =
              FinancialPlannerRepository.GetTransactions().Where(m => m.AccountId == account.Id).ToList();

            var accountTransactions =
                transactions
                    .OrderBy(m => m.PaymentDate);

            var firstTransaction = accountTransactions.FirstOrDefault();

            if (firstTransaction != null)
            {
                decimal previousTransactionAmount = 0;

                foreach (var tran in accountTransactions)
                {
                    if (tran.Id == firstTransaction.Id)
                    {
                        tran.Balance = tran.IsWithdrawal
                            ? account.InitialAmount - tran.Amount
                            : account.InitialAmount + tran.Amount;
                    }
                    else
                    {
                        tran.Balance = tran.IsWithdrawal
                            ? previousTransactionAmount - tran.Amount
                            : previousTransactionAmount + tran.Amount;
                    }

                    previousTransactionAmount = tran.Balance;

                    FinancialPlannerRepository.EditTransaction(tran);
                    FinancialPlannerRepository.Save();
                }

                FinancialPlannerRepository.Save();
            }
        }

        public void AdjustAccountAmount(Account account)
        {
            var transactions =
              FinancialPlannerRepository.GetTransactions().Where(m => m.AccountId == account.Id).ToList();

            transactions = transactions.ToList();
            var withdrawalAmount = transactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var depositAmount = transactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            account.Amount = account.InitialAmount + depositAmount - withdrawalAmount;

            FinancialPlannerRepository.EditAccount(account);
            FinancialPlannerRepository.Save();
        }

        public void AdjustExpenseBalance(Expense expense)
        {
            var transactions =
               FinancialPlannerRepository.GetTransactions().Where(m => m.ExpenseId == expense.Id).ToList();

            var newWithdrawalAmount = transactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var newDepositAmount = transactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            expense.Balance = expense.Amount - newWithdrawalAmount + newDepositAmount;

            FinancialPlannerRepository.EditExpense(expense);
            FinancialPlannerRepository.Save();
        }

        public void AdjustExpenseBalance(Expense expense, Transaction transaction)
        {
            expense.Balance = transaction.IsWithdrawal
                        ? expense.Balance - transaction.Amount
                        : expense.Balance + transaction.Amount;

            FinancialPlannerRepository.EditExpense(expense);
            FinancialPlannerRepository.Save();
        }
    }
}