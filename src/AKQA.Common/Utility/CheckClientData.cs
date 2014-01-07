using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKQA.Common.Utility
{
    public class CheckClientData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientDatas"></param>
        public static void CheckClientDataIsSafe(string[] clientDatas)
        {
            foreach (string clientData in clientDatas)
            {
                CheckClientDataIsSafe(clientData);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientData"></param>
        public static void CheckClientDataIsSafe(string clientData)
        {
            if (clientData == null) return;
            if (clientData == string.Empty) return;
            if (clientData.IndexOf("'") >= 0)
            {
                throw new ArgumentException("Client data is not standard");
            }

        }
    }
}
