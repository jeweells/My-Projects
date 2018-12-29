using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ColorManager;

namespace FastNotes
{

	public partial class ColorPicker : Form
	{
        public event Action<RGB> ColorChanged;
		public Color currentColorPicked;
		HSV currentShown = new HSV(0, 1, 1);
		Font font;

		public ColorPicker(Color startColor, Font customFont = null) : this()
		{
			SetStartColor(startColor);

			selectBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); ;
			cancelBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); ;
			font = customFont;
			if(font != null)
			{
				selectBtn.Font = font;
				cancelBtn.Font = font;
			}
		}

		public ColorPicker()
		{
			InitializeComponent();
		}

		public void SetStartColor(Color startColor)
		{
			currentShown = new HSV(new RGB(startColor.R, startColor.G, startColor.B));
		}
		public void ModifySV(float s, float v)
		{
			currentShown.S = s;
			currentShown.V = v;
			colorDot.Location = new Point(
				(int)(currentShown.S * colorDisplayer.Width + colorDotBoundsLT.X),
				(int)((1 - currentShown.V) * colorDisplayer.Height + colorDotBoundsLT.Y)
				);
		}
		
		public void ModifySV()
		{
			currentShown.S = Math.Abs(colorDot.Location.X -  colorDotBoundsLT.X) / (float)colorDisplayer.Width;
			currentShown.V =1- Math.Abs(colorDot.Location.Y  - colorDotBoundsLT.Y) / (float)colorDisplayer.Height;

		}

		public void ModifyH(int h)
		{
			int ch;
			if (h < 0) ch = 0;
			else if (h > 360) ch = 360;
			else ch = h;
			currentShown.H = ch;
			int x =(int) (maxRightHueDot * ch/(float)360f);
			hueDot.Location = new Point(x, hueDot.Location.Y);
		}
		void PickCurrentColor()
		{
			RGB rgb = new RGB(currentShown);
			colorPickedDisplayer.BackColor = Color.FromArgb(rgb.R, rgb.G, rgb.B);
            ColorChanged?.Invoke(rgb);
        }

        protected override CreateParams CreateParams
		{
			get
			{
				var cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
				return cp;
			}
		}
		
		public Bitmap GradientHue(int width, int height, bool vertical = true)
		{
			Bitmap tmp = new Bitmap(width, height);
			byte[] data = new byte[width * height * 4];
			HSV color = new HSV(0, 1f, 1f);
			for (int i = 0; i < height; i++)
			{
				if (vertical)
				{
					color.H = (int) ((i + 1) / ((float)height)  * 360f);

				}
				for (int j = 0; j < width; j++)
				{
					if (!vertical)
					{
						color.H = (int)((j + 1) / ((float)width) * 360f);
					}
					RGB rgbColor = new RGB(color);
					int pos = i * (width * 4) + j * 4;
					data[pos] = rgbColor.B;
					data[pos + 1] = rgbColor.G;
					data[pos + 2] = rgbColor.R;
					data[pos + 3] = 255;
				}
			}

			BitmapData bd = tmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			Marshal.Copy(data, 0, bd.Scan0, data.Length);
			tmp.UnlockBits(bd);
			return tmp;
		}

		public Bitmap GradientFrom(HSV color, int width, int height)
		{
			Bitmap tmp = new Bitmap(width, height);
			byte[] data = new byte[width * height * 4];

			for(int i = 0; i < height; i++)
			{
				for(int j = 0; j < width; j++)
				{
					color.S = (j + 1) / ((float)width);
					color.V = 1-(i + 1) / ((float)height);
					RGB rgbColor = new RGB(color);
					int pos = i * (width* 4) + j * 4;
					data[pos] = rgbColor.B;
					data[pos + 1] = rgbColor.G;
					data[pos + 2] = rgbColor.R;
					data[pos + 3] = 255;
				}
			}

			BitmapData bd = tmp.LockBits(new System.Drawing.Rectangle(0,0, width, height) , System.Drawing.Imaging.ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			Marshal.Copy(data, 0, bd.Scan0, data.Length);
			tmp.UnlockBits(bd);
			return tmp;
		}

		public Bitmap CircleWhiteBorder(HSV color, int width, int height)
		{
			Bitmap tmp = new Bitmap(width, height);
			byte[] data = new byte[width * height * 4];

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					color.S = (j + 1) / ((float)width);
					color.V = 1 - (i + 1) / ((float)height);
					RGB rgbColor = new RGB(color);
					int pos = i * (width * 4) + j * 4;
					data[pos] = rgbColor.B;
					data[pos + 1] = rgbColor.G;
					data[pos + 2] = rgbColor.R;
					data[pos + 3] = 255;
				}
			}

			BitmapData bd = tmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			Marshal.Copy(data, 0, bd.Scan0, data.Length);
			tmp.UnlockBits(bd);
			return tmp;
		}

		public void ChangeHue(int pos)
		{
			Image i =colorDisplayer.BackgroundImage;
			colorDisplayer.BackgroundImage = 
				GradientFrom(new HSV(pos, 0, 0), colorDisplayer.Width, colorDisplayer.Height);
			ModifyH(pos);
			PickCurrentColor();
			if (i != null) i.Dispose();

		}


		private void hueSelector_Paint(object sender, PaintEventArgs e)
		{

		}

		private void hueSelector_LoadCompleted(object sender, AsyncCompletedEventArgs e)
		{

		}
		private void ColorPicker_Shown(object sender, EventArgs e)
		{
			hueSelector.BackgroundImage = GradientHue(hueSelector.Width-hueDot.Width/2, hueSelector.Height-12, false);

			maxRightHueDot = hueSelector.Width - hueDot.Width;
			colorDotBoundsLT = new Point(-colorDot.Width / 2, -colorDot.Height / 2);
			colorDotBoundsRB = new Point(colorDotBoundsLT.X + colorDisplayer.Width, colorDotBoundsLT.Y + colorDisplayer.Height);
			colorDisplayer.Cursor = pickCursor;

			ModifySV(currentShown.S, currentShown.V);
			ChangeHue(currentShown.H);
		}

		private void ColorPicker_Load(object sender, EventArgs e)
		{
		}

		bool hueDotDragged = false;
		private void hueDot_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				hueDotDragged = true;
				MoveHueDot(e);

            }
		}
		int maxRightHueDot;
		private void MoveHueDot(MouseEventArgs e)
		{
			int curX = hueSelector.PointToClient(Cursor.Position).X;
			hueDot.Location = new Point(
				(curX > maxRightHueDot) ?
					maxRightHueDot 
					:
					(curX < hueDot.Width / 2) ?
						0
						: 
						curX - hueDot.Width/2
				, hueDot.Location.Y);
			ChangeHue((int)(hueDot.Location.X / (float)maxRightHueDot * 360f));

            //MessageBox.Show($"{hueSelector.PointToClient(Cursor.Position)} ");
        }

        private void hueSelector_MouseMove(object sender, MouseEventArgs e)
		{
			if(hueDotDragged)
			{
				MoveHueDot(e);
			}
		}

		private void hueDot_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				hueDotDragged = false;
			}
		}

		private void hueSelector_MouseUp(object sender, MouseEventArgs e)
		{
			//if(e.Button == MouseButtons.Left)
			//{
			//	hueDotDragged = false;
			//}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
		bool colorDotDragged = false;
		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			
		}
		Point colorDotBoundsLT;
		Point colorDotBoundsRB;
		private void MoveColorDot(MouseEventArgs e)
		{
			Point curp = colorDisplayer.PointToClient(Cursor.Position);
			colorDot.Location = new Point(
				(curp.X > colorDotBoundsRB.X) ? // X
					colorDotBoundsRB.X 
					:
					(curp.X < 0) ?
						colorDotBoundsLT.X
						: 
						curp.X - colorDot.Width/2,
			(curp.Y > colorDotBoundsRB.Y) ? // Y 
					colorDotBoundsRB.Y 
					:
					(curp.Y < 0) ?
						colorDotBoundsLT.Y
						: 
						curp.Y - colorDot.Height / 2);

			ModifySV();
			PickCurrentColor();
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
		
		}
		Cursor pickCursor = new Cursor(Properties.Resources.PickerDotCursor.Handle);
		private void colorDisplayer_Enter(object sender, EventArgs e)
		{
		}

		private void colorDisplayer_Leave(object sender, EventArgs e)
		{
		}

		private void colorDisplayer_MouseEnter(object sender, EventArgs e)
		{
			
		}

		private void colorDisplayer_MouseLeave(object sender, EventArgs e)
		{
		}

		private void colorDisplayer_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				colorDotDragged = true;
			}
		}

		private void colorDisplayer_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				colorDotDragged = false;
				MoveColorDot(e);
			}
		}

		private void colorDisplayer_MouseMove(object sender, MouseEventArgs e)
		{
			if (colorDotDragged)
			{
				MoveColorDot(e);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			currentColorPicked = colorPickedDisplayer.BackColor;
			DialogResult = DialogResult.OK;

		//	MessageBox.Show($"Color {currentShown.H},{currentShown.S},{currentShown.V}\nPos: {colorDot.Location.ToString()}");
			Close();
		}

		private void ColorPicker_VisibleChanged(object sender, EventArgs e)
		{
			if(Visible)
			{
				ModifySV(currentShown.S, currentShown.V);
				ChangeHue(currentShown.H);
			//	MessageBox.Show($"Current: {currentShown.H}, {currentShown.S}, {currentShown.V}");

			}
		}
	}
}
