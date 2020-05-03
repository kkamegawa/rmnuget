using Semver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RemoveNuget
{
    public static class rmnupkg
    {
        static void Main()
        {
            List<string> targetFolderList = new List<string>();

#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @".nuget\packages");
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
            var files = new System.IO.DirectoryInfo(folder);
            var direcories = files.GetDirectories();
            foreach(var dir in direcories)
            {
                targetFolderList.AddRange(CollectNupkgFolder(dir.FullName, 1));
            }
            RemoveNupkgfromDisk(targetFolderList);
        }

        static void RemoveNupkgfromDisk(IEnumerable<string> targetFolders)
        {
            Parallel.ForEach(targetFolders, x => { Directory.Delete(x, true); });
        }

        static public IEnumerable<string> CollectNupkgFolder(string folderName, int leaveFiles)
        {
            List<string> targetFolderList = new List<string>();

            var direcories = new DirectoryInfo(folderName).GetDirectories();

            if(direcories.Length > leaveFiles)
            {
                SemVersion versionInfo;

                if (direcories.All(x => SemVersion.TryParse(x.Name, out versionInfo)) == true)
                {
                    var removes = direcories.OrderBy(x => x.Name).SkipLast(leaveFiles).Select(x => x.FullName);
                    targetFolderList.AddRange(removes);
                }
            }

            return targetFolderList;
        }
    }
}