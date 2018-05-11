using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using MyLibrary;

namespace MyLibraryApp
{
	[Activity]			
	public class EditLibraryActivity : Activity
	{
        private IId _id;

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.EditLibrary);

			FindViewById<Button>(Resource.Id.saveButton).Click += OnSaveClick;
			FindViewById<Button>(Resource.Id.cancelButton).Click += OnCancelClick;

            var spinner = FindViewById<Spinner>(Resource.Id.libraryTypes);           
            spinner.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, Enum.GetNames(typeof(LibraryType)));

            var position = Intent.GetIntExtra("LibraryPosition", -1);
            if (position == -1)
            {
                SetTitle(Resource.String.add_library);
            }
            else
            {
                PopulateFields(position);
                SetTitle(Resource.String.edit_library);
            }            
        }

        private void PopulateFields(int position)
        {
            var library = MainActivity.LibraryManager.Get(new SqlId(position + 1));

            _id = library.Id;

            FindViewById<EditText>(Resource.Id.nameInput).Text = library.Name;
            FindViewById<EditText>(Resource.Id.uriInput).Text = library.Uri.AbsoluteUri;
            FindViewById<Spinner>(Resource.Id.libraryTypes).SetSelection((int)library.Type);
        }

	    private void OnSaveClick(object sender, EventArgs e)
		{
			var name = FindViewById<EditText>(Resource.Id.nameInput).Text;
            var type = FindViewById<Spinner>(Resource.Id.libraryTypes).SelectedItemPosition;
            var uri = FindViewById<EditText>(Resource.Id.uriInput).Text;

		    if (!Patterns.WebUrl.Matcher(uri).Matches())
		    {
		        Toast.MakeText(ApplicationContext, "Invalid URL", ToastLength.Short).Show();
		        return;
		    }

            var intent = new Intent();

            intent.PutExtra("name", name);
            intent.PutExtra("type", type);
            intent.PutExtra("uri", uri);
            if (_id != null)
            {
                intent.PutExtra("id", ((SqlId)_id).Id);
            }

			SetResult(Result.Ok, intent);

			Finish();
		}

	    private void OnCancelClick(object sender, EventArgs e)
		{
			Finish();
		}
	}
}