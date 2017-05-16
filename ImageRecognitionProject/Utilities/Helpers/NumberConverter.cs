using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helpers
{
    public class NumberConverter
    {
        public static int LbpArrayToDecimal(int[,] array)
        {
            string binary = String.Empty;
            binary += array[0, 0];
            binary += array[0, 1];
            binary += array[0, 2];
            binary += array[1, 2];
            binary += array[2, 2];
            binary += array[2, 1];
            binary += array[2, 0];
            binary += array[1, 0];
            return Convert.ToInt32(binary, 2);
        }
    }
}
