using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.IO;
using MyLibrary;
using Android.Content.Res;

namespace MyLibraryApp
{
    public static class Utils
    {
        public static string GetDatabasePath(Context context)
        {
            var databaseName = context.GetString(Resource.String.database_name);
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), $"{databaseName}.db3");
        }
    }

    [Activity(Label = "My Library App", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        public static IEntityManager<Library> LibraryManager;
        public static IEntityManager<User> UserManager;
        public static IEntityManager<Account> AccountManager;

        protected override void OnCreate(Bundle bundle)
		{                        
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "mylibrary.db3");
            //LibraryManager = new EntityManager<Library>(path);
            //UserManager = new EntityManager<User>(path);
            //AccountManager = new EntityManager<Account>(path);//, LibraryManager, UserManager);

            base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);

			FindViewById<Button>(Resource.Id.accountsButton).Click += OnAccountsClick;
			FindViewById<Button>(Resource.Id.librariesButton).Click += OnLibrariesClick;
            FindViewById<Button>(Resource.Id.usersButton).Click += OnUsersClick;
			FindViewById<Button>(Resource.Id.booksButton).Click += OnBooksClick;
		}

        void OnUsersClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(UsersActivity));

            StartActivity(intent);
        }

        void OnAccountsClick(object sender, EventArgs e)
		{            
			var intent = new Intent(this, typeof(AccountsActivity));

			StartActivity(intent);
		}

        void OnLibrariesClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(LibrariesActivity));

            StartActivity(intent);
        }

        void OnBooksClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(BooksActivity));

            StartActivity(intent);
        }
    }
}