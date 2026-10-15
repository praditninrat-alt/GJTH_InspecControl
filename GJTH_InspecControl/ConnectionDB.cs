using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace GJTH_ISPC
{
    public class ConnectionDB
    {
        SqlConnection objConn = new SqlConnection();
        public static String strModule = "";
        public DataSet  QueryToDataSet(string strSql)
        {
            objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AXDB"].ToString());
            DataSet ds = new DataSet();
            //SqlDataAdapter adapter = new SqlDataAdapter(strSql, objConn);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand objCmd = new SqlCommand(strSql, objConn);
            adapter.SelectCommand = objCmd;
            objCmd.CommandTimeout = 200;
            adapter.Fill(ds, "Datatable1");
            objConn.Close();
            adapter.Dispose();
            return ds;
        }

        public DataTable QueryToDataTable(string strSql)
        {
            

            objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AXDB"].ToString());
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand objCmd = new SqlCommand(strSql, objConn);
            adapter.SelectCommand = objCmd;
            objCmd.CommandTimeout = 200;
            adapter.Fill(ds, "Datatable1");
            objConn.Close();
            adapter.Dispose();
            return ds.Tables[0];
        }

        public void ExecuteDB(string strSql)
        {
            SqlCommand objCmd;
            objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AXDB"].ToString());
            objCmd = new SqlCommand(strSql, objConn);
            objConn.Open();
            objCmd.ExecuteNonQuery();
            objConn.Close();
            objCmd.Dispose();
            objCmd = null;
        }

        public DataSet QueryToDataSetExt(string strSql)
        {
            objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["EXTDB"].ToString());
            DataSet ds = new DataSet();
            //SqlDataAdapter adapter = new SqlDataAdapter(strSql, objConn);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand objCmd = new SqlCommand(strSql, objConn);
            adapter.SelectCommand = objCmd;
            objCmd.CommandTimeout = 200;
            adapter.Fill(ds, "Datatable1");
            objConn.Close();
            adapter.Dispose();
            return ds;
        }

        public DataTable QueryToDataTableExt(string strSql)
        {
            objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["EXTDB"].ToString());
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand objCmd = new SqlCommand(strSql, objConn);
            adapter.SelectCommand = objCmd;
            objCmd.CommandTimeout = 200;
            adapter.Fill(ds, "Datatable1");
            objConn.Close();
            adapter.Dispose();
            return ds.Tables[0];
        }

        public void ExecuteDBExt(string strSql)
        {
            SqlCommand objCmd;
            objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["EXTDB"].ToString());
            objCmd = new SqlCommand(strSql, objConn);
            objConn.Open();
            objCmd.ExecuteNonQuery();
            objConn.Close();
            objCmd.Dispose();
            objCmd = null;
        }

        public double ConvertByCurrency(string strCurr, double dPrice)
        {
            double dPriceResult = 0;

            DataTable dtConvert = QueryToDataTable("Select * from CurConvers Where CurrencyCode='" + strCurr + "'");

            DataTable dtCurrTHB = QueryToDataTable("Select * from CurConvers Where CurrencyCode='THB'");

            int iDigit = Convert.ToInt32(dtConvert.Rows[0]["Decimal"]);

            double dCal = (dPrice / Convert.ToDouble(dtCurrTHB.Rows[0]["Conversion"])) * Convert.ToDouble(dtConvert.Rows[0]["Conversion"]);

            if (iDigit >= 0)
            {
                dPriceResult = Math.Round(dCal + 0.00000001, iDigit);

                ////-----Round UP
                //if (iDigit == 0)
                //{
                //    dPriceResult = Math.Ceiling(dCal + 0.00000001);
                //}
                //else
                //{
                //    double multiplier = Math.Pow(10, iDigit - 1);
                //    dPriceResult = Math.Ceiling(dPriceResult * multiplier) / multiplier;
                //}
            }
            else 
            {
                double n = Math.Pow(10, -iDigit);
                dPriceResult = Math.Round(dCal / n, 0) * n;

                ////-----Round UP
                //dPriceResult = Math.Ceiling(dCal / n) * n;
            }

            return dPriceResult;

            

        }

        public double ConvertCurrencyPrevious(string strCurr, double dPrice, double dPreConvertValue)
        {
            double dPriceResult = 0;

            DataTable dtConvert = QueryToDataTable("Select * from CurConvers Where CurrencyCode='" + strCurr + "'");

            DataTable dtCurrTHB = QueryToDataTable("Select * from CurConvers Where CurrencyCode='THB'");

            int iDigit = Convert.ToInt32(dtConvert.Rows[0]["Decimal"]);

            double dCal = (dPrice / Convert.ToDouble(dtCurrTHB.Rows[0]["Conversion"])) * dPreConvertValue;

            if (iDigit >= 0)
            {
                dPriceResult = Math.Round(dCal + 0.00000001, iDigit);

                ////-----Round UP
                //if (iDigit == 0)
                //{
                //    dPriceResult = Math.Ceiling(dCal + 0.00000001);
                //}
                //else
                //{
                //    double multiplier = Math.Pow(10, iDigit - 1);
                //    dPriceResult = Math.Ceiling(dPriceResult * multiplier) / multiplier;
                //}
            }
            else
            {
                double n = Math.Pow(10, -iDigit);
                dPriceResult = Math.Round(dCal / n, 0) * n;

                ////-----Round UP
                //dPriceResult = Math.Ceiling(dCal / n) * n;
            }

            return dPriceResult;



        }

        public string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;

            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public string EncryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        public string EncryptByKey(string strEncryp)
        {
            string EncryptionKey = "GJTH2022";
            byte[] clearBytes = Encoding.Unicode.GetBytes(strEncryp);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x61, 0x76, 0x49, 0x6e, 0x20, 0x65, 0x4d, 0x64, 0x76, 0x64, 0x65, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    strEncryp = Convert.ToBase64String(ms.ToArray());
                }
            }
            return strEncryp;
        }

        public string DecryptByKey(string strDecryp)
        {
            string EncryptionKey = "GJTH2022";

            strDecryp = strDecryp.Replace(" ", "+");
            int mod4 = strDecryp.Length % 4;
            if (mod4 > 0)
            {
                strDecryp += new string('=', 4 - mod4);
            }

            byte[] cipherBytes = Convert.FromBase64String(strDecryp);
            using (Aes encryptor = Aes.Create())
            {
                //Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x61, 0x76, 0x49, 0x6e, 0x20, 0x65, 0x4d, 0x64, 0x76, 0x64, 0x65, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    strDecryp = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return strDecryp;
        }

        public string EncryptMD5(string text)
        {
           
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public string EncryptMD5ByKey(string text)
        {
            string strResult = "";
            byte[] data = UTF8Encoding.UTF8.GetBytes(text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("REPAIRGJTH2020"));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    strResult = Convert.ToBase64String(results, 0, results.Length);
                }
            }

            return strResult;
        }

        public string DecryptMD5ByKey(string text)
        {
            string strResult = "";
            byte[] data = Convert.FromBase64String(text); // decrypt the incrypted text
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("REPAIRGJTH2020"));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    strResult = UTF8Encoding.UTF8.GetString(results);
                }
            }
            return strResult;
        }

        public string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public string GenID()
        {
            string strLength = "00000000000000000000";
            int iRunning;
            string strRunId = "";
            DataSet dsGetRunning = QueryToDataSetExt("Select * from NumberSequence where Module = 'CMS'");

            if (Convert.ToInt32(dsGetRunning.Tables[0].Rows[0]["Length"]) > strLength.Length)
            {
                return strRunId;
            }

            if (DateTime.Now.Year == Convert.ToDateTime(dsGetRunning.Tables[0].Rows[0]["CurrentDate"]).Year)
            {
                iRunning = Convert.ToInt32(dsGetRunning.Tables[0].Rows[0]["CurrentRunning"]) + 1;
            }
            else
            {
                iRunning = 1;
            }



            if (iRunning.ToString().Length < Convert.ToInt32(dsGetRunning.Tables[0].Rows[0]["Length"]))
            {
                strRunId = dsGetRunning.Tables[0].Rows[0]["Prefix"] + DateTime.Now.ToString("yy") + "-" + strLength.Substring(1, Convert.ToInt32(dsGetRunning.Tables[0].Rows[0]["Length"]) - iRunning.ToString().Length) + iRunning.ToString();
            }
            else
            {
                strRunId = dsGetRunning.Tables[0].Rows[0]["Prefix"] + DateTime.Now.ToString("yy") + "-" + iRunning.ToString();
            }

            ExecuteDBExt("Update NumberSequence Set CurrentRunning=" + iRunning + ",CurrentDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Module = 'CMS'");

            return strRunId;
        }

        public string GenWaxLot()
        {
            string strLength = "00000000000000000000";
            int iRunning;
            string strRunId = "";
            DataSet dsGetRunning = QueryToDataSetExt("Select * from NumberSequence where Module = 'WAX'");

            if (Convert.ToInt32(dsGetRunning.Tables[0].Rows[0]["Length"]) > strLength.Length)
            {
                return strRunId;
            }

            if (DateTime.Now.Year == Convert.ToDateTime(dsGetRunning.Tables[0].Rows[0]["CurrentDate"]).Year)
            {
                iRunning = Convert.ToInt32(dsGetRunning.Tables[0].Rows[0]["CurrentRunning"]) + 1;
            }
            else
            {
                iRunning = 1;
            }



            if (iRunning.ToString().Length < Convert.ToInt32(dsGetRunning.Tables[0].Rows[0]["Length"]))
            {
                strRunId = dsGetRunning.Tables[0].Rows[0]["Prefix"] + DateTime.Now.ToString("yy") + "-" + strLength.Substring(1, Convert.ToInt32(dsGetRunning.Tables[0].Rows[0]["Length"]) - iRunning.ToString().Length) + iRunning.ToString();
            }
            else
            {
                strRunId = dsGetRunning.Tables[0].Rows[0]["Prefix"] + DateTime.Now.ToString("yy") + "-" + iRunning.ToString();
            }

            ExecuteDBExt("Update NumberSequence Set CurrentRunning=" + iRunning + ",CurrentDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Module = 'WAX'");

            return strRunId;
        }

        public string GenLotID(string strMatNum)
        {
            int iLength = 3;
            string strPrefix = "LOT";
            string strLength = "00000000000000000000";
            int iRunning;
            string strRunId = "";
            string strCurrentSeq = strPrefix + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") ;
            //DataSet dsGetRunning = QueryToDataSetExt("Select * from NumberSequence where Module = 'WT'");

            if (iLength > strLength.Length)
            {
                return strRunId;
            }

            DataTable dtGetSeq = QueryToDataTableExt("Select LotID from PLT_PlatingList Where LotID like '" + strCurrentSeq + "%' ");

            if (dtGetSeq.Rows.Count > 0)
            {
                iRunning = dtGetSeq.Rows.Count + 1;
            }
            else
            {
                iRunning = 1;
            }


            
            if (iRunning.ToString().Length < iLength)
            {
                
                strRunId = strPrefix + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")  +  "-" + strLength.Substring(1, iLength - iRunning.ToString().Length) + iRunning.ToString();
            }
            else
            {
                strRunId = strPrefix + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")  +  "-" + iRunning.ToString();
            }

            //ExecuteDBExt("Update NumberSequence Set CurrentRunning=" + iRunning + ",CurrentDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Module = 'WT'");

            return strRunId;
        }

        public string GenCastLotByMat(string strMatType,DateTime dCastDate)
        {
            int iLength = 2;
            string strPrefix = "";
            string strLength = "00000000000000000000";
            int iRunning;
            string strRunId = "";
            string strCurrentSeq = strPrefix + dCastDate.ToString("yyyy") + dCastDate.ToString("MM") + dCastDate.ToString("dd") + strMatType;
            //DataSet dsGetRunning = QueryToDataSetExt("Select * from NumberSequence where Module = 'WT'");

            if (iLength > strLength.Length)
            {
                return strRunId;
            }

            DataTable dtGetSeq = QueryToDataTableExt("Select CastNo from CMS_Casting Where CastNo like '" + strCurrentSeq + "%'");

            if (dtGetSeq.Rows.Count > 0)
            {
                iRunning = dtGetSeq.Rows.Count + 1;
            }
            else
            {
                iRunning = 1;
            }



            if (iRunning.ToString().Length < iLength)
            {

                strRunId = strPrefix + dCastDate.ToString("yyyy") + dCastDate.ToString("MM") + dCastDate.ToString("dd") + strMatType + "-" + strLength.Substring(1, iLength - iRunning.ToString().Length) + iRunning.ToString();
            }
            else
            {
                strRunId = strPrefix + dCastDate.ToString("yyyy") + dCastDate.ToString("MM") + dCastDate.ToString("dd") + strMatType + "-"+ iRunning.ToString();
            }


            return strRunId;
        }

        public enum MatType
        {
            YG = 1,
            WG = 2,
            RG = 3,
            SL = 4,
            PT = 5
          
        }

        public enum PermissionRole
        {
            Admin = 0,
            Manager = 1,
            User = 2

        }

       
     
    }
}