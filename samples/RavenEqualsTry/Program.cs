using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;
using Raven.Client.Indexes;
using System.Collections.Generic;
using System.Linq;

namespace RavenEqualsTry
{
    class Program
    {
        static void Main(string[] args)
        {
            const string DefaultDatabase = "EqualsTryOut";
            IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = DefaultDatabase
            }.Initialize();

            IndexCreation.CreateIndexes(typeof(Users).Assembly, store);
            store.DatabaseCommands.EnsureDatabaseExists(DefaultDatabase);

            using (var ses = store.OpenSession())
            {
                //var user = new User { Name = "TuGberK", Roles = new List<string> { "adMin", "GuEst" } };
                //ses.Store(user);
                //ses.SaveChanges();

                ////this works
                //var user = ses.Load<User>("uSeRs/1");

                ////this finds name:TuGberK
                //var user = ses.Query<User>().Where(usr => usr.Name == "tugberk").FirstOrDefault();

                ////this finds name:TuGberK
                //var user = ses.Query<User>().Where(usr => usr.Name.Equals("tugberk", StringComparison.Ordinal)).FirstOrDefault();

                ////this finds Roles:adMin
                //var users = ses.Query<User>().Where(usr => usr.Roles.Any(role => role == "admin")).ToArray();

                var user = new User { Name = "Irmak", Roles = new List<string> { "adMin", "GuEst" } };
                ses.Store(user);
                ses.SaveChanges();

                //This fails due to Turkish I
                var user1 = ses.Query<User>().Where(usr => usr.Name == "ırmak").FirstOrDefault();

                //this finds name:Irmak
                var user2 = ses.Query<User>().Where(usr => usr.Name == "IrMak").FirstOrDefault();

                //this works like a charm
                var retrievedUser = ses.Query<User, Users>().Where(usr => usr.Name == "ırmak").FirstOrDefault();
            }
        }
    }

    public class Users : AbstractIndexCreationTask<User>
    {
        public Users()
        {
            Map = users => from user in users
                           select new 
                           {
                               user.Name 
                           };

            Analyzers.Add(x => x.Name, typeof(LuceneAnalyzers.TurkishLowerCaseKeywordAnalyzer).AssemblyQualifiedName);
        }
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
