using System.IO;

namespace DevMentor.Context.FileManager
{
    sealed class WriteAllTextOperation : SingleFileOperation
    {
        private readonly string contents;
        
        public WriteAllTextOperation(string path, string contents)
            : base(path)
        {
            this.contents = contents;
        }

        public override void Execute()
        {
            if (File.Exists(path))
            {
                string temp = FileUtils.GetTempFileName(Path.GetExtension(path));
                File.Copy(path, temp);
                TempPath = temp;
                File.WriteAllText(TempPath, contents);
            }
            else
            {
                File.WriteAllText(path, contents);
                string temp = FileUtils.GetTempFileName(Path.GetExtension(path));
                File.Copy(path, temp);
                TempPath = temp;
                File.WriteAllText(TempPath, contents);

            }

        }
    }
}