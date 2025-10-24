import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

/**
 * Interface para Técnico
 * ✅ MELHORIA: Tipagem forte
 */
export interface Tecnico {
  id?: number;
  nome: string;
  especialidade?: string;
  telefone?: string;
  email?: string;
  status: string;
}

/**
 * Service para gerenciar Técnicos
 * ✅ MELHORIA: Isola lógica HTTP do componente
 */
@Injectable({
  providedIn: 'root'
})
export class TecnicoService {
  private readonly apiUrl = `${environment.apiUrl}/tecnico`;

  constructor(private http: HttpClient) { }

  /**
   * Busca todos os técnicos
   */
  getAll(filtro?: string): Observable<Tecnico[]> {
    let params = new HttpParams();
    if (filtro) {
      params = params.set('filtro', filtro);
    }
    return this.http.get<Tecnico[]>(this.apiUrl, { params });
  }

  /**
   * Busca técnicos disponíveis
   */
  getDisponiveis(): Observable<Tecnico[]> {
    return this.http.get<Tecnico[]>(`${this.apiUrl}/disponiveis`);
  }

  /**
   * Busca técnico por ID
   */
  getById(id: number): Observable<Tecnico> {
    return this.http.get<Tecnico>(`${this.apiUrl}/${id}`);
  }

  /**
   * Cria novo técnico
   */
  create(tecnico: Tecnico): Observable<Tecnico> {
    return this.http.post<Tecnico>(this.apiUrl, tecnico);
  }

  /**
   * Atualiza técnico
   */
  update(id: number, tecnico: Tecnico): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, tecnico);
  }

  /**
   * Exclui técnico
   */
  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
