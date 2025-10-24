import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ClienteService, Cliente } from '../services/cliente.service';

/**
 * Componente para gerenciar Clientes
 * ⚠️ ATENÇÃO: Ainda mais código duplicado!
 */
@Component({
  selector: 'app-cliente',
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.css']
})
export class ClienteComponent implements OnInit {
  clientes: any[] = [];
  private apiUrl = 'http://localhost:5000/api/cliente';
  
  mostrarFormulario = false;
  editando = false;
  
  novoCliente: any = {
    razaoSocial: '',
    nomeFantasia: '',
    cnpj: '',
    email: '',
    telefone: '',
    endereco: '',
    cidade: '',
    estado: '',
    cep: ''
  };
  
  clienteEditando: any = null;
  
  // ❌ Lista de estados hardcoded
  estados = [
    'AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA',
    'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN',
    'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'
  ];

  // ✅ REFATORADO: Usa ClienteService
  constructor(
    private clienteService: ClienteService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.carregarClientes();
  }

  carregarClientes() {
    // ✅ CORRIGIDO: Usa service
    this.clienteService.getAll().subscribe(
      (data) => {
        this.clientes = data;
      },
      (error) => {
        console.error('Erro ao carregar clientes:', error);
        this.snackBar.open('Erro ao carregar clientes!', 'Fechar', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    );
  }

  abrirFormulario() {
    this.mostrarFormulario = true;
    this.editando = false;
    this.limparFormulario();
  }

  fecharFormulario() {
    this.mostrarFormulario = false;
    this.limparFormulario();
  }

  limparFormulario() {
    this.novoCliente = {
      razaoSocial: '',
      nomeFantasia: '',
      cnpj: '',
      email: '',
      telefone: '',
      endereco: '',
      cidade: '',
      estado: '',
      cep: ''
    };
    this.clienteEditando = null;
  }

  salvar() {
    // ✅ MELHORADO: Validação com MatSnackBar
    if (!this.novoCliente.razaoSocial || this.novoCliente.razaoSocial.trim() === '') {
      this.snackBar.open('Razão Social é obrigatória!', 'Fechar', { duration: 3000 });
      return;
    }

    if (!this.novoCliente.cnpj || this.novoCliente.cnpj.trim() === '') {
      this.snackBar.open('CNPJ é obrigatório!', 'Fechar', { duration: 3000 });
      return;
    }

    // ✅ Validação completa de CNPJ com dígitos verificadores
    if (!this.validarCNPJ(this.novoCliente.cnpj)) {
      this.snackBar.open('CNPJ inválido! Verifique os dígitos.', 'Fechar', { duration: 4000 });
      return;
    }

    if (this.editando && this.clienteEditando) {
      this.atualizar();
    } else {
      this.criar();
    }
  }

  criar() {
    // ✅ CORRIGIDO: Usa service
    this.clienteService.create(this.novoCliente).subscribe(
      (response) => {
        this.snackBar.open('Cliente criado com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.fecharFormulario();
        this.carregarClientes(); // ✅ Atualiza lista sem reload
      },
      (error) => {
        console.error('Erro ao criar cliente:', error);
        this.snackBar.open('Erro ao criar cliente!', 'Fechar', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    );
  }

  editarCliente(cliente: any) {
    this.clienteEditando = cliente;
    this.novoCliente = {
      razaoSocial: cliente.razaoSocial,
      nomeFantasia: cliente.nomeFantasia,
      cnpj: cliente.cnpj,
      email: cliente.email,
      telefone: cliente.telefone,
      endereco: cliente.endereco,
      cidade: cliente.cidade,
      estado: cliente.estado,
      cep: cliente.cep
    };
    this.mostrarFormulario = true;
    this.editando = true;
  }

  atualizar() {
    if (!this.clienteEditando) return;
    
    // ✅ CORRIGIDO: Usa service
    const body = {
      id: this.clienteEditando.id,
      razaoSocial: this.novoCliente.razaoSocial,
      nomeFantasia: this.novoCliente.nomeFantasia,
      cnpj: this.novoCliente.cnpj,
      email: this.novoCliente.email,
      telefone: this.novoCliente.telefone,
      endereco: this.novoCliente.endereco,
      cidade: this.novoCliente.cidade,
      estado: this.novoCliente.estado,
      cep: this.novoCliente.cep
    };
    
    this.clienteService.update(this.clienteEditando.id!, body).subscribe(
      (response) => {
        this.snackBar.open('Cliente atualizado com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.fecharFormulario();
        this.carregarClientes(); // ✅ Atualiza lista sem reload
      },
      (error) => {
        console.error('Erro ao atualizar cliente:', error);
        this.snackBar.open('Erro ao atualizar cliente!', 'Fechar', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    );
  }

  excluir(id: number) {
    // ⚠️ TODO: Substituir confirm() por MatDialog
    if (confirm('Tem certeza que deseja excluir este cliente? Esta ação pode afetar ordens de serviço vinculadas.')) {
      // ✅ CORRIGIDO: Usa service
      this.clienteService.delete(id).subscribe(
        (response) => {
          this.snackBar.open('Cliente excluído com sucesso!', 'Fechar', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.carregarClientes(); // ✅ Atualiza lista sem reload
        },
        (error) => {
          console.error('Erro ao excluir cliente:', error);
          this.snackBar.open('Erro ao excluir cliente! Pode haver ordens de serviço vinculadas.', 'Fechar', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
        }
      );
    }
  }

  // ❌ Máscara de CNPJ feita manualmente (ruim)
  formatarCNPJ(cnpj: string): string {
    if (!cnpj) return '';
    return cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, '$1.$2.$3/$4-$5');
  }

  formatarCEP(cep: string): string {
    if (!cep) return '';
    return cep.replace(/^(\d{5})(\d{3})$/, '$1-$2');
  }

  formatarData(data: string): string {
    if (!data) return '';
    const d = new Date(data);
    return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`;
  }

  /**
   * Valida CNPJ com dígitos verificadores
   * Implementa o algoritmo oficial da Receita Federal
   */
  validarCNPJ(cnpj: string): boolean {
    if (!cnpj) return false;

    // Remove caracteres não numéricos
    cnpj = cnpj.replace(/[^\d]/g, '');

    // CNPJ deve ter 14 dígitos
    if (cnpj.length !== 14) return false;

    // Rejeita CNPJs com todos os dígitos iguais
    if (/^(\d)\1+$/.test(cnpj)) return false;

    // Validação do primeiro dígito verificador
    let tamanho = cnpj.length - 2;
    let numeros = cnpj.substring(0, tamanho);
    const digitos = cnpj.substring(tamanho);
    let soma = 0;
    let pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
      soma += parseInt(numeros.charAt(tamanho - i)) * pos--;
      if (pos < 2) pos = 9;
    }

    let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado !== parseInt(digitos.charAt(0))) return false;

    // Validação do segundo dígito verificador
    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
      soma += parseInt(numeros.charAt(tamanho - i)) * pos--;
      if (pos < 2) pos = 9;
    }

    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    return resultado === parseInt(digitos.charAt(1));
  }
}
