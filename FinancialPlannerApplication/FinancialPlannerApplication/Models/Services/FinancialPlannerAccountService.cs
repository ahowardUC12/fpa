using System;
using System.Linq;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public class FinancialPlannerAccountService : IFinancialPlannerAccountService
    {
        private readonly IFinancialPlannerRepository FinancialPlannerRepository;
        private readonly ISetViewModelsService SetViewModelsService;

        public FinancialPlannerAccountService(IFinancialPlannerRepository financialPlannerRepository, ISetViewModelsService setViewModelsService)
        {
            FinancialPlannerRepository = financialPlannerRepository;
            SetViewModelsService = setViewModelsService;
        }

        public AccountIndexViewModel MapAccountIndexViewModel(string username)
        {
            var accounts = FinancialPlannerRepository.GetAccounts()
              .Where(m => m.UserName == username);

            var vm = new AccountIndexViewModel
            {
                Accounts = SetViewModelsService.SetAccountViewModels(accounts)
            };

            return vm;
        }

        public EditAccountViewModel MapEditAccountViewModel(int id)
        {
            var account = FinancialPlannerRepository.GetAccounts()
                    .FirstOrDefault(m => m.Id == id);

            if (account == null)
                return null;

            var vm = new EditAccountViewModel
            {
                Id = account.Id,
                Name = account.Name,
                Amount = account.Amount,
                UserName = account.UserName
            };

            return vm;
        }

        public Account AddAccount(string username, EditAccountViewModel vm)
        {
            var account = new Account
            {
                Name = vm.Name,
                Amount = vm.Amount,
                UserName = username,
                InitialAmount = vm.Amount
            };

            FinancialPlannerRepository.AddAccount(account);
            FinancialPlannerRepository.Save();

            return account;
        }

        public EditTransactionViewModel MapEditTransactionViewModel(string username)
        {
            var accounts = FinancialPlannerRepository.GetAccounts()
              .Where(m => m.UserName == username);

            var vm = new EditTransactionViewModel
            {
                Accounts = SetViewModelsService.SetAccountViewModels(accounts)
            };

            return vm;
        }

        public EditTransactionViewModel MapEditTransactionViewModelForEdit(int id, int accountId, string username)
        {
            var transaction = FinancialPlannerRepository.GetTransactions()
              .FirstOrDefault(m => m.Id == id);

            if (transaction == null)
                return null;

            var accounts = FinancialPlannerRepository.GetAccounts()
                .Where(m => m.UserName == username);

            var budgets = FinancialPlannerRepository.GetBudgets()
                .Where(m => m.Username == username).ToList();

            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);

            var vm = new EditTransactionViewModel
            {
                Accounts = SetViewModelsService.SetAccountViewModels(accounts),
                AccountTransactionId = transaction.Id,
                Amount = transaction.Amount,
                IsWithdrawal = transaction.IsWithdrawal,
                Name = transaction.Name,
                PaymentDate = transaction.PaymentDate,
                SelectedAccountId = accountId != 0 ? accountId : 0,
                OldAccountId = accountId != 0 ? accountId : 0,
                WasWithdrawal = transaction.IsWithdrawal,
                OldAmount = transaction.Amount,
                Budgets = SetViewModelsService.SetBudgetViewModels(budgets),
                Expenses = SetViewModelsService.SetExpenseViewModels(expenses)
            };

            if(transaction.BudgetItemId != null && transaction.BudgetItemId > 0)
                SetBudgetItemsForViewModel(transaction, vm);

            SetSelectedExpenseForViewModel(transaction, vm);

            return vm;
        }

        private void SetSelectedExpenseForViewModel(Transaction transaction, EditTransactionViewModel vm)
        {
            var selectedExpense =
                FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == transaction.ExpenseId);

            if (selectedExpense != null)
            {
                vm.SelectedExpenseId = selectedExpense.Id;
            }
        }

        private void SetBudgetItemsForViewModel(Transaction transaction, EditTransactionViewModel vm)
        {
            var selectedBudgetItem =
                FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == transaction.BudgetItemId);

            if (selectedBudgetItem != null)
            {
                var budgetItems =
                    FinancialPlannerRepository.GetBudgetItems()
                        .Where(m => m.BudgetId == selectedBudgetItem.BudgetId)
                        .ToList();

                vm.BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems);
                vm.SelectedBudgetId = selectedBudgetItem.BudgetId;
                vm.SelectedBudgetItemId = transaction.BudgetItemId;
            }
        }

        public bool EditAccount(EditAccountViewModel vm, string username)
        {
            var account = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.Id);

            if (account == null)
                return false;

            account.InitialAmount = account.InitialAmount + (vm.Amount - account.Amount);
            account.Name = vm.Name;
            account.Amount = vm.Amount;
            account.UserName = username;
            
            FinancialPlannerRepository.EditAccount(account);
            FinancialPlannerRepository.Save();

            AdjustTransactionBalances(account);

            return true;
        }

        public AccountTransactionsViewModel MapAccountTransactionsViewModel(int id)
        {
            var transactions = FinancialPlannerRepository.GetTransactions()
               .Where(m => m.AccountId == id).ToList();

            var withdrawalAmount = transactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var depositAmount = transactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            var transactionTotal = depositAmount - withdrawalAmount;

            var vm = new AccountTransactionsViewModel
            {
                Transactions = SetViewModelsService.SetTransactionViewModels(transactions),
                TransactionTotal = transactionTotal
            };

            return vm;
        }

        public EditTransactionViewModel MapEditTransactionViewModelForAdd(int id, string username)
        {
            var accounts = FinancialPlannerRepository.GetAccounts()
               .Where(m => m.UserName == username);
            var budgets = FinancialPlannerRepository.GetBudgets()
                .Where(m => m.Username == username).ToList();
            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);
            budgets = budgets.Where(m => m.BudgetItems.Any()).ToList();

            var vm = new EditTransactionViewModel
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

        public void AdjustAccountBalance(Account account, EditTransactionViewModel vm)
        {
            if (vm.IsWithdrawal)
                account.Amount = account.Amount - vm.Amount;
            else
                account.Amount = account.Amount + vm.Amount;

            FinancialPlannerRepository.EditAccount(account);
            FinancialPlannerRepository.Save();
        }

        public Transaction AddTransaction(Account account, EditTransactionViewModel vm)
        {
            var newTransaction = new Transaction
            {
                Name = vm.Name,
                Amount = vm.Amount,
                IsWithdrawal = vm.IsWithdrawal,
                AccountId = vm.SelectedAccountId != -1 ? vm.SelectedAccountId : 0,
                PaymentDate = vm.PaymentDate,
                Balance = account.Amount,
                BudgetItemId = vm.SelectedBudgetItemId,
                ExpenseId = vm.SelectedExpenseId != -1 ? vm.SelectedExpenseId : 0
            };

            FinancialPlannerRepository.AddTransaction(newTransaction);
            FinancialPlannerRepository.Save();

            return newTransaction;
        }

        public void AdjustBudgetItemBalance(BudgetItem budgetItem, EditTransactionViewModel vm)
        {
            budgetItem.Balance = vm.IsWithdrawal
                ? budgetItem.Balance - vm.Amount
                : budgetItem.Balance + vm.Amount;

            if (budgetItem.Balance > budgetItem.Amount)
                budgetItem.Amount = budgetItem.Balance;

            FinancialPlannerRepository.EditBudgetItem(budgetItem);
            FinancialPlannerRepository.Save();
        }

        public void AdjustTransactionBalances(Account account, EditTransactionViewModel vm)
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

        public void MapEditTransactionViewModel(EditTransactionViewModel vm, string username)
        {
            var accounts = FinancialPlannerRepository.GetAccounts().Where(m => m.UserName == username);
            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == username);
            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);
            var budgetItems =
                FinancialPlannerRepository.GetBudgetItems().Where(m => m.BudgetId == vm.SelectedBudgetId);
            vm.BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems);
            vm.Accounts = SetViewModelsService.SetAccountViewModels(accounts);
            vm.Budgets = SetViewModelsService.SetBudgetViewModels(budgets);
            vm.Expenses = SetViewModelsService.SetExpenseViewModels(expenses);
        }

        public void EditTransaction(EditTransactionViewModel vm)
        {
            var transaction = FinancialPlannerRepository.GetTransactions().FirstOrDefault(m => m.Id == vm.AccountTransactionId);

            if (transaction != null)
            {
                transaction.Amount = vm.Amount;
                transaction.Name = vm.Name;
                transaction.PaymentDate = vm.PaymentDate;
                transaction.IsWithdrawal = vm.IsWithdrawal;
                transaction.BudgetItemId = vm.SelectedBudgetItemId;
                transaction.AccountId = vm.SelectedAccountId;
                transaction.ExpenseId = vm.SelectedExpenseId;

                FinancialPlannerRepository.EditTransaction(transaction);
                FinancialPlannerRepository.Save();
            }
        }

        public void AdjustOldBudgetItemBalance(int? oldBudgetItemId)
        {
            var oldBudgetItem = FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == oldBudgetItemId);

            if (oldBudgetItem != null)
            {
                EditBudgetItem(oldBudgetItem);
            }
        }

        public void AdjustNewBudgetItemBalance(EditTransactionViewModel vm)
        {
            var budgetItem = FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == vm.SelectedBudgetItemId);

            if (budgetItem != null)
            {
                EditBudgetItem(budgetItem);
            }
        }

        private void EditBudgetItem(BudgetItem budgetItem)
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

        public void AdjustExpenseBalance(Expense expense, Transaction transaction)
        {
            expense.Balance = transaction.IsWithdrawal
                        ? expense.Balance - transaction.Amount
                        : expense.Balance + transaction.Amount;

            FinancialPlannerRepository.EditExpense(expense);
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
    }
}