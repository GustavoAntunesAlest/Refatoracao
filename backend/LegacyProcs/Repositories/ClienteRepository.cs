using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LegacyProcs.Models;

namespace LegacyProcs.Repositories;

/// <summary>
/// Implementação do repositório de Cliente
/// ✅ MIGRADO: .NET 8 com async/await
/// </summary>
public class ClienteRepository : IClienteRepository
{
    private readonly string _connString;

    public ClienteRepository(IConfiguration configuration)
    {
        _connString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string não encontrada");
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync(string? busca = null)
    {
        var lista = new List<Cliente>();

        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            string sql;
            SqlCommand cmd;

            if (string.IsNullOrEmpty(busca))
            {
                sql = "SELECT * FROM Cliente ORDER BY RazaoSocial";
                cmd = new SqlCommand(sql, conn);
            }
            else
            {
                sql = "SELECT * FROM Cliente WHERE RazaoSocial LIKE @Busca OR CNPJ LIKE @Busca ORDER BY RazaoSocial";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Busca", "%" + busca + "%");
            }

            using (cmd)
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(MapFromReader(reader));
                    }
                }
            }
        }

        return lista;
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = "SELECT * FROM Cliente WHERE Id = @Id";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapFromReader(reader);
                    }
                }
            }
        }

        return null;
    }

    public async Task<int> CreateAsync(Cliente cliente)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = @"INSERT INTO Cliente (RazaoSocial, NomeFantasia, CNPJ, Email, Telefone, Endereco, Cidade, Estado, CEP, DataCadastro) 
                       VALUES (@RazaoSocial, @NomeFantasia, @CNPJ, @Email, @Telefone, @Endereco, @Cidade, @Estado, @CEP, @DataCadastro);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@RazaoSocial", cliente.RazaoSocial);
                cmd.Parameters.AddWithValue("@NomeFantasia", (object?)cliente.NomeFantasia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CNPJ", cliente.CNPJ);
                cmd.Parameters.AddWithValue("@Email", (object?)cliente.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telefone", (object?)cliente.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Endereco", (object?)cliente.Endereco ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cidade", (object?)cliente.Cidade ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", (object?)cliente.Estado ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CEP", (object?)cliente.CEP ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DataCadastro", cliente.DataCadastro);

                var id = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(id);
            }
        }
    }

    public async Task<bool> UpdateAsync(Cliente cliente)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = @"UPDATE Cliente SET 
                       RazaoSocial = @RazaoSocial, 
                       NomeFantasia = @NomeFantasia, 
                       CNPJ = @CNPJ, 
                       Email = @Email, 
                       Telefone = @Telefone, 
                       Endereco = @Endereco, 
                       Cidade = @Cidade, 
                       Estado = @Estado, 
                       CEP = @CEP 
                       WHERE Id = @Id";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@RazaoSocial", cliente.RazaoSocial);
                cmd.Parameters.AddWithValue("@NomeFantasia", (object?)cliente.NomeFantasia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CNPJ", cliente.CNPJ);
                cmd.Parameters.AddWithValue("@Email", (object?)cliente.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telefone", (object?)cliente.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Endereco", (object?)cliente.Endereco ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cidade", (object?)cliente.Cidade ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", (object?)cliente.Estado ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CEP", (object?)cliente.CEP ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", cliente.Id);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = "DELETE FROM Cliente WHERE Id = @Id";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    private static Cliente MapFromReader(SqlDataReader reader)
    {
        return new Cliente
        {
            Id = (int)reader["Id"],
            RazaoSocial = reader["RazaoSocial"].ToString() ?? string.Empty,
            NomeFantasia = reader["NomeFantasia"] != DBNull.Value ? reader["NomeFantasia"].ToString() : null,
            CNPJ = reader["CNPJ"].ToString() ?? string.Empty,
            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
            Telefone = reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : null,
            Endereco = reader["Endereco"] != DBNull.Value ? reader["Endereco"].ToString() : null,
            Cidade = reader["Cidade"] != DBNull.Value ? reader["Cidade"].ToString() : null,
            Estado = reader["Estado"] != DBNull.Value ? reader["Estado"].ToString() : null,
            CEP = reader["CEP"] != DBNull.Value ? reader["CEP"].ToString() : null,
            DataCadastro = (DateTime)reader["DataCadastro"]
        };
    }
}
