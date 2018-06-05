using System;
namespace Tools.WebChecker
{
    public static class WebChecker
    {
		public static System.Net.HttpWebResponse Try(string url, int timeout = 60000)
        {
            Uri ourUri = new Uri(url);

			// Create a 'WebRequest' object with the specified url. 
			System.Net.WebRequest myWebRequest = System.Net.WebRequest.Create(url);
			myWebRequest.Timeout = timeout;
			//myWebRequest.Timeout = 5000;
			System.Net.WebResponse myWebResponse;
            // Send the 'WebRequest' and wait for response.
            try
            {
                myWebResponse = myWebRequest.GetResponse();
            }
            catch
            {
                return null;
            }
            myWebResponse.Close();
            return ((System.Net.HttpWebResponse)myWebResponse);
        }
    }
}
