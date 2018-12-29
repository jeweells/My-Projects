using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastNotes
{
    public class DataLoader
    {
        /// <summary>
        /// Get the data from any version of data and transforms it to the newest version
        /// </summary>
        /// <param name="deserializedObject"></param>
        /// <returns></returns>
        public static List<object> GetNewestData(object deserializedObject)
        {
            var nd = new List<object>();


            Type objectType = deserializedObject.GetType();
            if (objectType.Equals(typeof(NoteData_1_0_1_4[]))) // Version 1.0.1.4
            {
                foreach (NoteData_1_0_1_4 note in deserializedObject as NoteData_1_0_1_4[])
                {
                    nd.Add(note);
                }
            }
            else if (objectType.Equals(typeof(NoteData[]))) // Version 1.0.1.3
            {
                foreach (NoteData note in deserializedObject as NoteData[])
                {
                    nd.Add(new NoteData_1_0_1_4(note));
                }
            }
            else
            {
                throw new ArgumentException($"This data conversion (from {objectType.ToString()}) is not implemented yet.");
            }
            return nd;
        }
    }

}
