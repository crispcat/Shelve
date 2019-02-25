namespace Shelve.Core
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;

    public class Preprocessor
    {
        private string rootPath;

        private List<string> filePaths;

        private const int chunkSize = 2 * 1024; //2KB

        public Preprocessor(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Directory on path {path} is not exist.");
            }

            rootPath = path;
            filePaths = new List<string>();

            FindFiles();

            if (filePaths.Count == 0)
            {
                throw new IOException("Passed path root do not contains any .json files (subdirs included).");
            }
        }

        private void FindFiles()
        {
            Stack<string> dirs = new Stack<string>();
            dirs.Push(rootPath);

            while (dirs.Count > 0)
            {
                string curent = dirs.Pop();

                string[] files;
                string[] subs;

                try
                {
                    subs = Directory.GetDirectories(curent);
                    files = Directory.GetFiles(curent);
                }
                catch
                {
                    continue;
                }

                foreach (string file in files)
                {
                    if (file.EndsWith(".json"))
                    {
                        filePaths.Add(file);
                    }
                }

                foreach (string sub in subs)
                {
                    dirs.Push(sub);
                }
            }
        }

        public string Combine()
        {
            var sb = new StringBuilder();
            var buffer = new byte[chunkSize];

            foreach (var filePath in filePaths)
            {
                using (var file = File.OpenRead(filePath))
                {
                    int bytesRead;

                    while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        sb.Append(BitConverter.ToString(buffer));
                    }
                }
            }

            return sb.ToString();
        }
    }
}
