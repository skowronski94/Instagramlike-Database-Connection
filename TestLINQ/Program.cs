using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLINQ;

namespace InstagramLINQtoSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = "server-1337.database.windows.net";
                cb.UserID = "CommonUser"; // read only
                cb.Password = "instagram1718;;";
                cb.InitialCatalog = "generailDB";

                using (InstagramDataContext db = new InstagramDataContext(cb.ConnectionString))
                {
                    #region Select
                    IEnumerable<string> names =
                                        from u in db.comments
                                        orderby u.id
                                        select u.comment_text;
                    foreach (string n in names)
                    {
                        Console.WriteLine(n);
                    }
                    Console.WriteLine();
                    #endregion

                    #region Update
                    /*
                    string upUsername = "NewOnewo";
                    user upUser =
                        (from u in db.users
                         orderby u.id
                         where u.id == 1
                         select u).Single<user>();
                    upUser.username = upUsername;
                    Console.WriteLine($"Update id = 1 username to '{upUsername}'");
                    Console.WriteLine();
                    */
                    #endregion

                    #region Insert
                    /*
                    string newUsername = "NewOnewo";
                    user newUser = new user { username = newUsername };
                    db.users.InsertOnSubmit(newUser);
                    Console.WriteLine($"Insert username to '{newUsername}'");
                    Console.WriteLine();
                    */
                    #endregion

                    #region Delete
                    /*
                    user delUser =
                        (from u in db.users
                         orderby u.id descending
                         select u).First<user>();
                    if (delUser != null)
                        db.users.DeleteOnSubmit(delUser);
                    Console.WriteLine($"Delete Last record");
                    Console.WriteLine();
                    */
                    #endregion

                    db.SubmitChanges();

                    #region Select 
                    names =
                        from u in db.users
                        orderby u.id
                        select u.username;
                    foreach (string n in names)
                    {
                        Console.WriteLine(n);
                    }
                    // Console.WriteLine($"There are {names.Count()} users");
                    Console.WriteLine();
                    #endregion

                }

                #region Select --Association Update--
                DataLoadOptions dl = new DataLoadOptions();
                dl.LoadWith<user>(u => u.photos);
                dl.AssociateWith<user>(u => u.photos.Where(p => true));

                using (InstagramDataContext db = new InstagramDataContext(cb.ConnectionString))
                {
                    // db.LoadOptions = dl;
                    // db.Log = Console.Out;
                    var users = from u in db.users select u;
                    foreach (var u in users)
                    {
                        Console.WriteLine($"There are {u.photos.Count()} products in category {u.username}");
                    }
                }
                # endregion
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();

        }
    }
}
