using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class TableUtil
    {
        public string GetString(string strval)
        {
            string retStr = string.Empty;

            if (strval != null)
            {
                retStr = strval;
            }
            return retStr;
        }

        public int GetInt(int intval)
        {
            int retInt = 0;

            if (intval != 0)
            {
                retInt = intval;
            }
            return retInt;
        }

    }
}
