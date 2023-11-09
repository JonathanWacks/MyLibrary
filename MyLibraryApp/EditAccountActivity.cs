using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Linq;
using MyLibrary;

namespace MyLibraryApp
{
	[Activity]			
	public class EditAccountActivity : Activity
	{
        private int _id;

		protected override void OnCreate(Bundle bundle)
		{
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.EditAccount);

            FindViewById<Button>(Resource.Id.saveButton).Click += OnSaveClick;
            FindViewById<Button>(Resource.Id.cancelButton).Click += OnCancelClick;

            //FindViewById<Spinner>(Resource.Id.libraries).Adapter = new ArrayAdapter<JLibrary>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, MainActivity.LibraryManager.GetAll().Select(l => new JLibrary(l)).ToArray());
            FindViewById<Spinner>(Resource.Id.users).Adapter = new ArrayAdapter<JUser>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, MainActivity.UserManager.GetAll().Select(u => new JUser(u)).ToArray());

            var position = Intent.GetIntExtra("AccountPosition", -1);
            if (position == -1)
            {
                SetTitle(Resource.String.add_account);
            }
            else
            {
                PopulateFields(position);
                SetTitle(Resource.String.edit_account);
            }
        }

        private void PopulateFields(int position)
        {
            var account = MainActivity.AccountManager.Get(position + 1);

            //_id = account.Id;

            //FindViewById<Spinner>(Resource.Id.libraries).SetSelection(account.Library.Id);
            //FindViewById<Spinner>(Resource.Id.users).SetSelection(account.User.Id);
            FindViewById<EditText>(Resource.Id.cardNoInput).Text = account.Login.CardNo;
            FindViewById<EditText>(Resource.Id.pinInput).Text = account.Login.PIN;
        }

        void OnSaveClick(object sender, EventArgs e)
		{
            //var library = FindViewById<Spinner>(Resource.Id.libraries).SelectedItem as JLibrary;
            var user = FindViewById<Spinner>(Resource.Id.users).SelectedItem as JUser;
            var cardNo = FindViewById<EditText>(Resource.Id.cardNoInput).Text;
            var pin = FindViewById<EditText>(Resource.Id.pinInput).Text;

            var intent = new Intent();

			//intent.PutExtra("library", new LibraryParcelable { Library = library.Library } );
            intent.PutExtra("user", new UserParcelable { User = user.User });
            intent.PutExtra("cardNo", cardNo);
            intent.PutExtra("pin", pin);
            if (_id != null)
            {
                intent.PutExtra("id", _id);
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