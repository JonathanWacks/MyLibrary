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
        private ListView _lv;
        private ArrayAdapter<Account> _adapter;
    
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Accounts);

            _lv = FindViewById<ListView>(Resource.Id.listView);

            var accounts = MainActivity.AccountManager.GetAll();

            _adapter = new ArrayAdapter<Account>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, accounts.ToArray());
            _lv.Adapter = _adapter;
            _lv.ItemClick += OnItemClick;

            FindViewById<Button>(Resource.Id.newButton).Click += OnNewClick;
            FindViewById<Button>(Resource.Id.deleteButton).Click += OnDeleteClick;
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            var checkedItems = _lv.GetCheckedItemIds();
            foreach (var item in checkedItems)
            {
                MainActivity.AccountManager.Delete((int)item);
            }
            _adapter.NotifyDataSetChanged();
        }

        private void OnNewClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(EditAccountActivity));

            StartActivityForResult(intent, (int)Action.Add);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                var library = (data.GetParcelableExtra("library") as LibraryParcelable).Library;
                var user = (data.GetParcelableExtra("user") as UserParcelable).User;
                var cardNo = data.GetStringExtra("cardNo");
                var pin = data.GetStringExtra("pin");
                
                switch ((Action)requestCode)
                {
                    case Action.Add:

                        MainActivity.AccountManager.Add(new Account { Library = library, User = user, Login = new Login { CardNo = cardNo, PIN = pin } });
                        _adapter.NotifyDataSetChanged();
                        break;

                    case Action.Edit:

                        var id = data.GetIntExtra("id", -1);
                        //MainActivity.AccountManager.Edit(new Account { Id = id, Library = library, User = user, Login = new Login { CardNo = cardNo, PIN = pin } });
                        _adapter.NotifyDataSetChanged();
                        break;
                }
            }
        }

        private enum Action
        {
            Add = 1,
            Edit = 2
        }

        void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;

            var intent = new Intent(this, typeof(EditAccountActivity));

            intent.PutExtra("AccountPosition", position);

            StartActivityForResult(intent, (int)Action.Edit);
        }
    }
}