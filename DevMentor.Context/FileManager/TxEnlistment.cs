using System;
using System.Collections.Generic;
using System.Transactions;

namespace DevMentor.Context.FileManager
{
    internal class TxEnlistment : IEnlistmentNotification
    {
        private readonly List<ITransactionsOperation> _journal = new List<ITransactionsOperation>();

        public TxEnlistment(Transaction tx)
        {
            tx.EnlistVolatile(this, EnlistmentOptions.None);
        }
        
        public void EnlistOperation(ITransactionsOperation operation)
        {
            operation.Execute();

            _journal.Add(operation);
        }

        public void Commit(Enlistment enlistment)
        {
            for (int i = _journal.Count - 1; i >= 0; i--)
            {
                _journal[i].Commit();
            }
            DisposeJournal();

            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            Rollback(enlistment);
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        public void Rollback(Enlistment enlistment)
        {
            DisposeJournal();
            enlistment.Done();
        }

        private void DisposeJournal()
        {
            IDisposable disposable;
            for (int i = _journal.Count - 1; i >= 0; i--)
            {
                disposable = _journal[i] as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                _journal.RemoveAt(i);
            }
        }
    }
}