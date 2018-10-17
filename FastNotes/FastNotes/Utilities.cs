using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastNotes
{
	public static class Utilities
	{
		public static Bitmap ColorTint(this Bitmap sourceBitmap, Color tint)
		{
			BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
									sourceBitmap.Width, sourceBitmap.Height),
									ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

			Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

			sourceBitmap.UnlockBits(sourceData);


			for (int k = 0, endk = pixelBuffer.Length - 4; k < endk; k += 4)
			{
				pixelBuffer[k + 2] = tint.R; // I don't...
				pixelBuffer[k + 1] = tint.G; // Know..
				pixelBuffer[k] = tint.B; // Why the order is inverted
			}


			Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


			BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
									resultBitmap.Width, resultBitmap.Height),
									ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


			Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
			resultBitmap.UnlockBits(resultData);


			return resultBitmap;
		}

	}
}
