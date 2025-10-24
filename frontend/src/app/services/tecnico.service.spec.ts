import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TecnicoService } from './tecnico.service';

describe('TecnicoService', () => {
  let service: TecnicoService;
  let httpMock: HttpTestingController;
  const apiUrl = 'http://localhost:5000/api/tecnico';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TecnicoService]
    });
    service = TestBed.inject(TecnicoService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all tecnicos', () => {
    const mockTecnicos = [
      { id: 1, nome: 'João Silva', email: 'joao@test.com', telefone: '11999999999', especialidade: 'Elétrica', status: 'Ativo', dataCadastro: '2025-10-15' },
      { id: 2, nome: 'Maria Santos', email: 'maria@test.com', telefone: '11988888888', especialidade: 'Hidráulica', status: 'Ativo', dataCadastro: '2025-10-14' }
    ];

    service.getAll().subscribe(tecnicos => {
      expect(tecnicos.length).toBe(2);
      expect(tecnicos).toEqual(mockTecnicos);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockTecnicos);
  });

  it('should get tecnico by id', () => {
    const mockTecnico = { id: 1, nome: 'João Silva', email: 'joao@test.com', telefone: '11999999999', especialidade: 'Elétrica', status: 'Ativo', dataCadastro: '2025-10-15' };

    service.getById(1).subscribe(tecnico => {
      expect(tecnico).toEqual(mockTecnico);
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockTecnico);
  });

  it('should create tecnico', () => {
    const newTecnico = { nome: 'Pedro Oliveira', email: 'pedro@test.com', telefone: '11977777777', especialidade: 'Ar Condicionado' };
    const mockResponse = { ...newTecnico, id: 3, status: 'Ativo', dataCadastro: '2025-10-15' };

    service.create(newTecnico).subscribe(tecnico => {
      expect(tecnico).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newTecnico);
    req.flush(mockResponse);
  });

  it('should update tecnico', () => {
    const updatedTecnico = { id: 1, nome: 'João Silva', email: 'joao@test.com', telefone: '11999999999', especialidade: 'Elétrica', status: 'Férias', dataCadastro: '2025-10-15' };

    service.update(1, updatedTecnico).subscribe(response => {
      expect(response).toEqual(updatedTecnico);
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedTecnico);
    req.flush(updatedTecnico);
  });

  it('should delete tecnico', () => {
    service.delete(1).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });
});
