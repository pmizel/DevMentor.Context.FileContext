using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;

namespace DevMentor.Context.FileManager
{
    public class TxFileManager : IFileManager
    {
        public TxFileManager()
        {
            FileUtils.EnsureTempFolderExists();
        }

        public void WriteAllText(string path, string contents)
        {
            if (IsInTransaction())
            {
                EnlistOperation(new WriteAllTextOperation(path, contents));
            }
            else
            {
                File.WriteAllText(path, contents);
            }
        }

        public string GetTempFileName(string extension)
        {
            string retVal = FileUtils.GetTempFileName(extension);
            
            return retVal;
        }

        public string GetTempFileName()
        {
            return GetTempFileName(".tmp");
        }

        [ThreadStatic]
        private static Dictionary<string, TxEnlistment> _enlistments;

        private static readonly object _enlistmentsLock = new object();

        private static bool IsInTransaction()
        {
            return Transaction.Current != null;
        }

        private static void EnlistOperation(ITransactionsOperation operation)
        {
            Transaction tx = Transaction.Current;
            TxEnlistment enlistment;

            lock (_enlistmentsLock)
            {
                if (_enlistments == null)
                {
                    _enlistments = new Dictionary<string, TxEnlistment>();
                }

                if (!_enlistments.TryGetValue(tx.TransactionInformation.LocalIdentifier, out enlistment))
                {
                    enlistment = new TxEnlistment(tx);
                    _enlistments.Add(tx.TransactionInformation.LocalIdentifier, enlistment);
                }
                enlistment.EnlistOperation(operation);
            }
        }
    }
}
