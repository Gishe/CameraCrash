using System;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Android.Util;
using Java.IO;

namespace CameraCrash
{
    [Activity(Label = "CameraCrash", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {

        private ImageAdapter adapter;
        private string m_strPhotoPath;
        private string m_strPreviousPhotoPath;
        private GridView m_gridview;

        private const int REQ_CODE_PICTURE = 1337;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            adapter = new ImageAdapter(this);

            //// Get our button from the layout resource,
            //// and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += ButtonOnClick;

            m_gridview = (GridView)FindViewById(Resource.Id.gridView1);
            m_gridview.Adapter = adapter;
            m_gridview.ItemClick += ButtonOnClick;
            StartCamera();
        }

        private void ButtonOnClick(object _sender, EventArgs _eventArgs)
        {
            StartCamera();
        }

        public void StartCamera()
        {
            try
            {
                m_strPhotoPath = FileSystem.CreatePublicFile(ApplicationContext, Guid.NewGuid() + ".jpg", true);
                
                File photoFile = new File(m_strPhotoPath);

                try
                {
                    if (!photoFile.Exists())
                    {
                        // For some reason the system couldn't create the temp file for us
                        throw new FileNotFoundException(string.Format("Could not find temporary file {0}", m_strPhotoPath));
                    }
                }
                catch (Exception ex)
                {
                    
                }

                Android.Net.Uri photoFileUri = Android.Net.Uri.FromFile(photoFile);

                Intent intent = new Intent(MediaStore.ActionImageCapture);
                intent.PutExtra(MediaStore.ExtraOutput, photoFileUri);
                StartActivityForResult(intent, REQ_CODE_PICTURE);
            }
            catch (Exception ex)
            {
                Log.Error("CameraCrash",ex.Message);
            }

            //create new Intent

            //intent.AddFlags(ActivityFlags.SingleTop);
            //intent.PutExtra(MediaStore.ExtraOutput, MediaStore.Images.Media.ExternalContentUri);
            //intent.PutExtra(MediaStore.ExtraVideoQuality, 1);
            //this.StartActivityForResult(intent, REQ_CODE_PICTURE);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == REQ_CODE_PICTURE && resultCode != Result.Canceled)
            {
                Log.Verbose("CameraCrash", "Photo returned.");

                var bmp = LoadAndResizeBitmap(m_strPhotoPath);
                adapter.Add(bmp);
                
                GC.Collect();
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        public Bitmap LoadAndResizeBitmap(string fileName)
        {
            
            float px = TypedValue.ApplyDimension(ComplexUnitType.Dip, 90, Resources.DisplayMetrics);
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > px || outWidth > px)
            {
                inSampleSize = (int)(outWidth > outHeight
                    ? outHeight / px
                    : outWidth / px);
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }
}

