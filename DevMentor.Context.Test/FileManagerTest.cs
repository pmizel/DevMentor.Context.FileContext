using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Transactions;
using DevMentor.Context.FileManager;

namespace DevMentor.Context.Test
{
    [TestClass]
    public class FileManagerTest
    {
        private int _numTempFiles;
        private IFileManager _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new TxFileManager();
            _numTempFiles = Directory.GetFiles(Path.Combine(Path.GetTempPath(), "CdFileMgr")).Length;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            int numTempFiles = Directory.GetFiles(Path.Combine(Path.GetTempPath(), "CdFileMgr")).Length;
            Assert.AreEqual(_numTempFiles, numTempFiles, "Unexpected value for numTempFiles.");
        }

        [TestMethod]
        public void CanWriteAllText()
        {
            string f1 = _target.GetTempFileName();
            try
            {
                const string contents = "abcdef";
                File.WriteAllText(f1, "123");

                using (TransactionScope scope1 = new TransactionScope())
                {
                    _target.WriteAllText(f1, contents);
                    scope1.Complete();
                }

                Assert.AreEqual(contents, File.ReadAllText(f1), "Unexpected value from ReadAllText.");
            }
            finally
            {
                File.Delete(f1);
            }
        }

        [TestMethod]
        public void CanWriteAllTextAndRollback()
        {
            string f1 = _target.GetTempFileName();
            try
            {
                const string contents1 = "123";
                const string contents2 = "abcdef";
                File.WriteAllText(f1, contents1);

                using (TransactionScope scope1 = new TransactionScope())
                {
                    _target.WriteAllText(f1, contents2);
                }

                Assert.AreEqual(contents1, File.ReadAllText(f1), "Unexpected value from ReadAllText.");
            }
            finally
            {
                File.Delete(f1);
            }
        }
    }
}
