using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.IO;
using SQLite;
using MyLibrary;

namespace MyLibraryApp
{
	[Activity(Label = "My Library App", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        public static ILibraryManager LibraryManager;
        public static IUserManager UserManager;
        public static IAccountManager AccountManager;

        protected override void OnCreate(Bundle bundle)
		{            
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "mylibrary.db3");
            LibraryManager = new LibraryManager(path);
            UserManager = new UserManager(path);
            AccountManager = new AccountManager(path, LibraryManager, UserManager);

            base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);

			FindViewById<Button>(Resource.Id.accountsButton).Click += OnAccountsClick;
			FindViewById<Button>(Resource.Id.librariesButton).Click += OnLibrariesClick;
            FindViewById<Button>(Resource.Id.usersButton).Click += OnUsersClick;
			//FindViewById<Button>(Resource.Id.booksButton).Click += OnBooksClick;
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

        //void OnBooksClicks(object sender, EventArgs e)
        //{
        //	StartActivity(typeof(AboutActivity));
        //}       
	}
}