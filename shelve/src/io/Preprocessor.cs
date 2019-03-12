namespace Shelve.IO
{
    using System.IO;
    using System.Text;
    using System.Collections.Generic;

    public class Preprocessor
    {
        private string rootPath;

        public Preprocessor(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Directory on path {path} is not exist.");
            }

            rootPath = path;
        }

        public string MergeAllFiles()
        {
            var paths = FindJsonsPaths();

            if (paths.Count == 0)
            {
                throw new IOException("Passed path do not contains any set files (subdirs included).");
            }

            var sb = new StringBuilder().Append("[");

            foreach (var path in paths)
            {
                var fileText = File.ReadAllText(path).Trim(new char[] { '[', ']' });
                sb.Append(fileText).Append(",");
            }

            return sb.Remove(sb.Length - 1, 1).Append("]").ToString();
        }

        private List<string> FindJsonsPaths()
        {
            var dirs = new Stack<string>(new string[] { rootPath });
            var paths = new List<string>();

            while (dirs.Count > 0)
            {
                var curent = dirs.Pop();
                var subs = Directory.GetDirectories(curent);
                var files = Directory.GetFiles(curent);

                foreach (string file in files)
                {
                    if (file.EndsWith(".json"))
                    {
                        paths.Add(file);
                    }
                }

                foreach (string sub in subs)
                {
                    dirs.Push(sub);
                }
            }

            return paths;
        }
    }
}
