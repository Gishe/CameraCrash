using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;

namespace CameraCrash
{
    public class ImageAdapter : BaseAdapter
    {
        private List<Bitmap> Images = new List<Bitmap>();
        private Context m_context;
        public ImageAdapter(Context c)
        {
            m_context = c;
        }
        
        public override Object GetItem(int position)
        {
            return Images[position];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;
            if (convertView == null)
            {
                // if it's not recycled, initialize some attributes
                imageView = new ImageView(m_context);
                imageView.LayoutParameters = new GridView.LayoutParams(85, 85);
                imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                imageView.SetPadding(8, 8, 8, 8);
            }
            else
            {
                imageView = (ImageView)convertView;
            }
            
            imageView.SetImageBitmap(Images[position]);
            
            return imageView;
        }

        public void Add(Bitmap _bmp)
        {
            Images.Add(_bmp);
            NotifyDataSetChanged();
        }

        public override int Count { get { return Images.Count; } }
    }
}