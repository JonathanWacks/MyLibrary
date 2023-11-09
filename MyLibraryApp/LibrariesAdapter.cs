using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MyLibrary;
using System;

namespace MyLibraryApp
{
    internal class LibrariesAdapter : RecyclerView.Adapter
    {
        public IEntityManager<SqlLibrary> LibraryManager { get; private set; }

        public event EventHandler<LibrariesAdapterClickEventArgs> ItemClick;
        public event EventHandler<LibrariesAdapterClickEventArgs> ItemLongClick;

        public LibrariesAdapter(IEntityManager<SqlLibrary> libraryManager)
        {
            LibraryManager = libraryManager;            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(
                //Resource.Layout.Library
                Android.Resource.Layout.SimpleListItem1
                , parent, false);
            return new LibrariesAdapterViewHolder(itemView, OnClick, OnLongClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var library = LibraryManager.Get(position + 1);

            var holder = viewHolder as LibrariesAdapterViewHolder;
            holder.SetName(library.Name + " - " + (position + 1));
        }

        public override int ItemCount => LibraryManager.Count;

        void OnClick(LibrariesAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(LibrariesAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class LibrariesAdapterViewHolder : RecyclerView.ViewHolder
    {
        public void SetName(string name)
        {
            Name.Text = name;
        }

        public TextView Name { get; set; }

        public LibrariesAdapterViewHolder(View itemView, Action<LibrariesAdapterClickEventArgs> clickListener,
                            Action<LibrariesAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(
                //Resource.Id.libraryName
                Android.Resource.Id.Text1
                );
            itemView.Click += (sender, e) => clickListener(new LibrariesAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new LibrariesAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class LibrariesAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}