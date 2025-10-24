import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

/**
 * Interface para Ordem de Serviço
 * ✅ MELHORIA: Tipagem forte
 */
export interface OrdemServico {
  id?: number;
  titulo: string;
  descricao?: string;
  tecnico: string;
  status: string;
  dataCriacao?: Date;
  dataAtualizacao?: Date;
}

/**
 * Interface para criação de Ordem de Serviço
 */
export interface CreateOrdemServicoDto {
  titulo: string;
  descricao?: string;
  tecnico: string;
}

/**
 * Interface para atualização de Ordem de Serviço
 */
export interface UpdateOrdemServicoDto {
  status: string;
}

/**
 * Service para gerenciar Ordens de Serviço
 * ✅ MELHORIA: Isola lógica HTTP do componente
 */
@Injectable({
  providedIn: 'root'
})
export class OrdemServicoService {
  private readonly apiUrl = `${environment.apiUrl}/ordemservico`;

  constructor(private http: HttpClient) { }

  /**
   * Busca todas as ordens de serviço
   */
  getAll(filtro?: string): Observable<OrdemServico[]> {
    let params = new HttpParams();
    if (filtro) {
      params = params.set('filtro', filtro);
    }
    return this.http.get<OrdemServico[]>(this.apiUrl, { params });
  }

  /**
   * Busca ordem de serviço por ID
   */
  getById(id: number): Observable<OrdemServico> {
    return this.http.get<OrdemServico>(`${this.apiUrl}/${id}`);
  }

  /**
   * Cria nova ordem de serviço
   */
  create(dto: CreateOrdemServicoDto): Observable<OrdemServico> {
    return this.http.post<OrdemServico>(this.apiUrl, dto);
  }

  /**
   * Atualiza ordem de serviço
   */
  update(id: number, dto: UpdateOrdemServicoDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, dto);
  }

  /**
   * Exclui ordem de serviço
   */
  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
