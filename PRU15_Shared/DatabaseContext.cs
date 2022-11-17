using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace PRU15_Shared
{
    public class DatabaseContext
    {
        #region Singleton pattern
        /// <summary>
        /// Singleton
        /// </summary>
        private static readonly DatabaseContext _instance = new DatabaseContext();
        #endregion
        //private SqlConnection connection;
        private string ConnectionString = "Server=tcp:pru15-backend.database.windows.net,1433;" +
                     "Initial Catalog=pru15-backend;Persist Security Info=False;User ID=admin_pru15;" +
                     "Password=P@ssword01;Encrypt=True;" +
                     "TrustServerCertificate=False;Connection Timeout=30;";

        DatabaseContext()
        {

        }

        public static DatabaseContext Instance
        {
            get { return _instance; }
        }

        public void InsertAllDataCalon(DataCalon dataCalon , out string exception)
        {

            var connection = new SqlConnection(ConnectionString);
            exception = string.Empty;
                    try
                    {
                connection.Open();

                SqlCommand sqlCommand = new SqlCommand($"INSERT INTO Calon (Nama_Calon, Nama_Parlimen, Nama_Parti," +
                             $"Negeri, Kod_Parlimen) VALUES( '{dataCalon.NamaCalon}' , '{dataCalon.NamaParlimen}' , " +
                             $" '{dataCalon.NamaParti}' , '{dataCalon.Negeri}' ,  '{dataCalon.KodParlimen.Trim().Replace(" " , string.Empty)}' )", connection);
                        sqlCommand.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        exception = $"FAILED at - {dataCalon.NamaCalon} - { dataCalon.KodParlimen} : { ex.Message}";

                    }
                    finally {

                connection.Close();

                Console.WriteLine($"Calon {dataCalon.NamaCalon} is SAVED");

                    }


        }



        public void InsertAllDataParlimen(DataParlimen dataCalon, out string exception)
        {

            var connection = new SqlConnection(ConnectionString);
            exception = string.Empty;
            try
            {
                connection.Open();

                SqlCommand sqlCommand = new SqlCommand($"INSERT INTO Parlimen (Parlimen_Kod, Nama_Parlimen, Jumlah_Calon," +
                             $"Negeri) VALUES( '{dataCalon.KodParlimen.Trim().Replace(" ", string.Empty)}' , '{dataCalon.NamaParlimen}' , " +
                             $" '{dataCalon.JumlahCalon}' , '{dataCalon.Negeri}')", connection);
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                exception = $"FAILED at - {dataCalon.NamaParlimen} - { dataCalon.KodParlimen} : { ex.Message}";

            }
            finally
            {

                connection.Close();

                Console.WriteLine($"Calon {dataCalon.NamaParlimen} is SAVED");

            }


        }

        public List<DataCalon> GetDataCalon(string kodParlimen = null)
        {
            List<DataCalon> dataCalon = new List<DataCalon>();
            SqlDataReader reader;
            var connection = new SqlConnection(ConnectionString);

            //SqlTransaction sqlTransaction;
            try
            {

                {
                    SqlCommand sqlCommand;

                    if (string.IsNullOrEmpty(kodParlimen))
                    {
                        sqlCommand = new SqlCommand($"SELECT * FROM Calon", connection);
                    }
  
                    else
                    {
                    sqlCommand = new SqlCommand($"SELECT * FROM Calon WHERE Kod_Parlimen = '{kodParlimen.ToUpper()}'", connection);
                        }
              

                    connection.Open();
                    //sqlTransaction =connection.BeginTransaction();
                    reader = sqlCommand.ExecuteReader();
                    // Read ProductId from each record  
                    while (reader.Read())
                    {
                        string parliment_dun;
                        if (reader["Kod_Parlimen"].ToString().StartsWith("P"))
                        {
                            parliment_dun = "PARLIMEN";
                            dataCalon.Add(new DataCalon
                            {
                                Parlimen_Dun = parliment_dun,
                                NamaCalon = reader["Nama_Calon"].ToString(),
                                NamaParlimen = reader["Nama_Parlimen"].ToString(),
                                NamaParti = reader["Nama_Parti"].ToString(),
                                KodParlimen = reader["Kod_Parlimen"].ToString().Trim(),
                                Negeri = reader["Negeri"].ToString(),
                                Jumlah_Undian = reader["Jumlah_Undian"].ToString()

                            });
                        }
                        //else
                        //{
                        //    parliment_dun = "DUN";
                        //}

                        //dataCalon.Add(new DataCalon
                        //{
                        //    Parlimen_Dun = parliment_dun,
                        //    NamaCalon = reader["Nama_Calon"].ToString(),
                        //    NamaParlimen = reader["Nama_Parlimen"].ToString(),
                        //    NamaParti = reader["Nama_Parti"].ToString(),
                        //    KodParlimen = reader["Kod_Parlimen"].ToString().Trim(),
                        //    Negeri = reader["Negeri"].ToString(),
                        //    Jumlah_Undian = reader["Jumlah_Undian"].ToString()

                        //}); 
                        //Console.WriteLine(reader["PrductId"]);
                    }

                    //Commit current transaction  
                    //sqlTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                //sqlTransaction.Rollback();

                string msg = ex.Message;
            }
            finally
            {
                connection.Close();
                UpdateAPICall("CALON");
            }

            return dataCalon;
        }

        public List<DataParlimen> GetDataParlimen(string negeri = null)
        {
            List<DataParlimen> dataParlimen = new List<DataParlimen>();
            SqlDataReader reader;
            var connection = new SqlConnection(ConnectionString);

            //SqlTransaction sqlTransaction;
            try
            {

                {
                    SqlCommand sqlCommand;
                    if (string.IsNullOrEmpty(negeri))
                    {
                        sqlCommand = new SqlCommand($"SELECT * FROM Parlimen", connection);

                    }
                    else
                    {
                        sqlCommand = new SqlCommand($"SELECT * FROM Parlimen WHERE Negeri = '{negeri.ToUpper()}' ", connection);
                    }

                    connection.Open();
                    //sqlTransaction =connection.BeginTransaction();
                    reader = sqlCommand.ExecuteReader();
                    // Read ProductId from each record  
                    while (reader.Read())
                    {
                        string parliment_dun;
                        if (reader["Parlimen_Kod"].ToString().StartsWith("P"))
                        {
                            parliment_dun = "PARLIMEN";

                            dataParlimen.Add(new DataParlimen
                            {
                                Parlimen_Dun = parliment_dun,
                                NamaParlimen = reader["Nama_Parlimen"].ToString(),
                                KodParlimen = reader["Parlimen_Kod"].ToString(),
                                JumlahCalon = reader["Jumlah_Calon"].ToString(),
                                Negeri = reader["Negeri"].ToString(),


                            });
                        }
                        //else
                        //{
                        //    parliment_dun = "DUN";
                        //}

                        //dataParlimen.Add(new DataParlimen
                        //{
                        //    Parlimen_Dun = parliment_dun,
                        //    NamaParlimen = reader["Nama_Parlimen"].ToString(),
                        //    KodParlimen = reader["Parlimen_Kod"].ToString(),
                        //    JumlahCalon = reader["Jumlah_Calon"].ToString(),
                        //    Negeri = reader["Negeri"].ToString(),


                        //});
                        //Console.WriteLine(reader["PrductId"]);
                    }

                    //Commit current transaction  
                    //sqlTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                //sqlTransaction.Rollback();

                string msg = ex.Message;
            }
            finally
            {
                connection.Close();
                UpdateAPICall("PARLIMEN");
            }

            return dataParlimen;
        }

        public List<KerusiBertanding> GetDataPartiBertanding()
        {
            var dataCalon = GetDataCalon();
            List<KerusiBertanding> kerusiBertandings = new List<KerusiBertanding>();
            var jumlahKerusiPH = new List<DataCalon>();
            var jumlahKerusiPN = new List<DataCalon>();
            var jumlahKerusiBN = new List<DataCalon>();
            var jumlahKerusiPejuang = new List<DataCalon>();
            var jumlahKerusiSabah = new List<DataCalon>();
            var jumlahKerusiSarawak = new List<DataCalon>();


            var jumlahKerusiBebas = new List<DataCalon>();

            foreach (var item in dataCalon)
            {
                if(item.NamaParti.Contains("(PH)")|| item.NamaParti.Contains("(MUDA)") || item.NamaParti.Contains("(DAP)"))
                {
                    jumlahKerusiPH.Add(item);
                }
                else if(item.NamaParti.Contains("(PN)")|| item.NamaParti.Contains("(PAS)") || item.NamaParti.Contains("(PRM)"))
                 {
                    jumlahKerusiPN.Add(item);
                }
                else if (item.NamaParti.Contains("(BN)") || item.NamaParti.Contains("(PBRS)"))
                {
                    jumlahKerusiBN.Add(item);
                }
                else if (item.NamaParti.Contains("(PEJUANG)") || item.NamaParti.Contains("(PUTRA)"))
                {
                    jumlahKerusiPejuang.Add(item);
                }
                else if (item.NamaParti.Contains("(GRS)") || item.NamaParti.Contains("(PBRS)"))
                {
                    jumlahKerusiSabah.Add(item);
                }
                else if (item.NamaParti.Contains("(GPS)") || item.NamaParti.Contains("(PSB)"))
                {
                    jumlahKerusiSarawak.Add(item);
                }
                else if (item.NamaParti.Contains("BEBAS"))
                {
                    jumlahKerusiBebas.Add(item);
                }
                else
                {
                    //DO NOTHING
                }

            }

            kerusiBertandings.Add(
                new KerusiBertanding
                {
                    NamaParti = "PAKATAN HARAPAN (PH)",
                    JumlahKerusi = jumlahKerusiPH.Count
                });

            kerusiBertandings.Add(
    new KerusiBertanding
    {
        NamaParti = "PERIKATAN NASIONAL (PN)",
        JumlahKerusi = jumlahKerusiPN.Count
    });

            kerusiBertandings.Add(
new KerusiBertanding
{
NamaParti = "BARISAN NASIONAL (BN)",
JumlahKerusi = jumlahKerusiBN.Count
});

            kerusiBertandings.Add(
new KerusiBertanding
{
NamaParti = "PARTI PEJUANG TANAHAIR (PEJUANG)",
JumlahKerusi = jumlahKerusiPejuang.Count
});


            kerusiBertandings.Add(
new KerusiBertanding
{
    NamaParti = "PARTI GABUNGAN RAKYAT SABAH (GRS)",
    JumlahKerusi = jumlahKerusiSabah.Count
});

            kerusiBertandings.Add(
new KerusiBertanding
{
NamaParti = "GABUNGAN PARTI SARAWAK (GPS)",
JumlahKerusi = jumlahKerusiSarawak.Count
});


            kerusiBertandings.Add(
new KerusiBertanding
{
NamaParti = "BEBAS",
JumlahKerusi = jumlahKerusiBebas.Count
});

            return kerusiBertandings;
        }

        public void UpdateJumlahVisitor()
        {

            var connection = new SqlConnection(ConnectionString);
            string exception = string.Empty;
            try
            {
                connection.Open();

                SqlCommand sqlCommand = new SqlCommand("UPDATE Visitors set Jumlah_Visitor = Jumlah_Visitor + 1", connection);
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                exception = $"FAILED : { ex.Message}";

            }
            finally
            {
                connection.Close();
            }

        }

        private void UpdateAPICall(string columnName)
        {

            var connection = new SqlConnection(ConnectionString);
            string exception = string.Empty;
            try
            {
                connection.Open();
                var sqlQuery = $"UPDATE API_Data set Total_API_Call = Total_API_Call + 1 WHERE API_Name = '{columnName}'";
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                exception = $"FAILED : { ex.Message}";

            }
            finally
            {
                connection.Close();
            }

        }

        public string GetJumlahVisitor()
        {
            SqlDataReader reader;
            var connection = new SqlConnection(ConnectionString);
            string visitor = string.Empty;
            //SqlTransaction sqlTransaction;
            try
            {

                {
                    SqlCommand sqlCommand = new SqlCommand($"SELECT * FROM Visitors", connection);


                    connection.Open();
                    //sqlTransaction =connection.BeginTransaction();
                    reader = sqlCommand.ExecuteReader();
                    // Read ProductId from each record  
                    while (reader.Read())
                    {

                        visitor = reader["Jumlah_Visitor"].ToString();

                    }

                    //Commit current transaction  
                    //sqlTransaction.Commit();
                }
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return visitor;
        }


        public void UpdateUndianCalon(string calonName, string kodParlimen)
        {

            var connection = new SqlConnection(ConnectionString);
           string exception = string.Empty;
            try
            {
                connection.Open();

                SqlCommand sqlCommand = new SqlCommand($"UPDATE Calon set Jumlah_Undian = Jumlah_Undian + 1 WHERE " +
                    $"Nama_Calon LIKE '%{calonName}%' AND Kod_Parlimen = '{kodParlimen}'", connection);
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                exception = $"FAILED Updating - {calonName} - { kodParlimen} : { ex.Message}";

            }
            finally
            {
                connection.Close();
            }

        }

        public void ResetUndianCalon(string calonName, string kodParlimen)
        {

            var connection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand;
            string exception = string.Empty;
            try
            {
                connection.Open();

                if(!string.IsNullOrEmpty(calonName) && !string.IsNullOrEmpty(kodParlimen))
                {
                    sqlCommand = new SqlCommand($"UPDATE Calon set Jumlah_Undian = 0 WHHERE " +
                    $"Nama_Calon LIKE '%{calonName}%' AND Kod_Parlimen = '{kodParlimen}'", connection);
                }

                else
                {
                    sqlCommand = new SqlCommand($"UPDATE Calon set Jumlah_Undian = 0", connection);
                }
            
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                exception = $"FAILED Updating - {calonName} - { kodParlimen} : { ex.Message}";

            }
            finally
            {
                connection.Close();
            }

        }

    }
}
