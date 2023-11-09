using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using MyLibrary;

namespace MyLibraryApp
{
    [Activity(Label = "Libraries")]
    public class LibrariesActivity : Activity
    {
        private LibrariesAdapter _adapter;
        private RecyclerView _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Libraries);

            _view = FindViewById<RecyclerView>(Resource.Id.libraries);

            _adapter = new LibrariesAdapter(new EntityManager<SqlLibrary>(Utils.GetDatabasePath(this)));

            _view.SetLayoutManager(new LinearLayoutManager(this));
            _view.SetAdapter(_adapter);

            //var libraries = MainActivity.LibraryManager.GetAll();

            //_adapter = new ArrayAdapter<Library>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, libraries.ToArray());
            //_lv.Adapter = _adapter;
            //_lv.ChoiceMode = ChoiceMode.Multiple;            
            //_lv.ItemClick += OnItemClick;

            FindViewById<Button>(Resource.Id.newButton).Click += LibrariesActivity_Click;
            FindViewById<Button>(Resource.Id.deleteButton).Click += OnDeleteClick;

            _adapter.ItemClick += OnItemClick;
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            //var checkedItems = _lv.GetCheckedItemIds();            
            //foreach (var item in checkedItems)
            //{
            //    MainActivity.LibraryManager.Delete(new SqlId((int)item));
            //    _adapter.Remove(_adapter.GetItem(item));
            //}            
            //_adapter.NotifyDataSetChanged();
        }

        private void LibrariesActivity_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(EditLibraryActivity));

            StartActivityForResult(intent, (int)Action.Add);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if(resultCode == Result.Ok)
            {
                //var libraries = MainActivity.LibraryManager.GetAll();
                //_adapter = new ArrayAdapter<Library>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, libraries.ToArray());
                ////_lv = FindViewById<ListView>(Resource.Id.listView);
                //_lv.Adapter = _adapter;
                ////_adapter.NotifyDataSetChanged();
                _adapter.NotifyDataSetChanged();
            }
            //if (resultCode == Result.Ok)
            //{
            //    var name = data.GetStringExtra("name");
            //    var type = (LibraryType)data.GetIntExtra("type", -1);
            //    var affiliate = data.GetStringExtra("affiliate");

            //    switch ((Action)requestCode)
            //    {
            //        case Action.Add:

            //            MainActivity.LibraryManager.Add(new Library { Name = name, Type = type, Affiliate = affiliate });
            //            _adapter.NotifyDataSetChanged();
            //            break;

            //        case Action.Edit:

            //            var id = data.GetIntExtra("id", -1);
            //            MainActivity.LibraryManager.Edit(new Library { Name = name, Type = type, Affiliate = affiliate, Id = new SqlId(id) });
            //            _adapter.NotifyDataSetChanged();
            //            break;
            //    }
            //}
        }

        private enum Action
        {
            Add = 1,
            Edit = 2
        }

        void OnItemClick(object sender, LibrariesAdapterClickEventArgs e)
        {
            int position = e.Position;

            var intent = new Intent(this, typeof(EditLibraryActivity));

            intent.PutExtra("LibraryPosition", position);
            var library = _adapter.LibraryManager.Get(position);
            intent.PutExtra("Library", new LibraryParcelable { Library = library });

            StartActivityForResult(intent, (int)Action.Edit);
        }
    }
}