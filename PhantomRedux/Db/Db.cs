using Microsoft.AspNetCore.DataProtection;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;

namespace PhantomRedux
{
    public class Db
    {
        private static IDataProtector m_protector;

        private static string GetDbConfigFilename()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "phantom.db");
        }

        /// <summary>
        /// Initialize database details from locally-stored secrets
        /// </summary>
        public static void Initialize()
        {
            // Create
            var provider = DataProtectionProvider.Create("phantom");
            m_protector = provider.CreateProtector("phantom.db");

            // Decrypt string
            string protectedPayload = null;
            try
            {
                // Read encrypted string from disk
                protectedPayload = File.ReadAllText(GetDbConfigFilename());

                // Attempt decryption
                m_connectionString = m_protector.Unprotect(protectedPayload);

                // Attempt connection with MySQL
                Config.RefreshConfig();
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(FileNotFoundException) || string.IsNullOrEmpty(protectedPayload))
                {
                    Console.WriteLine("No database details were found. Please use /Dashboard/setDatabaseDetails to update them.");
                }
                else if (e.GetType() == typeof(CryptographicException))
                {
                    Console.WriteLine("Failed to decrypt database details. Please use /Dashboard/setDatabaseDetails to update them.");
                    Console.WriteLine(e);
                }
                else if (e.GetType() == typeof(MySqlException))
                {
                    Console.WriteLine("Failed to connect to MySQL with stored details. Please use /Dashboard/setDatabaseDetails to update them.");
                    Console.WriteLine(e);
                }
            }
        }

        public static void SetDetails(string host, string port, string username, string password, string database)
        {
            m_connectionString =
                $"server={host};user={username};database={database};port={port};password={password}";
            File.WriteAllText(GetDbConfigFilename(), m_protector.Protect(m_connectionString));
            Config.RefreshConfig();
        }

        /// <summary>
        /// Retrieve valid MySQL connection to make queries with.
        /// </summary>
        ///
        /// This function will return a valid MySqlConnection, or pass a MySqlException if an error
        /// occurs. Using try/catch is recommended, at least in sections where the validity of
        /// database details is checked.
        ///
        /// This can also be used in a `using` statement, e.g. `using (var conn = Db.Get())`
        ///
        /// After calling this function, call `Open()` on the resulting object. Then queries can be
        /// made with MySqlCommand. Call `Close()` when you're finished (this might happen
        /// automatically with `using`, but it's probably good practice either way),
        public static MySqlConnection Get()
        {
            // Return connection
            return new MySqlConnection(m_connectionString);
        }

        private static string EscapeString(string s)
        {
            return s.Replace("'", "\\'");
        }

        /// <summary>
        /// Generate an SQL string where all parameters are escaped (assumes single quotes are used for values)
        /// </summary>
        public static string GetCommand(string format, params object[] arg)
        {
            for (var i = 0; i < arg.Length; i++)
            {
                if (arg[i] is string)
                {
                    arg[i] = EscapeString((string)arg[i]);
                }
            }
            return string.Format(format, arg);
        }

        public static long[] ConvertDBListToIntArray(string s)
        {
            var tokens = s.Split(' ');
            var values = new long[tokens.Length];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = long.Parse(tokens[i]);
            }
            return values;
        }

        public static string ConvertIntArrayToDBList(IEnumerable<long> a)
        {
            StringBuilder dbList = new();
            dbList.AppendJoin(' ', a);

            return dbList.ToString();
        }

        private static void QuickRun(MySqlConnection conn, string query)
        {
            var cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        public static void ResetDatabase(bool config = false,
                                         bool players = false,
                                         bool sessions = false,
                                         bool episodes = false)
        {
            // THESE SCHEMAS ARE NOT PRODUCTION-READY!
            using var conn = Db.Get();
            conn.Open();

            if (config)
            {
                QuickRun(conn,
                    @"DROP TABLE IF EXISTS `pha_config`;
                    CREATE TABLE `pha_config` (
                        id TINYINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
                        is_maintenance TINYINT NOT NULL DEFAULT 0,
                        debug_log TINYINT NOT NULL DEFAULT 0,
                        session_time INT NOT NULL DEFAULT 3600,
                    );
                    INSERT INTO `sw_config` (id) VALUES ('1');");
            }

            if (players)
            {
                QuickRun(conn,
                    @"DROP TABLE IF EXISTS `pha_players`;
                    CREATE TABLE `pha_players` (
                        user_id BIGINT UNSIGNED NOT NULL PRIMARY KEY,
                        login_id VARCHAR[10] NOT NULL,
                        nick_name VARCHAR[12] NOT NULL,
                        avatar_clothing VARCHAR[32],
                        avatar_hat VARCHAR[32],
                    );");
            }
        }

        private static string m_connectionString;
    }
}
