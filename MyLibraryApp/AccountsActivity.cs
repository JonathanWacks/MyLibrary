using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MyLibrary;

namespace MyLibraryApp
{
	[Activity(Label = "Accounts")]			
	public class AccountsActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Accounts);

			var lv = FindViewById<ListView>(Resource.Id.listView);

            var accounts = MainActivity.AccountManager.GetAll();         

            lv.Adapter = new ArrayAdapter<Account>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, accounts.ToArray());	
			lv.ItemClick += OnItemClick;
		}

		void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			int position = e.Position; // e.Position is the position in the list of the item the user touched

			var intent = new Intent(this, typeof(EditLibraryActivity));

			intent.PutExtra("AccountPosition", position);

			StartActivity(intent);	
		}
	}
}