using System;
using System.IO;

namespace DevMentor.Context.FileManager
{
    abstract class SingleFileOperation : ITransactionsOperation, IDisposable
    {
        protected readonly string path;
        protected string TempPath;
        private bool disposed;

        public SingleFileOperation(string path)
        {
            this.path = path;
        }

        public abstract void Execute();

        public void Commit()
        {
            if (TempPath != null)
            {
                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.Copy(TempPath, path, true);
            }
        }
        
        public void Dispose()
        {
            InnerDispose();
            GC.SuppressFinalize(this);
        }

        private void InnerDispose()
        {
            if (!disposed)
            {
                if (TempPath != null)
                {
                    FileInfo fi = new FileInfo(TempPath);
                    if (fi.IsReadOnly)
                    {
                        fi.Attributes = FileAttributes.Normal;
                    }
                    File.Delete(TempPath);
                }

                disposed = true;
            }
        }
    }
}