using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LegacyProcs.Models;

namespace LegacyProcs.Repositories;

/// <summary>
/// Implementação do repositório de Ordem de Serviço
/// ✅ MIGRADO: .NET 8 com async/await
/// </summary>
public class OrdemServicoRepository : IOrdemServicoRepository
{
    private readonly string _connString;

    public OrdemServicoRepository(IConfiguration configuration)
    {
        _connString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string não encontrada");
    }

    public async Task<IEnumerable<OrdemServico>> GetAllAsync(string? filtro = null)
    {
        var lista = new List<OrdemServico>();

        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            string sql;
            SqlCommand cmd;

            if (string.IsNullOrEmpty(filtro))
            {
                sql = "SELECT * FROM OrdemServico ORDER BY DataCriacao DESC";
                cmd = new SqlCommand(sql, conn);
            }
            else
            {
                sql = "SELECT * FROM OrdemServico WHERE Titulo LIKE @Filtro ORDER BY DataCriacao DESC";
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

    public async Task<OrdemServico?> GetByIdAsync(int id)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = "SELECT * FROM OrdemServico WHERE Id = @Id";
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

    public async Task<int> CreateAsync(OrdemServico ordemServico)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            var sql = @"INSERT INTO OrdemServico (Titulo, Descricao, Tecnico, Status, DataCriacao) 
                       VALUES (@Titulo, @Descricao, @Tecnico, @Status, @DataCriacao);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Titulo", ordemServico.Titulo);
                cmd.Parameters.AddWithValue("@Descricao", (object?)ordemServico.Descricao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Tecnico", ordemServico.Tecnico);
                cmd.Parameters.AddWithValue("@Status", ordemServico.Status);
                cmd.Parameters.AddWithValue("@DataCriacao", ordemServico.DataCriacao);

                var id = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(id);
            }
        }
    }

    public async Task<bool> UpdateAsync(OrdemServico ordemServico)
    {
        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            // ✅ FIX: Atualizar TODOS os campos, não apenas Status
            var sql = @"UPDATE OrdemServico SET 
                       Titulo = @Titulo,
                       Descricao = @Descricao,
                       Tecnico = @Tecnico,
                       Status = @Status, 
                       DataAtualizacao = @DataAtualizacao 
                       WHERE Id = @Id";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Titulo", ordemServico.Titulo);
                cmd.Parameters.AddWithValue("@Descricao", (object?)ordemServico.Descricao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Tecnico", ordemServico.Tecnico);
                cmd.Parameters.AddWithValue("@Status", ordemServico.Status);
                cmd.Parameters.AddWithValue("@DataAtualizacao", DateTime.Now);
                cmd.Parameters.AddWithValue("@Id", ordemServico.Id);

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

            var sql = "DELETE FROM OrdemServico WHERE Id = @Id";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<PagedResult<OrdemServico>> GetPagedAsync(int pageNumber, int pageSize, string? filtro = null)
    {
        var result = new PagedResult<OrdemServico>
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        using (var conn = new SqlConnection(_connString))
        {
            await conn.OpenAsync();

            // Contar total de registros
            string countSql;
            SqlCommand countCmd;

            if (string.IsNullOrEmpty(filtro))
            {
                countSql = "SELECT COUNT(*) FROM OrdemServico";
                countCmd = new SqlCommand(countSql, conn);
            }
            else
            {
                countSql = "SELECT COUNT(*) FROM OrdemServico WHERE Titulo LIKE @Filtro";
                countCmd = new SqlCommand(countSql, conn);
                countCmd.Parameters.AddWithValue("@Filtro", "%" + filtro + "%");
            }

            using (countCmd)
            {
                result.TotalCount = (int)await countCmd.ExecuteScalarAsync();
            }

            // Buscar registros paginados
            string sql;
            SqlCommand cmd;
            int offset = (pageNumber - 1) * pageSize;

            if (string.IsNullOrEmpty(filtro))
            {
                sql = @"SELECT * FROM OrdemServico 
                       ORDER BY DataCriacao DESC 
                       OFFSET @Offset ROWS 
                       FETCH NEXT @PageSize ROWS ONLY";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Offset", offset);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
            }
            else
            {
                sql = @"SELECT * FROM OrdemServico 
                       WHERE Titulo LIKE @Filtro 
                       ORDER BY DataCriacao DESC 
                       OFFSET @Offset ROWS 
                       FETCH NEXT @PageSize ROWS ONLY";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Filtro", "%" + filtro + "%");
                cmd.Parameters.AddWithValue("@Offset", offset);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
            }

            using (cmd)
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Items.Add(MapFromReader(reader));
                    }
                }
            }
        }

        return result;
    }

    private static OrdemServico MapFromReader(SqlDataReader reader)
    {
        return new OrdemServico
        {
            Id = (int)reader["Id"],
            Titulo = reader["Titulo"].ToString() ?? string.Empty,
            Descricao = reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : null,
            Tecnico = reader["Tecnico"].ToString() ?? string.Empty,
            Status = reader["Status"].ToString() ?? string.Empty,
            DataCriacao = (DateTime)reader["DataCriacao"],
            DataAtualizacao = reader["DataAtualizacao"] != DBNull.Value ? (DateTime?)reader["DataAtualizacao"] : null
        };
    }
}
