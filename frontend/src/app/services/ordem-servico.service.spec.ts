import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { OrdemServicoService } from './ordem-servico.service';

describe('OrdemServicoService', () => {
  let service: OrdemServicoService;
  let httpMock: HttpTestingController;
  const apiUrl = 'http://localhost:5000/api/ordemservico';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [OrdemServicoService]
    });
    service = TestBed.inject(OrdemServicoService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all ordens de servico', () => {
    const mockOrdens = [
      { id: 1, titulo: 'Teste 1', descricao: 'Desc 1', tecnico: 'João', status: 'Pendente', dataCriacao: '2025-10-15' },
      { id: 2, titulo: 'Teste 2', descricao: 'Desc 2', tecnico: 'Maria', status: 'Concluída', dataCriacao: '2025-10-14' }
    ];

    service.getAll().subscribe(ordens => {
      expect(ordens.length).toBe(2);
      expect(ordens).toEqual(mockOrdens);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockOrdens);
  });

  it('should get ordem by id', () => {
    const mockOrdem = { id: 1, titulo: 'Teste 1', descricao: 'Desc 1', tecnico: 'João', status: 'Pendente', dataCriacao: '2025-10-15' };

    service.getById(1).subscribe(ordem => {
      expect(ordem).toEqual(mockOrdem);
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockOrdem);
  });

  it('should create ordem', () => {
    const newOrdem = { titulo: 'Nova OS', descricao: 'Descrição', tecnico: 'Pedro', status: 'Pendente' };
    const mockResponse = { ...newOrdem, id: 3, dataCriacao: '2025-10-15' };

    service.create(newOrdem).subscribe(ordem => {
      expect(ordem).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newOrdem);
    req.flush(mockResponse);
  });

  it('should update ordem', () => {
    const updatedOrdem = { id: 1, titulo: 'Atualizada', descricao: 'Desc', tecnico: 'João', status: 'Em Andamento', dataCriacao: '2025-10-15' };

    service.update(1, updatedOrdem).subscribe(response => {
      expect(response).toEqual(updatedOrdem);
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedOrdem);
    req.flush(updatedOrdem);
  });

  it('should delete ordem', () => {
    service.delete(1).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });

  it('should handle error on getAll', () => {
    service.getAll().subscribe(
      () => fail('should have failed'),
      (error) => {
        expect(error.status).toBe(500);
      }
    );

    const req = httpMock.expectOne(apiUrl);
    req.flush('Error', { status: 500, statusText: 'Server Error' });
  });
});
