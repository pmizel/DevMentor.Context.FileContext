namespace DevMentor.Context.FileManager
{
    public interface IFileManager
    {                
        string GetTempFileName(string extension);

        string GetTempFileName();

        void WriteAllText(string path, string contents);
    }
}