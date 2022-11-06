using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace PRU15_Shared
{

    public class Database
        {
            #region Singleton pattern
            /// <summary>
            /// Singleton
            /// </summary>
            private static readonly Database _instance = new Database();
            #endregion


            private SqliteConnection sqlite_conn;
            //private List<TokenViewModel> listTokenViewModel;
            Database()
            {
                // Create a new database connection:
                try
                {
                string currentProjectDir = Environment.CurrentDirectory;
                string projectDirectory = currentProjectDir.Contains("bin") ? Directory.GetParent(Environment.CurrentDirectory).FullName :
                     Directory.GetParent(Environment.CurrentDirectory).FullName;
                string cs = $"Data Source={currentProjectDir}/pru15data.db";
                sqlite_conn = new SqliteConnection(cs);
                    //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
                }
                catch (Exception ex)
                {
                Console.WriteLine($"Error when initializing  database : {ex.Message} ");
                Console.ReadKey();
            }

            }
            public static Database Instance
            {
                get { return _instance; }
            }

            public void InitializeDatabase()
            {
                try
                {
                    using (sqlite_conn)
                    {
                        sqlite_conn.Open();

                        var command = sqlite_conn.CreateCommand();
                        command.CommandText =
                            @"INSERT INTO Token ('Symbol', 'Name', 'TotalSupply', 'ContactAddress', 'TotalHolders')
                        VALUES('VEN', 'Vechain', 35987133, '0xd850942ef8811f2a866692a623011bde52a462c1', 65),
                        ('ZIR', 'Zilliqa', 53272942, '0x05f4a42e251f2d52b8ed15e9fedaacfcef1fad27', 54),
                        ('MKR', 'Maker', 45987133, '0x9f8f72aa9304c8b593d555f12ef6589cc3a579a2', 567),
                        ('BNB', 'BNB', 16579517, '0xB8c77482e45F1F44dE1745F52C74426C631bDD52', 4234234)";


                        command.ExecuteNonQuery();
                    }
                }
                catch (System.Exception ex)
                {
                    string msgv = ex.Message;
                }


            }

        public void InsertDataParliament()
        {
            var listData = GetCalon();

            List<DataParlimen> datas = new List<DataParlimen>();

            foreach (var item in listData)
            {
                if (!datas.Exists((DataParlimen obj) => obj.KodParlimen == item.KodParlimen))
                {
                    datas.Add(new DataParlimen
                    {
                        KodParlimen = item.KodParlimen,
                        NamaParlimen = item.NamaParlimen,
                        Negeri = item.Negeri,
                        JumlahCalon = JumlahCalon(item.KodParlimen).ToString(),
                    });
                }

            }

            foreach(var item in datas)
            {
                InsertOrUpdateData(item);
            }

            
        }

        public int JumlahCalon(string kodParliment)
        {
            List<string> listData = new List<string>();

            try
            {
                using (sqlite_conn)
                {
                    sqlite_conn.Open();
                    var command = sqlite_conn.CreateCommand();

                    command.CommandText =
                          $@"select Kod_Parlimen from Calon where Kod_Parlimen = '{kodParliment.ToUpper()}'";
                

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            listData.Add(reader["Kod_Parlimen"].ToString());
                       
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlite_conn.Close();
            }

            return listData.Count;
        }

        public List<DataParlimen> GetParliament(string negeri = null)
        {
            List<DataParlimen> listTokenData = new List<DataParlimen>();
            try
            {
                using (sqlite_conn)
                {
                    sqlite_conn.Open();

                    var command = sqlite_conn.CreateCommand();

                    if(string.IsNullOrEmpty(negeri))
                    {
                        command.CommandText =
                            @"SELECT Code, Name, State, Jumlah_Calon FROM 'Parliament'";
                    }
                    else
                    {
                        command.CommandText = $@"SELECT Code, Name, State, Jumlah_Calon FROM Parliament Where State = '{negeri.ToUpper()}'";

                    }


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string parliment_dun;
                            if(reader["Code"].ToString().StartsWith("P"))
                            {
                                parliment_dun = "PARLIMEN";
                            }
                            else
                            {
                                parliment_dun = "DUN";
                            }

                            listTokenData.Add(new DataParlimen
                            {
                                Parlimen_Dun = parliment_dun,
                                KodParlimen = reader["Code"].ToString(),
                                NamaParlimen = reader["Name"].ToString(),
                                Negeri = reader["State"].ToString(),
                                JumlahCalon = reader["Jumlah_Calon"].ToString()
                            }) ; 
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlite_conn.Close();
            }

            return listTokenData;

        }

        public List<DataCalon> GetCalon(string negeri = null , string kodParlimen = null)
        {
            List<DataCalon> listCalon = new List<DataCalon>();
            try
            {
                using (sqlite_conn)
                {
                    sqlite_conn.Open();

                    var command = sqlite_conn.CreateCommand();

                    if (string.IsNullOrEmpty(kodParlimen) && string.IsNullOrEmpty(negeri))
                    {
                        command.CommandText =
                       $@"SELECT Nama_Calon, Nama_Parlimen, Nama_Parti, Negeri, Kod_Parlimen FROM Calon";
                    }

                    else if (string.IsNullOrEmpty(kodParlimen))
                    {
                        command.CommandText =
                            $@"SELECT Nama_Calon, Nama_Parlimen, Nama_Parti, Negeri, Kod_Parlimen FROM 'Calon'
                                WHERE Negeri = '{negeri.ToUpper()}'";
                    }
                    else 
                    {
                        command.CommandText =
                           $@"SELECT Nama_Calon, Nama_Parlimen, Nama_Parti, Negeri, Kod_Parlimen FROM 'Calon'
                                WHERE Negeri = '{negeri.ToUpper()}' AND Kod_Parlimen = '{kodParlimen.ToUpper()}'";
                    }          


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string parliment_dun;
                            if (reader["Kod_Parlimen"].ToString().StartsWith("P"))
                            {
                                parliment_dun = "PARLIMEN";
                            }
                            else
                            {
                                parliment_dun = "DUN";
                            }

                            listCalon.Add(new DataCalon
                            {
                                Parlimen_Dun = parliment_dun,
                                NamaCalon = reader["Nama_Calon"].ToString(),
                                NamaParlimen = reader["Nama_Parlimen"].ToString(),
                                NamaParti = reader["Nama_Parti"].ToString(),
                                Negeri = reader["Negeri"].ToString(),
                                KodParlimen = reader["Kod_Parlimen"].ToString(),

                            });
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlite_conn.Close();
            }

            return listCalon;

        }
        //public List<string> GetAllSymbols()
        //{
        //    List<string> listSymbol = new List<string>();
        //    try
        //    {
        //        using (sqlite_conn)
        //        {
        //            sqlite_conn.Open();

        //            var command = sqlite_conn.CreateCommand();

        //            command.CommandText =
        //                @"SELECT Symbol FROM Token";

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    var symbol = reader.GetString(0);
        //                    listSymbol.Add(symbol);
        //                }
        //            }
        //        }

        //        return listSymbol;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        sqlite_conn.Close();
        //    }
        //}

        public void InsertOrUpdateData(DataParlimen tokenData)
            {
            try
            {
                using (sqlite_conn)
                {

                    var command = sqlite_conn.CreateCommand();
                    if (!CheckIfDataExist(tokenData.KodParlimen))
                    {

                        command.CommandText = @"INSERT INTO Parliament ('Code', 'Name', 'State' , 'Jumlah_Calon')
                        VALUES($code, $name, $state ,$jumlah_calon )";


                        sqlite_conn.Open();

                        command.Parameters.AddWithValue("$code", tokenData.KodParlimen);
                        command.Parameters.AddWithValue("$name", tokenData.NamaParlimen);
                        command.Parameters.AddWithValue("$state", tokenData.Negeri);
                        command.Parameters.AddWithValue("$jumlah_calon", tokenData.JumlahCalon);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error when save to database : {ex.Message} ");
            }
            finally
            {
                sqlite_conn.Close();
            }
            }

            private bool CheckIfDataExist(string code)
            {
                int counterCheck = 0;
                try
                {
                    using (sqlite_conn)
                    {
                        sqlite_conn.Open();
                        var command = sqlite_conn.CreateCommand();

                        command.CommandText =
                            @"SELECT Name FROM Parliament Where Code=$code";

                        command.Parameters.AddWithValue("$code", code);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                counterCheck++;
                            }
                        }
                    }

                    return counterCheck > 0;
                }
                catch (System.Exception)
                {
                    throw;
                }
                finally
                {
                    sqlite_conn.Close();
                }

            }
            /// <summary>
            /// Update token price in database from API calling response. Occurs every 5 minutes
            /// </summary>
            /// <param name="symbol">Token Symbol</param>
            /// <param name="price">Updated token price from API</param>
            public void UpdateTokenPrice(string symbol, decimal price)
            {
                try
                {
                    using (sqlite_conn)
                    {
                        sqlite_conn.Open();

                        var command = sqlite_conn.CreateCommand();
                        command.CommandText =
                            @"UPDATE Token SET Price = $price WHERE Symbol = $symbol ";
                        command.Parameters.AddWithValue("$price", price);
                        command.Parameters.AddWithValue("$symbol", symbol);
                        var value = command.ExecuteNonQuery();
                        Console.WriteLine($"Row(s) updated: {value}");
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }
    
}
