using System;
using System.IO;

namespace DevMentor.Context.FileManager
{
    static class FileUtils
    {
        private static readonly string tempFolder = Path.Combine(Path.GetTempPath(), "CdFileMgr");


        public static void EnsureTempFolderExists()
        {
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }
        }

        
        public static string GetTempFileName(string extension)
        {
            Guid g = Guid.NewGuid();
            string retVal = Path.Combine(tempFolder, g.ToString().Substring(0, 16)) + extension;

            return retVal;
        }
    }
}