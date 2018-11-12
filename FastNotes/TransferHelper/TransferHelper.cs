using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferHelper
{
    public class TransferHelper
    {
        #region Private Members
        /// <summary>
        /// Bytes on the previous iteration
        /// </summary>
        long prevbytes;
        /// <summary>
        /// Bytes transfered until now
        /// </summary>
        long bytesnow;
        /// <summary>
        /// Total bytes to transfer
        /// </summary>
        long totalbytes;
        /// <summary>
        /// Time elapsed since this started
        /// </summary>
        long msElapsed;
        /// <summary>
        /// ms per iteration
        /// </summary>
        long msFrecuency;

        int _Memory;

        /// <summary>
        /// Has the average of the memorylist multiplied by the number of elements in there
        /// </summary>
        long lastAvgSum;

        LinkedList<long> memoryList = new LinkedList<long>();
        #endregion

        #region Public Members

        /// <summary>
        /// Takes the last Memory elements and finds the average to tell the speed
        /// </summary>
        public int Memory { get { return _Memory; } set { _Memory = value > 0 ? value : 1; } }

        #endregion

        #region Constructor
        public TransferHelper(long prevbytes, long bytesnow, long totalbytes, long msElapsed, long msFrecuency)
        {
            this.totalbytes = totalbytes;
            this.msElapsed = msElapsed;
            this.msFrecuency = msFrecuency;
            Memory =(int) (5000f / msFrecuency); // An average of 5 seconds to tell the speed
            Update(prevbytes, bytesnow);
        }
        #endregion


        public void Update(long prevbytes, long bytesnow)
        {
            this.prevbytes = prevbytes;
            this.bytesnow = bytesnow;
        }

        /// <summary>
        /// Writes the information of the current state of this transfer
        /// </summary>
        /// <param name="format">
        /// {BN}: Inserts the bytes now in GB, MB, KB, B, depending on the best fit of the total bytes
        /// {TB}: Insertes the total bytes in GB, MB, KB, B, The best fit possible
        /// {WU}: Unit of the totalbytes and bytesnow (only GB, MB, KB and B)
        /// {S}: Speed of the transfer
        /// {SU}: Unit of the speed (Only Gb/s, Mb/s, Kb/s and B/s)
        /// {TL}: Time left to get the total bytes
        /// {TU}: Time unit of the transfer (Only hours, minutes and seconds)
        /// </param>
        /// <returns></returns>
        public string ToString(string format)
        {
            // Current difference of bytes transfered
            long currentDif = (1000 * (bytesnow - prevbytes) / msFrecuency);
            // Sum of the bytes deleted from memory
            long differenceOnTheAvgNum = 0;

            while(memoryList.Count >= Memory) // Sum and delete the bytes from memory if it's full
            {
                differenceOnTheAvgNum += memoryList.First.Value;
                memoryList.RemoveFirst();
            }
            // Add the current bytes transfered
            memoryList.AddLast(currentDif);

            // Numerator of the average of bytes transfered according to the memory
            lastAvgSum = lastAvgSum - differenceOnTheAvgNum + currentDif;

            // Get the average
            long dif = lastAvgSum / memoryList.Count;

            long divisor = 1L;
            string speedUnit = "b/s";
            if (dif > 999L) // Then it is Kb/s
            {
                if (dif > 999999L) // Then it is Mb/s
                {
                    if (dif > 999999999L) // Then it is Gb/s
                    {
                        speedUnit = "Gb/s";

                        divisor = 1000000000L;
                    }
                    else
                    {
                        speedUnit = "Mb/s";

                        divisor = 1000000L;
                    }
                }
                else
                {
                    speedUnit = "Kb/s";
                    divisor = 1000L;
                }
            }
            long totalBytesDivisor = 1L;
            string totalUnit = "B";
            if (totalbytes > 999L) // Then it is Kbytes
            {
                if (totalbytes > 999999L) // Then it is Mbytes
                {
                    if (totalbytes > 999999999L) // Then it is Gbytes
                    {
                        totalBytesDivisor = 1000000000L;
                        totalUnit = "GB";
                    }
                    else
                    {
                        totalBytesDivisor = 1000000L;
                        totalUnit = "MB";
                    }
                }
                else
                {
                    totalUnit = "KB";
                    totalBytesDivisor = 1000L;
                }
            }

            long remaningBytes = totalbytes - bytesnow;
            long iterationsLeft = remaningBytes / ((dif != 0) ? dif : 1L);
            long timeLeft = iterationsLeft * msFrecuency;
            long timeLeftDivisor = 1L;
            string timeLeftUnit = (timeLeft == 1L) ? "milisecond" : "miliseconds";
            if (timeLeft > 999L) // It'd be seconds
            {
                if (timeLeft > 59999L) // It'd be minutes
                {
                    if (timeLeft > 60L * 60000L - 1L) // It'd be hours
                    {
                        timeLeftUnit = (timeLeft < 2 * 60L * 60000L - 1L) ? "hour" : "hours";
                        timeLeftDivisor = 60L * 60000L;
                    }
                    else
                    {
                        timeLeftUnit = (timeLeft < 119999L) ? "minute" : "minutes";
                        timeLeftDivisor = 60000L;
                    }
                }
                else
                {
                    timeLeftUnit = (timeLeft < 2000L) ? "second" : "seconds";
                    timeLeftDivisor = 1000L;
                }
            }
            long timeLeftReduced = timeLeft / timeLeftDivisor;

            string speed = $"{((double)dif / divisor).ToString("0.0")} {speedUnit}";

            string result = format.Replace("{BN}", ((double)bytesnow / totalBytesDivisor).ToString("0.00"));
            result = result.Replace("{TB}", ((double)totalbytes / totalBytesDivisor).ToString("0.00"));
            result = result.Replace("{WU}", totalUnit);
            result = result.Replace("{S}", ((double)dif / divisor).ToString("0.0"));
            result = result.Replace("{SU}", speedUnit);
            result = result.Replace("{TL}", ((speed == "0.0") ? "Undefined" : timeLeftReduced.ToString()));
            result = result.Replace("{TU}", timeLeftUnit);
            return result;
        }
    }
}
