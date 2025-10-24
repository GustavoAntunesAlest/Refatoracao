import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OrdemServicoService, OrdemServico, CreateOrdemServicoDto } from '../services/ordem-servico.service';

/**
 * Componente para gerenciar Ordens de Serviço
 * ✅ REFATORADO: Agora usa OrdemServicoService (SRP)
 */
@Component({
  selector: 'app-ordem-servico',
  templateUrl: './ordem-servico.component.html',
  styleUrls: ['./ordem-servico.component.css']
})
export class OrdemServicoComponent implements OnInit {
  // ✅ CORRIGIDO: Tipagem forte com interface
  ordens: OrdemServico[] = [];
  
  // Flags de controle
  mostrarFormulario = false;
  editando = false;
  
  // ✅ CORRIGIDO: Modelo tipado
  novaOrdem: CreateOrdemServicoDto = {
    titulo: '',
    descricao: '',
    tecnico: ''
  };
  
  ordemEditando: OrdemServico | null = null;

  // ✅ CORRIGIDO: Usa service ao invés de HttpClient direto
  constructor(
    private ordemServicoService: OrdemServicoService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.carregarOrdens();
  }

  carregarOrdens() {
    // ✅ CORRIGIDO: Usa service
    this.ordemServicoService.getAll().subscribe(
      (data) => {
        this.ordens = data;
      },
      (error) => {
        console.error('Erro ao carregar ordens:', error);
        this.snackBar.open('Erro ao carregar ordens de serviço!', 'Fechar', {
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
    this.novaOrdem = {
      titulo: '',
      descricao: '',
      tecnico: ''
    };
    this.ordemEditando = null;
  }

  salvar() {
    // ✅ MELHORADO: Validação com MatSnackBar
    if (!this.novaOrdem.titulo || this.novaOrdem.titulo.trim() === '') {
      this.snackBar.open('Título é obrigatório!', 'Fechar', { duration: 3000 });
      return;
    }

    if (!this.novaOrdem.tecnico || this.novaOrdem.tecnico.trim() === '') {
      this.snackBar.open('Técnico é obrigatório!', 'Fechar', { duration: 3000 });
      return;
    }

    if (this.editando && this.ordemEditando) {
      this.atualizar();
    } else {
      this.criar();
    }
  }

  criar() {
    // ✅ CORRIGIDO: Usa service
    this.ordemServicoService.create(this.novaOrdem).subscribe(
      (response) => {
        this.snackBar.open('Ordem de serviço criada com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.fecharFormulario();
        this.carregarOrdens();
      },
      (error) => {
        console.error('Erro ao criar ordem:', error);
        this.snackBar.open('Erro ao criar ordem de serviço!', 'Fechar', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    );
  }

  editarOrdem(ordem: OrdemServico) {
    this.ordemEditando = ordem;
    this.novaOrdem = {
      titulo: ordem.titulo,
      descricao: ordem.descricao,
      tecnico: ordem.tecnico
    };
    this.mostrarFormulario = true;
    this.editando = true;
  }

  atualizar() {
    if (!this.ordemEditando) return;
    
    // ✅ CORRIGIDO: Usa service e envia objeto completo do tipo OrdemServico
    const body: OrdemServico = {
      id: this.ordemEditando.id!,
      titulo: this.novaOrdem.titulo,
      descricao: this.novaOrdem.descricao,
      tecnico: this.novaOrdem.tecnico,
      status: this.ordemEditando.status,
      dataCriacao: this.ordemEditando.dataCriacao
    };
    
    this.ordemServicoService.update(this.ordemEditando.id!, body).subscribe(
      (response) => {
        this.snackBar.open('Ordem de serviço atualizada com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.fecharFormulario();
        this.carregarOrdens();
      },
      (error) => {
        console.error('Erro ao atualizar ordem:', error);
        this.snackBar.open('Erro ao atualizar ordem de serviço!', 'Fechar', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    );
  }

  alterarStatus(ordem: OrdemServico, novoStatus: string) {
    // ✅ CORRIGIDO: Usa service e envia objeto completo
    const ordemAtualizada = { ...ordem, status: novoStatus };
    this.ordemServicoService.update(ordem.id!, ordemAtualizada).subscribe(
      (response) => {
        this.snackBar.open('Status alterado com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.carregarOrdens();
      },
      (error) => {
        console.error('Erro ao alterar status:', error);
        this.snackBar.open('Erro ao alterar status!', 'Fechar', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    );
  }

  excluir(id: number) {
    // ⚠️ TODO: Substituir confirm() por MatDialog
    if (confirm('Tem certeza que deseja excluir esta ordem de serviço?')) {
      // ✅ CORRIGIDO: Usa service
      this.ordemServicoService.delete(id).subscribe(
        (response) => {
          this.snackBar.open('Ordem de serviço excluída com sucesso!', 'Fechar', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.carregarOrdens();
        },
        (error) => {
          console.error('Erro ao excluir ordem:', error);
          this.snackBar.open('Erro ao excluir ordem de serviço!', 'Fechar', {
            duration: 3000,
            panelClass: ['error-snackbar']
          });
        }
      );
    }
  }

  // ⚠️ TODO: Criar pipe para lógica de apresentação
  getStatusClass(status: string): string {
    switch(status) {
      case 'Aberta':
        return 'status-aberta';
      case 'Pendente':
        return 'status-pendente';
      case 'Em Andamento':
        return 'status-andamento';
      case 'Concluída':
        return 'status-concluida';
      default:
        return '';
    }
  }

  formatarData(data: string): string {
    // ❌ Formatação manual (deveria usar DatePipe do Angular)
    if (!data) return '';
    const d = new Date(data);
    return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`;
  }
}
