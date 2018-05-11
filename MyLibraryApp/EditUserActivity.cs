using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MyLibrary;

namespace MyLibraryApp
{
	[Activity]			
	public class EditUserActivity : Activity
	{
        private IId _id;

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.EditUser);

			FindViewById<Button>(Resource.Id.saveButton).Click += OnSaveClick;
			FindViewById<Button>(Resource.Id.cancelButton).Click += OnCancelClick;

            var position = Intent.GetIntExtra("UserPosition", -1);
            if (position == -1)
            {
                SetTitle(Resource.String.add_user);
            }
            else
            {
                PopulateFields(position);
                SetTitle(Resource.String.edit_user);
            }            
        }

        private void PopulateFields(int position)
        {
            var user = MainActivity.UserManager.Get(new SqlId(position + 1));

            _id = user.Id;

            FindViewById<EditText>(Resource.Id.firstNameInput).Text = user.FirstName;
            FindViewById<EditText>(Resource.Id.lastNameInput).Text = user.LastName;
        }

        void OnSaveClick(object sender, EventArgs e)
		{
			var firstName = FindViewById<EditText>(Resource.Id.firstNameInput).Text;
            var lastName = FindViewById<EditText>(Resource.Id.lastNameInput).Text;

            var intent = new Intent();

            intent.PutExtra("firstName", firstName);
            intent.PutExtra("lastName", lastName);
            if (_id != null)
            {
                intent.PutExtra("id", ((SqlId)_id).Id);
            }

			SetResult(Result.Ok, intent);

			Finish();
		}

		void OnCancelClick(object sender, EventArgs e)
		{
			Finish();
		}
	}
}