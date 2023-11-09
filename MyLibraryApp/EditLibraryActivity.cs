using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MyLibrary;

namespace MyLibraryApp
{
    [Activity]			
	public class EditLibraryActivity : Activity
	{
        private int? _id;

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.EditLibrary);

			FindViewById<Button>(Resource.Id.saveButton).Click += OnSaveClick;
			FindViewById<Button>(Resource.Id.cancelButton).Click += OnCancelClick;

            //var spinner = FindViewById<Spinner>(Resource.Id.libraryTypes);           
            //spinner.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, Enum.GetNames(typeof(LibraryType)));

            var position = Intent.GetIntExtra("LibraryPosition", -1);
            var parcel = (LibraryParcelable)Intent.GetParcelableExtra("Library");
            var library = parcel.Library;

            if (position == -1)
            {
                SetTitle(Resource.String.add_library);
            }
            else
            {
                PopulateFields(library);
                SetTitle(Resource.String.edit_library);
            }            
        }

        private void PopulateFields(Library library)
        {
            //var library = MainActivity.LibraryManager.Get(position + 1);

            _id = (library as SqlLibrary).Id;

            FindViewById<EditText>(Resource.Id.nameInput).Text = library.Name;
            FindViewById<EditText>(Resource.Id.affiliateInput).Text = library.Host;
            //FindViewById<Spinner>(Resource.Id.libraryTypes).SetSelection((int)library.Type);
        }

	    private void OnSaveClick(object sender, EventArgs e)
		{
            var name = FindViewById<EditText>(Resource.Id.nameInput).Text;
            //var type = (LibraryType)FindViewById<Spinner>(Resource.Id.libraryTypes).SelectedItemPosition;
            var affiliate = FindViewById<EditText>(Resource.Id.affiliateInput).Text;

            //         var intent = new Intent();

            //         intent.PutExtra("name", name);
            //         intent.PutExtra("type", type);
            //         intent.PutExtra("affiliate", affiliate);
            //         if (_id != null)
            //         {
            //             intent.PutExtra("id", ((SqlId)_id).Id);
            //         }

            //var name = data.GetStringExtra("name");
            //var type = (LibraryType)data.GetIntExtra("type", -1);
            //var affiliate = data.GetStringExtra("affiliate");

            if (_id == null)
            {
                MainActivity.LibraryManager.Add(new Library { Name = name, //Type = type,
                                                                           Host = affiliate });
            }
            else
            {
                //MainActivity.LibraryManager.Edit(new Library { Name = name, //Type = type,
                //                                                            Host = affiliate, Id = _id.Value });
            }

            SetResult(Result.Ok);

            Finish();
		}

	    private void OnCancelClick(object sender, EventArgs e)
		{
			Finish();
		}
	}
}