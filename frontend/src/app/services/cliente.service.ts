import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

/**
 * Interface para Cliente
 * ✅ MELHORIA: Tipagem forte
 */
export interface Cliente {
  id?: number;
  razaoSocial: string;
  cnpj: string;
  endereco?: string;
  telefone?: string;
  email?: string;
}

/**
 * Service para gerenciar Clientes
 * ✅ MELHORIA: Isola lógica HTTP do componente
 */
@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  private readonly apiUrl = `${environment.apiUrl}/cliente`;

  constructor(private http: HttpClient) { }

  /**
   * Busca todos os clientes
   */
  getAll(filtro?: string): Observable<Cliente[]> {
    let params = new HttpParams();
    if (filtro) {
      params = params.set('filtro', filtro);
    }
    return this.http.get<Cliente[]>(this.apiUrl, { params });
  }

  /**
   * Busca cliente por ID
   */
  getById(id: number): Observable<Cliente> {
    return this.http.get<Cliente>(`${this.apiUrl}/${id}`);
  }

  /**
   * Cria novo cliente
   */
  create(cliente: Cliente): Observable<Cliente> {
    return this.http.post<Cliente>(this.apiUrl, cliente);
  }

  /**
   * Atualiza cliente
   */
  update(id: number, cliente: Cliente): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, cliente);
  }

  /**
   * Exclui cliente
   */
  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
