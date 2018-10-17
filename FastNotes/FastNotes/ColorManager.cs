using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorManager
{
	public class HSV
	{
		int h;
		float s, v;
		public int H { get => h; set => h = (360 + value % 360) % 360; }
		public float S { get => s; set => s = (value > 1) ? 1 : (value < 0) ? 0 : value; }
		public float V { get => v; set => v = (value > 1) ? 1 : (value < 0) ? 0 : value; }

		public HSV(int h, float s, float v)
		{
			H = h; S = s; V = v;
		}
		public HSV(RGB color)
		{
			float Rp = color.R / 255f;
			float Gp = color.G / 255f;
			float Bp = color.B / 255f;
			float cmax = Math.Max(Rp, Math.Max(Gp, Bp));
			float cmin = Math.Min(Rp, Math.Min(Gp, Bp));
			float det = (cmax - cmin);
			H = (int)
				((det == 0) ? 0 :
				(cmax == Rp) ? 60 * (((Gp - Bp) / det)) :
				(cmax == Gp) ? 60 * (((Bp - Rp) / det) + 2) :
									60 * (((Rp - Gp) / det) + 4));
			s = (cmax == 0) ? 0 : det / cmax;
			v = cmax;
		}

	}
	public class RGB
	{
		public byte R, G, B;
		public RGB(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}
		public RGB(HSV color)
		{
			float C = color.V * color.S;
			float X = C * (1 - Math.Abs((color.H / 60f) % 2 - 1));
			float m = color.V - C;
			float Rp = 0, Gp = 0, Bp = 0;
			if (0 <= color.H && color.H < 60)
			{
				Rp = C; Gp = X; Bp = 0;
			}
			else if (60 <= color.H && color.H < 120)
			{
				Rp = X; Gp = C; Bp = 0;
			}
			else if (120 <= color.H && color.H < 180)
			{
				Rp = 0; Gp = C; Bp = X;
			}
			else if (180 <= color.H && color.H < 240)
			{
				Rp = 0; Gp = X; Bp = C;
			}
			else if (240 <= color.H && color.H < 300)
			{
				Rp = X; Gp = 0; Bp = C;
			}
			else if (300 <= color.H && color.H < 360)
			{
				Rp = C; Gp = 0; Bp = X;
			}
			R = (byte)((Rp + m) * 255f);
			G = (byte)((Gp + m) * 255f);
			B = (byte)((Bp + m) * 255f);
		}
	}
}
