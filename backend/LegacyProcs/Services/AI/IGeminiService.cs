namespace LegacyProcs.Services.AI;

/// <summary>
/// Interface para serviço de IA Gemini
/// ✨ NOVA FEATURE - Assistente inteligente para ordens de serviço
/// </summary>
public interface IGeminiService
{
    /// <summary>
    /// Gera descrição automática baseada no título da ordem de serviço
    /// </summary>
    Task<string> GerarDescricaoAsync(string titulo);

    /// <summary>
    /// Sugere técnico disponível baseado na descrição do problema e lista de técnicos
    /// </summary>
    Task<string> SugerirTecnicoAsync(string descricao, List<string> tecnicosDisponiveis);

    /// <summary>
    /// Analisa e classifica a prioridade da ordem de serviço
    /// </summary>
    Task<string> AnalisarPrioridadeAsync(string descricao);

    /// <summary>
    /// Estima tempo necessário para conclusão do serviço
    /// </summary>
    Task<string> EstimarTempoAsync(string descricao);
}
