import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

/**
 * Service para funcionalidades de IA (Google Gemini)
 * ✨ NOVA FEATURE - Assistente inteligente para ordens de serviço
 */
export interface IAResponse {
  descricao?: string;
  especialidade?: string;
  prioridade?: string;
  tempo?: string;
}

@Injectable({
  providedIn: 'root'
})
export class IAAssistenteService {
  private readonly apiUrl = `${environment.apiUrl}/iaassistente`;

  constructor(private http: HttpClient) { }

  /**
   * Gera descrição automática baseada no título
   */
  gerarDescricao(titulo: string): Observable<IAResponse> {
    return this.http.post<IAResponse>(`${this.apiUrl}/gerar-descricao`, { titulo });
  }

  /**
   * Sugere especialidade de técnico baseada na descrição
   */
  sugerirTecnico(descricao: string): Observable<IAResponse> {
    return this.http.post<IAResponse>(`${this.apiUrl}/sugerir-tecnico`, { descricao });
  }

  /**
   * Analisa e classifica a prioridade
   */
  analisarPrioridade(descricao: string): Observable<IAResponse> {
    return this.http.post<IAResponse>(`${this.apiUrl}/analisar-prioridade`, { descricao });
  }

  /**
   * Estima tempo necessário para conclusão
   */
  estimarTempo(descricao: string): Observable<IAResponse> {
    return this.http.post<IAResponse>(`${this.apiUrl}/estimar-tempo`, { descricao });
  }
}
