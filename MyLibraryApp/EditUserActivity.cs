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
        private int? _id;

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
            var user = MainActivity.UserManager.Get(position + 1);

            //_id = user.Id;

            FindViewById<EditText>(Resource.Id.firstNameInput).Text = user.Name;
        }

        void OnSaveClick(object sender, EventArgs e)
		{
			var name = FindViewById<EditText>(Resource.Id.firstNameInput).Text;

            var intent = new Intent();

            intent.PutExtra("name", name);
            if (_id != null)
            {
                intent.PutExtra("id", _id.Value);
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