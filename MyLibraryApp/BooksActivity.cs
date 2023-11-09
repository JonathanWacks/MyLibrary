using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using MyLibrary;

namespace MyLibraryApp
{
    [Activity(Label = "Books")]			
	public class BooksActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Books);

			var lv = FindViewById<ListView>(Resource.Id.listView);

            var firstAccount = MainActivity.AccountManager.GetAll().ElementAt(1);                                

            lv.Adapter = new ArrayAdapter<Book>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, firstAccount.LibraryUser.GetBooksAsync().Result.ToArray());	
			//lv.ItemClick += OnItemClick;
		}

		//void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
		//{
		//	int position = e.Position; // e.Position is the position in the list of the item the user touched

		//	var intent = new Intent(this, typeof(EditLibraryActivity));

		//	intent.PutExtra("AccountPosition", position);

		//	StartActivity(intent);	
		//}
	}
}