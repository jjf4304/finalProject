using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageApp
{
	public partial class Form1 : Form
	{
		byte[] originalImagePixels;
		int originalHeight;
		int originalWidth;
		Bitmap originalBitmap;

		public Form1()
		{
			InitializeComponent();
			Bitmap image = new Bitmap(pictureBox1.Image);
			originalImagePixels = copyToPixelArray(image);
			originalHeight = image.Height;
			originalWidth = image.Width * 4; // each of r g b and a fit in 1 byte so total is 4 bytes
			originalBitmap = image;
		}

		// copies a bitmap to a pixel array so that it's 
		// easy to change the pixels en masse
		private byte[] copyToPixelArray(Bitmap image)
		{
			Rectangle wholeImage = new Rectangle(0, 0, image.Width, image.Height);
			BitmapData bitmapSrc = image.LockBits(wholeImage, ImageLockMode.ReadOnly, image.PixelFormat);
			IntPtr srcPtr = bitmapSrc.Scan0;
			int size = Math.Abs(bitmapSrc.Stride) * image.Height;
			byte[] pixels = new byte[size];
			Marshal.Copy(srcPtr, pixels, 0, size);
			image.UnlockBits(bitmapSrc);
			return pixels;

		}

		// copies a pixel array of bytes into a new
		// Bitmap object
		private Bitmap copyToBitmap(byte[] pixels)
		{
			Bitmap image = new Bitmap(originalBitmap); // create new bitmap
			Rectangle wholeImage = new Rectangle(0, 0, image.Width, image.Height);
			BitmapData bitmapSrc = image.LockBits(wholeImage, ImageLockMode.ReadWrite, image.PixelFormat);
			IntPtr srcPtr = bitmapSrc.Scan0;
			int size = Math.Abs(bitmapSrc.Stride) * image.Height;
			Marshal.Copy(pixels, 0, srcPtr, size);
			image.UnlockBits(bitmapSrc);
			return image;
		}

		// traverse through the image pixels along the y 
		// axis of the image and grayscale them
		private void grayscaleExample1(byte[] processedPixels)
		{
			for (int y = 0; y < originalHeight; y++)
			{
				for (int x = 0; x < originalWidth; x+=4)
				{
					int index = y * originalWidth + x;

					byte averagePixel = (byte)((originalImagePixels[index]
						+ originalImagePixels[index + 1]
						+ originalImagePixels[index + 2] / 3));

					processedPixels[index] = averagePixel;
					processedPixels[index + 1] = averagePixel;
					processedPixels[index + 2] = averagePixel;
					processedPixels[index + 3] = originalImagePixels[index + 3];
				}
			}
		}

		//traverse through the image pixels along the x
		// axis of the image and grayscale them
		private void grayscaleExample2(byte[] processedPixels)
		{
			for (int x = 0; x < originalWidth; x+=4)
			{
				for (int y = 0; y < originalHeight; y++)
				{
					int index = y * originalWidth + x;
					byte averagePixel = (byte)((originalImagePixels[index]
						+ originalImagePixels[index + 1]
						+ originalImagePixels[index + 2] / 3));

					processedPixels[index] = averagePixel;
					processedPixels[index + 1] = averagePixel;
					processedPixels[index + 2] = averagePixel;
					processedPixels[index + 3] = originalImagePixels[index + 3];
				}
			}
		}

		// grayscales the image using the Properties of 
		// a bitmap.
		private Bitmap usingSetPixel(Bitmap bitmap)
		{
			Bitmap returnBitMap = new Bitmap(bitmap);
			//Loop through image pixels to set color
			for (int x = 0; x < bitmap.Width; x++)
			{
				for (int y = 0; y < bitmap.Height; y++)
				{
					Color pixelColor = bitmap.GetPixel(x, y);
					byte averagePixel = (byte)((pixelColor.R + pixelColor.G 
						+ pixelColor.B) / 3);
					Color newColor = Color.FromArgb(averagePixel, averagePixel,
						averagePixel);
					returnBitMap.SetPixel(x, y, newColor);
				}
			}

			return returnBitMap;
		}


        // Button event
        private void Test1(object sender, EventArgs e)
        {
			Bitmap usingBitMap = new Bitmap(originalBitmap);
			byte[] processedPixels = new byte[originalHeight * originalWidth];

			// Test Example 1
			
			for (int i = 0; i < 20; i++)
			{
				grayscaleExample1(processedPixels);
				pictureBox1.Image = copyToBitmap(processedPixels);
				processedPixels = new byte[originalHeight * originalWidth];
			}
			
			//Test Example2

			for (int i = 0; i < 20; i++)
			{
				grayscaleExample2(processedPixels);
				pictureBox1.Image = copyToBitmap(processedPixels);
				processedPixels = new byte[originalHeight * originalWidth];
			}

			pictureBox1.Image = originalBitmap;

			// Test Using Set Pixel

			for (int i = 0; i < 20; i++)
			{
				pictureBox1.Image = usingSetPixel(usingBitMap);
			}


		}

    }
}
