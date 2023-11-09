using System;
using Android.OS;
using Android.Runtime;
using Java.Interop;
using MyLibrary;

namespace MyLibraryApp
{
    //public class JLibrary : Java.Lang.Object
    //{
    //    public JLibrary(Library library) { Library = library; }

    //    public Library Library { get; set; }

    //    public override string ToString()
    //    {
    //        return Library.ToString();
    //    }
    //}

    public class JUser : Java.Lang.Object
    {
        public JUser(User user) { User = user; }

        public User User { get; set; }

        public override string ToString()
        {
            return User.ToString();
        }
    }

    public class LibraryParcelable : Java.Lang.Object, IParcelable
    {
        private static readonly ParcelableCreator<LibraryParcelable> _creator
            = new ParcelableCreator<LibraryParcelable>((parcel) => new LibraryParcelable(parcel));

        [ExportField("CREATOR")]
        public static ParcelableCreator<LibraryParcelable> GetCreator()
        {
            return _creator;
        }

        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            dest.WriteString(Library.Name);
            dest.WriteString(Library.Host);
            dest.WriteInt(Library.Id);
            //dest.WriteInt((int)Library.Type);
        }

        private LibraryParcelable(Parcel parcel)
        {
            Library = new SqlLibrary
            {
                Name = parcel.ReadString(),
                Host = parcel.ReadString(),
                Id = parcel.ReadInt()
                //,
                //Type = (LibraryType)parcel.ReadInt()
            };
        }
        public SqlLibrary Library { get; set; }

        public LibraryParcelable() { }
    }

    public class UserParcelable : Java.Lang.Object, IParcelable
    {
        private static readonly ParcelableCreator<UserParcelable> _creator
            = new ParcelableCreator<UserParcelable>((parcel) => new UserParcelable(parcel));

        [ExportField("CREATOR")]
        public static ParcelableCreator<UserParcelable> GetCreator()
        {
            return _creator;
        }

        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            dest.WriteString(User.Name);
            //dest.WriteInt(User.Id);
        }

        private UserParcelable(Parcel parcel)
        {
            User = new User
            {
                Name = parcel.ReadString(),
                //Id = parcel.ReadInt()
            };
        }
        public User User { get; set; }

        public UserParcelable() { }
    }


    public class ParcelableCreator<T> : Java.Lang.Object, IParcelableCreator where T : Java.Lang.Object, new()
    {
        private readonly Func<Parcel, T> _create;

        public ParcelableCreator(Func<Parcel, T> create)
        {
            _create = create;
        }

        public Java.Lang.Object CreateFromParcel(Parcel parcel)
        {
            return _create(parcel);
        }

        public Java.Lang.Object[] NewArray(int size)
        {
            return new T[size];
        }
    }
}