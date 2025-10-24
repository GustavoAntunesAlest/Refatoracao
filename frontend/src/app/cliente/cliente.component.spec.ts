import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ClienteComponent } from './cliente.component';

describe('ClienteComponent - Validação de CNPJ', () => {
  let component: ClienteComponent;
  let fixture: ComponentFixture<ClienteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClienteComponent ],
      imports: [ 
        HttpClientTestingModule,
        MatSnackBarModule
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClienteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('validarCNPJ', () => {
    it('deve rejeitar CNPJ vazio', () => {
      expect(component.validarCNPJ('')).toBeFalse();
      expect(component.validarCNPJ(null as any)).toBeFalse();
      expect(component.validarCNPJ(undefined as any)).toBeFalse();
    });

    it('deve rejeitar CNPJ com menos de 14 dígitos', () => {
      expect(component.validarCNPJ('123456789')).toBeFalse();
      expect(component.validarCNPJ('12345678901')).toBeFalse();
      expect(component.validarCNPJ('1234567890123')).toBeFalse();
    });

    it('deve rejeitar CNPJ com mais de 14 dígitos', () => {
      expect(component.validarCNPJ('123456789012345')).toBeFalse();
      expect(component.validarCNPJ('12345678901234567')).toBeFalse();
    });

    it('deve rejeitar CNPJ com todos os dígitos iguais', () => {
      expect(component.validarCNPJ('00000000000000')).toBeFalse();
      expect(component.validarCNPJ('11111111111111')).toBeFalse();
      expect(component.validarCNPJ('22222222222222')).toBeFalse();
      expect(component.validarCNPJ('33333333333333')).toBeFalse();
      expect(component.validarCNPJ('44444444444444')).toBeFalse();
      expect(component.validarCNPJ('55555555555555')).toBeFalse();
      expect(component.validarCNPJ('66666666666666')).toBeFalse();
      expect(component.validarCNPJ('77777777777777')).toBeFalse();
      expect(component.validarCNPJ('88888888888888')).toBeFalse();
      expect(component.validarCNPJ('99999999999999')).toBeFalse();
    });

    it('deve rejeitar CNPJ com dígitos verificadores inválidos', () => {
      expect(component.validarCNPJ('12345678000100')).toBeFalse(); // Dígitos errados
      expect(component.validarCNPJ('11222333000181')).toBeFalse(); // Primeiro dígito errado
      expect(component.validarCNPJ('11222333000171')).toBeFalse(); // Segundo dígito errado
    });

    it('deve aceitar CNPJs válidos sem formatação', () => {
      // CNPJs reais válidos (exemplos públicos)
      expect(component.validarCNPJ('11222333000181')).toBeTrue();
      expect(component.validarCNPJ('11444777000161')).toBeTrue();
      expect(component.validarCNPJ('34028316000103')).toBeTrue(); // Correios
      expect(component.validarCNPJ('00000000000191')).toBeTrue(); // CNPJ válido especial
    });

    it('deve aceitar CNPJs válidos com formatação', () => {
      expect(component.validarCNPJ('11.222.333/0001-81')).toBeTrue();
      expect(component.validarCNPJ('11.444.777/0001-61')).toBeTrue();
      expect(component.validarCNPJ('34.028.316/0001-03')).toBeTrue();
    });

    it('deve aceitar CNPJ com pontos, barras e hífens', () => {
      const cnpjFormatado = '11.222.333/0001-81';
      const cnpjSemFormatacao = '11222333000181';
      
      expect(component.validarCNPJ(cnpjFormatado)).toBeTrue();
      expect(component.validarCNPJ(cnpjSemFormatacao)).toBeTrue();
    });

    it('deve rejeitar CNPJ com letras', () => {
      expect(component.validarCNPJ('1122233300018A')).toBeFalse();
      expect(component.validarCNPJ('ABC22333000181')).toBeFalse();
    });

    it('deve rejeitar CNPJ com caracteres especiais inválidos', () => {
      expect(component.validarCNPJ('11@22233300018!')).toBeFalse();
      expect(component.validarCNPJ('11#22233300018$')).toBeFalse();
    });

    it('deve validar múltiplos CNPJs válidos conhecidos', () => {
      // Lista de CNPJs válidos para teste
      const cnpjsValidos = [
        '11222333000181',
        '11444777000161',
        '34028316000103',
        '00000000000191',
        '11222333000181',
        '07526557000162'
      ];

      cnpjsValidos.forEach(cnpj => {
        expect(component.validarCNPJ(cnpj)).toBeTrue();
      });
    });

    it('deve validar CNPJs com espaços (que devem ser removidos)', () => {
      expect(component.validarCNPJ('11 222 333 0001 81')).toBeTrue();
      expect(component.validarCNPJ(' 11222333000181 ')).toBeTrue();
    });
  });

  describe('formatarCNPJ', () => {
    it('deve formatar CNPJ corretamente', () => {
      expect(component.formatarCNPJ('11222333000181')).toBe('11.222.333/0001-81');
      expect(component.formatarCNPJ('34028316000103')).toBe('34.028.316/0001-03');
    });

    it('deve retornar string vazia para CNPJ vazio', () => {
      expect(component.formatarCNPJ('')).toBe('');
      expect(component.formatarCNPJ(null as any)).toBe('');
    });
  });
});
