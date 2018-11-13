using BuiltInFonts.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BuiltInFonts
{
    public static class Fonts
    {

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
           IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        static PrivateFontCollection fontCol = new PrivateFontCollection();

        static FontFamily _NovaSquare = null;
        public static FontFamily NovaSquare
        {
            get
            {
                if (_NovaSquare == null) _NovaSquare = LoadFont(Resources.NovaSquare);
                return _NovaSquare;
            }
        }
        static FontFamily _Quesha = null;
        public static FontFamily Quesha
        {
            get
            {
                if (_Quesha == null) _Quesha = LoadFont(Resources.Quesha);
                return _Quesha;
            }
        }

        static FontFamily _Trench = null;
        public static FontFamily Trench
        {
            get
            {
                if (_Trench == null) _Trench = LoadFont(Resources.trench100free);
                return _Trench;
            }
        }


        /// <summary>
        /// Loads the font from a resource
        /// </summary>
        static FontFamily LoadFont(byte[] fontData)
        {
            int index = fontCol.Families.Length;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fontCol.AddMemoryFont(fontPtr, fontData.Length);
            AddFontMemResourceEx(fontPtr, (uint)fontData.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
            return fontCol.Families[index];
        }
    }
}
