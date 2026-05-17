using Dapper;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Factoria.Proyectos.Api.Shared.Dto.Genericas;
using Factoria.Proyectos.Api.Infraestructura.Servicios;
using System.Data;

namespace Factoria.Proyectos.Api.Infraestructura.Data
{
    public class DapperRepository
    {
        private readonly IConfiguration _config;
        private readonly ArchivoTexto _log;
        private readonly Log _settings;

        public DapperRepository(IConfiguration config, ArchivoTexto log, IOptions<Log> settings)
        {
            _config = config;
            _log = log;
            _settings = settings.Value;
        }

        private NpgsqlConnection GetConnection(string connectionName) // <-- Usamos NpgsqlConnection
        {
            string? connectionString = _config.GetConnectionString(connectionName) ?? _config[connectionName] ?? _config["ConnectionStrings:" + connectionName];

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = connectionName;
            }

            bool esCadenaNormal = connectionString.Contains("Host=", StringComparison.OrdinalIgnoreCase)
                               || connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase);

            if (!esCadenaNormal)
            {
                try
                {
                    var base64Bytes = Convert.FromBase64String(connectionString);
                    string decodedString = System.Text.Encoding.UTF8.GetString(base64Bytes);

                    if (decodedString.Contains("Host=", StringComparison.OrdinalIgnoreCase) ||
                        decodedString.Contains("Server=", StringComparison.OrdinalIgnoreCase))
                    {
                        connectionString = decodedString;
                        Console.WriteLine("[DAPPER] Cadena Base64 detectada y decodificada.");
                    }
                }
                catch
                {
                    Console.WriteLine("[DAPPER WARN] No era Base64, usando original.");
                }
            }

            if (string.IsNullOrEmpty(connectionString)) throw new Exception("Connection String vacía");

            return new NpgsqlConnection(connectionString); 
        }

        public async Task<T?> QuerySingleAsync<T>(
            string sqlOrSp,
            object? parametros = null,
            string connectionName = "_ConexionBD",
            CommandType commandType = CommandType.StoredProcedure) 
        {
            try
            {
                using var cn = GetConnection(connectionName);
                return await cn.QueryFirstOrDefaultAsync<T>(
                    sqlOrSp,
                    parametros,
                    commandType: commandType 
                );
            }
            catch (Exception ex)
            {
                _log.GenerarArchivo(ex, _settings.RutaLog, "Error_Dapper_QuerySingle.txt");
                Console.WriteLine($"[CRITICAL DAPPER ERROR] SQL/SP: {sqlOrSp} \n{ex}");
                throw;
            }
        }

        public async Task<IEnumerable<T>> QueryListAsync<T>(
            string sqlOrSp,
            object? parametros = null,
            string connectionName = "_ConexionBD",
            CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var cn = GetConnection(connectionName);
                return await cn.QueryAsync<T>(
                    sqlOrSp,
                    parametros,
                    commandType: commandType
                );
            }
            catch (Exception ex)
            {
                _log.GenerarArchivo(ex, _settings.RutaLog, "Error_Dapper_QueryList.txt");
                Console.WriteLine($"[CRITICAL DAPPER ERROR] SQL/SP: {sqlOrSp} \n{ex}");
                throw;
            }
        }

        public async Task<T> QueryMultipleAsync<T>(
            string sqlOrSp,
            Func<SqlMapper.GridReader, T> map,
            object? parametros = null,
            string connectionName = "_ConexionBD",
            CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var cn = GetConnection(connectionName);
                using var multi = await cn.QueryMultipleAsync(
                    sqlOrSp,
                    parametros,
                    commandType: commandType
                );

                return map(multi);
            }
            catch (Exception ex)
            {
                _log.GenerarArchivo(ex, _settings.RutaLog, "Error_Dapper_QueryMultiple.txt");
                throw;
            }
        }

        public async Task<int> ExecuteAsync(
            string sqlOrSp,
            object? parametros = null,
            string connectionName = "_ConexionBD",
            CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var cn = GetConnection(connectionName);
                return await cn.ExecuteAsync(
                    sqlOrSp,
                    parametros,
                    commandType: commandType
                );
            }
            catch (Exception ex)
            {
                _log.GenerarArchivo(ex, _settings.RutaLog, "Error_Dapper_Execute.txt");
                Console.WriteLine($"[CRITICAL DAPPER ERROR] SQL/SP: {sqlOrSp} \n{ex}");
                throw;
            }
        }
    }
}