import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ClienteService } from './cliente.service';

describe('ClienteService', () => {
  let service: ClienteService;
  let httpMock: HttpTestingController;
  const apiUrl = 'http://localhost:5000/api/cliente';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ClienteService]
    });
    service = TestBed.inject(ClienteService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all clientes', () => {
    const mockClientes = [
      { id: 1, razaoSocial: 'Empresa A', nomeFantasia: 'A Ltda', cnpj: '12345678000190', email: 'contato@a.com', telefone: '1133334444', endereco: 'Rua A', cidade: 'São Paulo', estado: 'SP', cep: '01000-000', dataCadastro: '2025-10-15' },
      { id: 2, razaoSocial: 'Empresa B', nomeFantasia: 'B Ltda', cnpj: '98765432000110', email: 'contato@b.com', telefone: '1122223333', endereco: 'Rua B', cidade: 'Rio de Janeiro', estado: 'RJ', cep: '20000-000', dataCadastro: '2025-10-14' }
    ];

    service.getAll().subscribe(clientes => {
      expect(clientes.length).toBe(2);
      expect(clientes).toEqual(mockClientes);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockClientes);
  });

  it('should get cliente by id', () => {
    const mockCliente = { id: 1, razaoSocial: 'Empresa A', nomeFantasia: 'A Ltda', cnpj: '12345678000190', email: 'contato@a.com', telefone: '1133334444', endereco: 'Rua A', cidade: 'São Paulo', estado: 'SP', cep: '01000-000', dataCadastro: '2025-10-15' };

    service.getById(1).subscribe(cliente => {
      expect(cliente).toEqual(mockCliente);
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockCliente);
  });

  it('should create cliente', () => {
    const newCliente = { razaoSocial: 'Empresa C', nomeFantasia: 'C Ltda', cnpj: '11111111000111', email: 'contato@c.com', telefone: '1144445555', endereco: 'Rua C', cidade: 'Belo Horizonte', estado: 'MG', cep: '30000-000' };
    const mockResponse = { ...newCliente, id: 3, dataCadastro: '2025-10-15' };

    service.create(newCliente).subscribe(cliente => {
      expect(cliente).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newCliente);
    req.flush(mockResponse);
  });

  it('should update cliente', () => {
    const updatedCliente = { id: 1, razaoSocial: 'Empresa A Atualizada', nomeFantasia: 'A Ltda', cnpj: '12345678000190', email: 'novo@a.com', telefone: '1133334444', endereco: 'Rua A', cidade: 'São Paulo', estado: 'SP', cep: '01000-000', dataCadastro: '2025-10-15' };

    service.update(1, updatedCliente).subscribe(response => {
      expect(response).toEqual(updatedCliente);
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedCliente);
    req.flush(updatedCliente);
  });

  it('should delete cliente', () => {
    service.delete(1).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });
});
