using ToolExportVideo.Library;
using ToolExportVideo.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolExportVideo.Common
{
    public static class Commonfunc
    {
        public static T ConvertToType<T>(object result)
        {
            if (result == null || result == DBNull.Value)
            {
                return default(T);
            }
            return (T)Convert.ChangeType(result, typeof(T));
        }
        public static T CastToSpecificType<T>(object obj) where T : class
        {
            if (obj is T castedObj)
            {
                return castedObj;
            }
            else
            {
                return null;
            }
        }
        public static int FindPowerOfThreeRange(int n)
        {
            int k = 0;
            while (true)
            {
                int lowerBound = (int)Math.Pow(3, k);
                int upperBound = (int)Math.Pow(3, k + 1);

                if (n >= lowerBound && n < upperBound)
                {
                    return k + 1; // Trả về k+1 vì khoảng là từ 3^k đến 3^(k+1)
                }

                k++;

                // Điều kiện dừng để tránh vòng lặp vô hạn
                if (upperBound > n)
                {
                    return -1;
                }
            }
        }
    }
}
