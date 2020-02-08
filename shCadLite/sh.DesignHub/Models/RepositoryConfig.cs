using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sh.DesignHub.Models
{
    public class RepositoryConfig
    {
        public static FileInfo RepoConfigFile
        {
            get
            {
                var assfile = new FileInfo(Assembly.GetExecutingAssembly().Location);
                var result = new FileInfo($@"{assfile.DirectoryName}\repositories.json");
                if (!result.Exists)
                {
                    File.WriteAllText(result.FullName, "[]");
                }
                return result;
            }
        }
        public static List<RepositoryConfig> GetRepositories()
        {
            var repofile = RepoConfigFile;
            if (!repofile.Exists) throw new FileNotFoundException("未找到仓库设置文件", repofile.FullName);
            return JsonConvert.DeserializeObject<List<RepositoryConfig>>(File.ReadAllText(repofile.FullName));
        }

        public string Name { get; set; }

        public string url { get; set; }

        public string Local { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string email { get; set; }

        public string authorname { get; set; }


        public Patch GetChangeAsync()
        {
            using (var repo = new Repository(Local))
            {
                Tree commitTree = repo.Head.Tip.Tree; // Main Tree
                //Tree parentCommitTree = repo.Head.Tip.Parents.Single().Tree; // Secondary Tree
                var patch = repo.Diff.Compare<Patch>(commitTree, DiffTargets.WorkingDirectory); // Difference
                //return repo.Diff.Compare<TreeChanges>();
                return patch;
            }
        }





        public async Task CreateAsync()
        {
            await Task.Run(() =>
            {
                Repository.Init(Local);
                var content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                using (var repo = new Repository(Local))
                {
                    // Stage the file
                    Commands.Stage(repo, "*");
                    // Create the committer's signature and commit
                    var author = new Signature(authorname, email, DateTime.Now);
                    var committer = author;
                    // Commit to the repository
                    var commit = repo.Commit(content, author, committer);
                    // git push
                    var options = new PushOptions
                    {
                        CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                        {
                            return new UsernamePasswordCredentials()
                            {
                                Username = username,
                                Password = password
                            };
                        }),
                        OnPushTransferProgress = (c, t, b) => ProcessHandler.OnProgress($"current:{c},total:{t},bytes:{b}")
                    };
                    repo.Network.Push(repo.Branches["BranchesTest"], options);
                }
            });
        }

        public async Task CloneAsync()
        {
            await Task.Run(() =>
            {
                Repository.Clone(url, Local, new CloneOptions { OnTransferProgress = p => ProcessHandler.OnProgress($"{p.ReceivedObjects}/{p.TotalObjects}({p.ReceivedBytes}bytes已接收)") });
            });
        }


        public void Push()
        {
            using (var repo = new Repository(Local))
            {
                var options = new PushOptions
                {
                    CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    {
                        return new UsernamePasswordCredentials()
                        {
                            Username = username,
                            Password = password
                        };
                    }),
                    OnPushTransferProgress = (c, t, b) => ProcessHandler.OnProgress($"current:{c},total:{t},bytes:{b}")
                };
                repo.Network.Push(repo.Branches["master"], options);
            }
        }

        public void Commit()
        {
            var content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            using (var repo = new Repository(Local))
            {
                // Stage the file
                Commands.Stage(repo, "*");
                // Create the committer's signature and commit
                var author = new Signature(authorname, email, DateTime.Now);
                var committer = author;
                // Commit to the repository
                //var commit = repo.Commit(content, author, committer);
            }
        }


        public IProcessHandler ProcessHandler { get; set; } = new DefaultProcessHandler();

        private class DefaultProcessHandler : IProcessHandler
        {
            public bool OnProgress(string message)
            {
                return true;
            }
        }

    }


    public interface IProcessHandler
    {
        bool OnProgress(string message);
    }
}
