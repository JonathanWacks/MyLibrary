using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MyLibrary;

namespace MyLibraryApp
{
    [Activity(Label = "Users")]
    public class UsersActivity : Activity
    {
        private ArrayAdapter<User> _adapter;
        private ListView _lv;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Users);
            _lv = FindViewById<ListView>(Resource.Id.listView);
            _lv.ChoiceMode = ChoiceMode.Multiple;
            
            var users = MainActivity.UserManager.GetAll();

            _adapter = new ArrayAdapter<User>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, users.ToArray());

            _lv.Adapter = _adapter;
            _lv.ItemClick += OnItemClick;

            FindViewById<Button>(Resource.Id.newButton).Click += UsersActivity_Click;
            FindViewById<Button>(Resource.Id.deleteButton).Click += OnDeleteClick;
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            var checkedItems = _lv.GetCheckedItemIds();
            foreach (var item in checkedItems)
            {
                MainActivity.UserManager.Delete(new SqlId((int)item));
            }
            _adapter.NotifyDataSetChanged();
        }

        private void UsersActivity_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(EditUserActivity));

            StartActivityForResult(intent, 1);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if(resultCode == Result.Ok)
            {
                var firstName = data.GetStringExtra("firstName");
                var lastName = data.GetStringExtra("lastName");

                switch (requestCode)
                {
                    case 1:
                    
                        var userAdded = new User {FirstName = firstName, LastName = lastName};
                        MainActivity.UserManager.Add(userAdded);
                        Toast.MakeText(this, $"{userAdded} added", ToastLength.Short);
                        _adapter.NotifyDataSetChanged();
                    
                        break;
                    case 2:
                    
                        var id = data.GetIntExtra("id", -1);
                        var userEdited = new User {FirstName = firstName, LastName = lastName, Id = new SqlId(id)};
                        MainActivity.UserManager.Edit(userEdited);
                        Toast.MakeText(this, $"{userEdited} updated", ToastLength.Short);
                        _adapter.NotifyDataSetChanged();
                    
                        break;
                    default:
                        throw new Exception($"Unknown result code in {nameof(UsersActivity)}");
                }
            }
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(EditUserActivity));

            intent.PutExtra("UserPosition", e.Position);

            StartActivityForResult(intent, 2);
        }
    }
}