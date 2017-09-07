using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CameraCrash
{
    class FileSystem
    {
        /// <summary>
        /// Creates a stream to a publicly file
        /// </summary>
        /// <param name="_context">Android Context</param>
        /// <param name="_filename">Name of the file for which to create the stream</param>
        /// <param name="_writable">True if the file should be publicly writable, otherwise false</param>
        /// <returns>Stream</returns>
        public static Stream CreatePublicStream(Context _context, string _filename, bool _writable = false)
        {
            var permission = _writable ? FileCreationMode.WorldWriteable : FileCreationMode.WorldReadable;
            return _context.OpenFileOutput(_filename, permission);
        }

        /// <summary>
        /// Creates a publicly available file
        /// </summary>
        /// <param name="_context">Android Context</param>
        /// <param name="_filename">Name of the file for which to create the stream</param>
        /// <param name="_writable">True if the file should be publicly writable, otherwise false</param>
        /// <returns>Absolute Path to the new file</returns>
        public static string CreatePublicFile(Context _context, string _filename, bool _writable = false)
        {
            using (var fs = CreatePublicStream(_context, _filename, _writable))
            {
                fs.Close();
            }

            return GetPublicFilePath(_context, _filename);
        }


        /// <summary>
        /// Gets the absolute path to the publicly available file created from the Context
        /// </summary>
        /// <param name="_context">Android Context</param>
        /// <param name="_filename">File for which to return the path</param>
        /// <returns></returns>
        public static string GetPublicFilePath(Context _context, string _filename)
        {
            var file = _context.GetFileStreamPath(_filename);
            return file.AbsolutePath;
        }
    }
}