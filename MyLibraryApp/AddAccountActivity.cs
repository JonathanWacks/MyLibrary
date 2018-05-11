using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace MyLibraryApp
{
	[Activity(Label = "Add Account")]			
	public class AddAccountActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.AddAccount);

			FindViewById<Button>(Resource.Id.saveButton  ).Click += OnSaveClick;
			FindViewById<Button>(Resource.Id.cancelButton).Click += OnCancelClick;
		}

		void OnSaveClick(object sender, EventArgs e)
		{
			var library = FindViewById<EditText>(Resource.Id.libraryInput).Text;
			var firstName = FindViewById<EditText>(Resource.Id.firstNameInput).Text;
            var lastName = FindViewById<EditText>(Resource.Id.lastNameInput).Text;
            //var type = FindViewById<>

            var intent = new Intent();

			intent.PutExtra("Library", library);
            intent.PutExtra("FirstName", firstName);
            intent.PutExtra("LastName", lastName);
            //intent.PutExtra("Type", type);

			SetResult(Result.Ok, intent);

			Finish();
		}

		void OnCancelClick(object sender, EventArgs e)
		{
			Finish();
		}
	}
}