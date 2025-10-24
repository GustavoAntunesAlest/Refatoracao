import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TecnicoService, Tecnico } from '../services/tecnico.service';

/**
 * Componente para gerenciar Técnicos
 * ✅ REFATORADO: Agora usa TecnicoService (SRP)
 */
@Component({
  selector: 'app-tecnico',
  templateUrl: './tecnico.component.html',
  styleUrls: ['./tecnico.component.css']
})
export class TecnicoComponent implements OnInit {
  // ✅ CORRIGIDO: Tipagem forte
  tecnicos: Tecnico[] = [];
  
  mostrarFormulario = false;
  editando = false;
  
  // ✅ CORRIGIDO: Modelo tipado
  novoTecnico: Tecnico = {
    nome: '',
    email: '',
    telefone: '',
    especialidade: '',
    status: 'Ativo'
  };
  
  tecnicoEditando: Tecnico | null = null;

  // ✅ CORRIGIDO: Usa service
  constructor(
    private tecnicoService: TecnicoService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.carregarTecnicos();
  }

  carregarTecnicos() {
    // ✅ CORRIGIDO: Usa service
    this.tecnicoService.getAll().subscribe(
      (data) => {
        this.tecnicos = data;
      },
      (error) => {
        console.error('Erro ao carregar técnicos:', error);
        this.snackBar.open('Erro ao carregar técnicos!', 'Fechar', {
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
    this.novoTecnico = {
      nome: '',
      email: '',
      telefone: '',
      especialidade: '',
      status: 'Ativo'
    };
    this.tecnicoEditando = null;
  }

  salvar() {
    // ✅ MELHORADO: Validação com MatSnackBar
    if (!this.novoTecnico.nome || this.novoTecnico.nome.trim() === '') {
      this.snackBar.open('Nome é obrigatório!', 'Fechar', { duration: 3000 });
      return;
    }

    if (!this.novoTecnico.especialidade) {
      this.snackBar.open('Especialidade é obrigatória!', 'Fechar', { duration: 3000 });
      return;
    }

    if (this.editando && this.tecnicoEditando) {
      this.atualizar();
    } else {
      this.criar();
    }
  }

  criar() {
    // ✅ CORRIGIDO: Usa service
    this.tecnicoService.create(this.novoTecnico).subscribe(
      (response) => {
        this.snackBar.open('Técnico criado com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.fecharFormulario();
        this.carregarTecnicos(); // ✅ Atualiza lista sem reload
      },
      (error) => {
        console.error('Erro ao criar técnico:', error);
        this.snackBar.open('Erro ao criar técnico!', 'Fechar', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    );
  }

  editarTecnico(tecnico: Tecnico) {
    this.tecnicoEditando = tecnico;
    this.novoTecnico = {
      nome: tecnico.nome,
      email: tecnico.email,
      telefone: tecnico.telefone,
      especialidade: tecnico.especialidade,
      status: tecnico.status
    };
    this.mostrarFormulario = true;
    this.editando = true;
  }

  atualizar() {
    if (!this.tecnicoEditando) return;
    
    // ✅ CORRIGIDO: Usa service
    const body: Tecnico = {
      id: this.tecnicoEditando.id,
      nome: this.novoTecnico.nome,
      email: this.novoTecnico.email,
      telefone: this.novoTecnico.telefone,
      especialidade: this.novoTecnico.especialidade,
      status: this.novoTecnico.status
    };
    
    this.tecnicoService.update(this.tecnicoEditando.id!, body).subscribe(
      (response) => {
        this.snackBar.open('Técnico atualizado com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.fecharFormulario();
        this.carregarTecnicos(); // ✅ Atualiza lista sem reload
      },
      (error) => {
        console.error('Erro ao atualizar técnico:', error);
        this.snackBar.open('Erro ao atualizar técnico!', 'Fechar', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    );
  }

  alterarStatus(tecnico: Tecnico, novoStatus: string) {
    // ✅ CORRIGIDO: Usa service
    const body = { ...tecnico, status: novoStatus };
    
    this.tecnicoService.update(tecnico.id!, body).subscribe(
      (response) => {
        this.snackBar.open('Status alterado com sucesso!', 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.carregarTecnicos(); // ✅ Atualiza lista sem reload
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
    if (confirm('Tem certeza que deseja excluir este técnico?')) {
      // ✅ CORRIGIDO: Usa service
      this.tecnicoService.delete(id).subscribe(
        (response) => {
          this.snackBar.open('Técnico excluído com sucesso!', 'Fechar', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.carregarTecnicos(); // ✅ Atualiza lista sem reload
        },
        (error) => {
          console.error('Erro ao excluir técnico:', error);
          this.snackBar.open('Erro ao excluir técnico!', 'Fechar', {
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
      case 'Ativo':
        return 'status-ativo';
      case 'Inativo':
        return 'status-inativo';
      case 'Férias':
        return 'status-ferias';
      default:
        return '';
    }
  }

  formatarData(data: string): string {
    if (!data) return '';
    const d = new Date(data);
    return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`;
  }
}
