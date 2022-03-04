using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoMMS
{
    public class Provider
    {
        static string strconn = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", Application.Current.Properties["DB_IP"].ToString(), Application.Current.Properties["DB_PORT"].ToString(), Application.Current.Properties["DB_NAME"].ToString(), Application.Current.Properties["DB_ID"].ToString(), Application.Current.Properties["DB_PW"].ToString());
       
        #region MainPage
        public DataTable EquipList_R10()
        {
            using (MySqlConnection conn = new MySqlConnection(strconn))
            {
                conn.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand
                    {
                        CommandText = "USP_MobileEquipList_R10",
                        CommandType = CommandType.StoredProcedure,
                        Connection = conn
                    };
                    ResourceManager rm = new ResourceManager("CoMMS.Resource1", Assembly.GetExecutingAssembly());
                    string mstcode = "";
                    if (Application.Current.Properties.ContainsKey("Resorce_Mst_Code"))
                        mstcode = Application.Current.Properties["Resorce_Mst_Code"].ToString();
                    else
                        mstcode = rm.GetString("Resorce_Mst_Code");
         

                    cmd.Parameters.Add(new MySqlParameter("@v_MstCode", MySqlDbType.VarChar, 50));
                    cmd.Parameters["@v_MstCode"].Value = mstcode;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    da.Dispose();
                    conn.Close();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                    else
                    {
                        return new DataTable();
                    }
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
            }
        }
        #endregion
        #region Service
        public DataSet LoadCode()
        {
            using (MySqlConnection conn = new MySqlConnection(strconn))
            {
                conn.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand
                    {
                        CommandText = "USP_MobileLoadCode_R10",
                        CommandType = CommandType.StoredProcedure,
                        Connection = conn
                    };
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    da.Dispose();
                    conn.Close();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }
                    else
                    {
                        return new DataSet();
                    }
                }
                catch (Exception)
                {
                    return new DataSet();
                }
            }
        }
        #endregion
    }
}
