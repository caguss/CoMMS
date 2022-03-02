using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CoMMS
{
    public static class CodeService
    {
        public static DataTable SensorCodeTable;

        /// <summary>
        /// 센서이름반환
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string SensorNameReturn(string code)
        {
            DataRow[] result = SensorCodeTable.Select($"resource_code = '{code}'");

            if (result.Length < 1)
            {
                return "";
            }
            else
            {
                return result[0][2].ToString();
            }
        }


        public static string IOTCodeReturn(string code)
        {
            DataRow[] result = SensorCodeTable.Select($"resource_code = '{code}'");

            if (result.Length < 1)
            {
                return "";
            }
            else
            {
                return result[0][6].ToString();
            }
        }
    }
}
