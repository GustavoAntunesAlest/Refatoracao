using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LegacyProcs.Models;

namespace LegacyProcs.Repositories;

/// <summary>
/// Implementação do repositório de Técnico
/// ✅ MIGRADO: .NET 8 com async/await
/// </summary>
public class TecnicoRepository : ITecnicoRepository
{
    private readonly string _connString;

    public TecnicoRepository(IConfiguration configuration)
    {
        _connString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string não encontrada");
    }

    public async Task<IEnumerable<Tecnico>> GetAllAsync(string? filtro = null)
    {
        var lista = new List<Tecnico>();

        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            string sql;
            SqlCommand cmd;

            if (string.IsNullOrEmpty(filtro))
            {
                sql = "SELECT * FROM Tecnico ORDER BY Nome";
                cmd = new SqlCommand(sql, conn);
            }
            else
            {
                sql = "SELECT * FROM Tecnico WHERE Nome LIKE @Filtro OR Especialidade LIKE @Filtro ORDER BY Nome";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Filtro", "%" + filtro + "%");
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

    public async Task<IEnumerable<Tecnico>> GetDisponiveisAsync()
    {
        var lista = new List<Tecnico>();

        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = "SELECT * FROM Tecnico WHERE Status = 'Ativo' ORDER BY Nome";
            using (var cmd = new SqlCommand(sql, conn))
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

    public async Task<Tecnico?> GetByIdAsync(int id)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = "SELECT * FROM Tecnico WHERE Id = @Id";
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

    public async Task<int> CreateAsync(Tecnico tecnico)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = @"INSERT INTO Tecnico (Nome, Email, Telefone, Especialidade, Status, DataCadastro) 
                       VALUES (@Nome, @Email, @Telefone, @Especialidade, @Status, @DataCadastro);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Nome", tecnico.Nome);
                cmd.Parameters.AddWithValue("@Email", (object?)tecnico.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telefone", (object?)tecnico.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Especialidade", (object?)tecnico.Especialidade ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", tecnico.Status);
                cmd.Parameters.AddWithValue("@DataCadastro", tecnico.DataCadastro);

                var id = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(id);
            }
        }
    }

    public async Task<bool> UpdateAsync(Tecnico tecnico)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = @"UPDATE Tecnico SET 
                       Nome = @Nome, 
                       Email = @Email, 
                       Telefone = @Telefone, 
                       Especialidade = @Especialidade, 
                       Status = @Status 
                       WHERE Id = @Id";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Nome", tecnico.Nome);
                cmd.Parameters.AddWithValue("@Email", (object?)tecnico.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telefone", (object?)tecnico.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Especialidade", (object?)tecnico.Especialidade ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", tecnico.Status);
                cmd.Parameters.AddWithValue("@Id", tecnico.Id);

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

            var sql = "DELETE FROM Tecnico WHERE Id = @Id";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    private static Tecnico MapFromReader(SqlDataReader reader)
    {
        return new Tecnico
        {
            Id = (int)reader["Id"],
            Nome = reader["Nome"].ToString() ?? string.Empty,
            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
            Telefone = reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : null,
            Especialidade = reader["Especialidade"] != DBNull.Value ? reader["Especialidade"].ToString() : null,
            Status = reader["Status"].ToString() ?? "Ativo",
            DataCadastro = (DateTime)reader["DataCadastro"]
        };
    }
}
