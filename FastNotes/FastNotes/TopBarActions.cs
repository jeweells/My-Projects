using System;
using System.Windows.Forms;

public static class TopBarActions 
{
	const int WM_NCLBUTTONDOWN = 0xA1;
	const int HT_CAPTION = 0x2;

	[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
	[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
	public static extern bool ReleaseCapture();
	public static void MouseDown(IntPtr handle, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			ReleaseCapture();
			SendMessage(handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
		}
	}
}
