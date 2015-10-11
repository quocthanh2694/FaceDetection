using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Windows.Forms;

namespace DXApplication1
{
    class ketnoi
    {
        public static string conn = DXApplication1.Properties.Settings.Default.conect;
        public SqlConnection con;
        public SqlCommand cm;
        public SqlDataAdapter da;
        public SqlDataReader reader;

        public void connect()
        {
            try
            {
                con = new SqlConnection(conn);
                //con = new SqlConnection(@"Data Source=WIN-GPVUMISDG71\SA;Initial Catalog=DanhSachSinhVien;User ID=sa;Password=123");
                con.Open();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        public void connectClose()
        {
            con.Close();
        }

        public DataTable laydl(string sql,List<SqlParameter> par=null)
        {
            connect();
            cm = new SqlCommand(sql, con);
            if (par != null)
            {
                foreach (SqlParameter sp in par)
                    cm.Parameters.Add(sp);
            }
            cm.ExecuteScalar();
            da = new SqlDataAdapter();
            da.SelectCommand = cm;
            DataTable dt = new DataTable();
            da.Fill(dt);
            da.Dispose();
            connectClose();
            return dt;
        }
        public string lay1dong(string sql)
        {
            connect();
            cm = new SqlCommand(sql, con);


            reader = cm.ExecuteReader();
            string v = reader.Read() ? reader[0].ToString() : "";
            reader.Dispose();


            connectClose();
            return v;

        }
        #region son
        public static DataTable readDB(string sql, CommandType cmdType = CommandType.Text, List<SqlParameter> listParams = null)
        {
            DataTable result = new DataTable();
            #region
            if (cmdType == CommandType.StoredProcedure)
            {
                SqlConnection connn = new SqlConnection(conn);

                if (connn.State == ConnectionState.Closed)
                {
                    connn.Open();

                }

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = cmdType;
                cmd.Connection = connn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                if (listParams != null)
                {
                    foreach (SqlParameter pram in listParams)
                    {
                        cmd.Parameters.Add(pram);
                    }
                }
                SqlDataReader dr = cmd.ExecuteReader();
                result.Load(dr);
                return result;
            }
            #endregion

            else
            {
                
                if (!sql.ToUpper().Contains("SELECT"))
                {
                    sql = "SELECT * FROM " + sql;

                }
                try
                {
                    SqlConnection connn = new SqlConnection(conn);

                    if (connn.State == ConnectionState.Closed)
                    {
                        connn.Open();

                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = cmdType;
                    cmd.Connection = connn;
                    cmd.CommandText = sql;
                    if (listParams != null)
                        foreach (SqlParameter sp in listParams)
                            cmd.Parameters.Add(sp);
                  //  cm = new SqlCommand(sql, c);
                   // if (listParams != null)
                    //{
                    //    foreach (SqlParameter sp in par)
                    //        cm.Parameters.Add(sp);
                    //}
                    //cm.ExecuteScalar();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.FillSchema(result, SchemaType.Source);
                    da.Fill(result);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Loi : " + e.ToString());

                }
                return result;
            }

        }
        public static DataTable readStrucreDT(string tableName)
        {
            DataTable result = new DataTable();
            string sql = "SELECT * FROM " + tableName;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.FillSchema(result, SchemaType.Source);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối database vui lòng kiểm tra lại");
            }
            return result;
        }
        public static int write(DataRow dr)
        {
            int result = 0;
            string sql = "SELECT * FROM " + dr.Table.TableName;
            DataTable dt = dr.Table;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                SqlCommandBuilder cmd = new SqlCommandBuilder(da);
                result = da.Update(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi ket noi " + ex.Message);
            }
            return result;
        }
        public static int excuteNonQuery(string sql, List<SqlParameter> listParams = null)
        {
            int count = 0;
            using (SqlConnection connn = new SqlConnection(conn))
            {
                if (connn.State == ConnectionState.Closed)
                {
                    connn.Open();

                }
                SqlCommand cmd = new SqlCommand(sql, connn);
                if (listParams != null)
                {
                    foreach (SqlParameter pram in listParams)
                    {
                        cmd.Parameters.Add(pram);
                    }
                }

                try
                {
                    count = cmd.ExecuteNonQuery();
                    //if (sql.ToUpper().IndexOf("INSERT INTO") == 0)
                    //{
                    //    sql = "Select @@Identity";
                    //    cmd.CommandText = sql;
                    //    count = int.Parse(cmd.ExecuteScalar().ToString());
                    //}
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra :" + ex.Message);
                }
                return count;

            }
        #endregion

        }
    }
}