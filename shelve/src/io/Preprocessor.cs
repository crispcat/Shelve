namespace Shelve.IO
{
    using System.IO;
    using System.Text;
    using System.Collections.Generic;

    public class Preprocessor
    {
        private string rootPath;

        private List<string> filePaths;

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
                throw new IOException("Passed path include subdirs do not contains any .json files.");
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

            sb.Append("[");

            foreach (var filePath in filePaths)
            {
                if (IsValid(filePath))
                {
                    var fileText = File.ReadAllText(filePath).Trim(new char[] { '[', ']' });

                    sb.Append(fileText).Append(",");
                }
            }

            return sb.Remove(sb.Length - 1, 1).Append("]").ToString();
        }

        private bool IsValid(string filePath)
        {
            
        }
    }
}
