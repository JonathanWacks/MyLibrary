using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MyLibrary;

namespace MyLibraryApp
{
    [Activity(Label = "Libraries")]
    public class LibrariesActivity : Activity
    {
        private ArrayAdapter<Library> _adapter;
        private ListView _lv;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Libraries);

            _lv = FindViewById<ListView>(Resource.Id.listView);

            var libraries = MainActivity.LibraryManager.GetAll();

            _adapter = new ArrayAdapter<Library>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, libraries.ToArray());
            _lv.Adapter = _adapter;
            _lv.ChoiceMode = ChoiceMode.Multiple;            
            _lv.ItemClick += OnItemClick;

            FindViewById<Button>(Resource.Id.newButton).Click += LibrariesActivity_Click;
            FindViewById<Button>(Resource.Id.deleteButton).Click += OnDeleteClick;
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            var checkedItems = _lv.GetCheckedItemIds();
            foreach (var item in checkedItems)
            {
                MainActivity.LibraryManager.Delete(new SqlId((int)item));
            }
            _adapter.NotifyDataSetChanged();
        }

        private void LibrariesActivity_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(EditLibraryActivity));

            StartActivityForResult(intent, 1);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if(resultCode == Result.Ok)
            {
                var name = data.GetStringExtra("name");
                var type = (LibraryType)data.GetIntExtra("type", -1);
                var uri = new Uri(data.GetStringExtra("uri"));

                if (requestCode == 1)
                {
                    MainActivity.LibraryManager.Add(new Library { Name = name, Type = type, Uri = uri });
                    _adapter.NotifyDataSetChanged();
                }

                else if(requestCode == 2)
                {
                    var id = data.GetIntExtra("id", -1);
                    MainActivity.LibraryManager.Edit(new Library { Name = name, Type = type, Uri = uri, Id = new SqlId(id) });
                    _adapter.NotifyDataSetChanged();
                }
            }
        }

        void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;

            var intent = new Intent(this, typeof(EditLibraryActivity));

            intent.PutExtra("LibraryPosition", position);

            StartActivityForResult(intent, 2);
        }
    }
}