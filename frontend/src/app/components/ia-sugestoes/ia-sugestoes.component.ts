import { Component, Input, Output, EventEmitter } from '@angular/core';
import { IAAssistenteService } from '../../services/ia-assistente.service';
import { MatSnackBar } from '@angular/material/snack-bar';

/**
 * Componente reutilizável para sugestões de IA
 * ✨ NOVA FEATURE - Botões inteligentes com Google Gemini
 */
@Component({
  selector: 'app-ia-sugestoes',
  templateUrl: './ia-sugestoes.component.html',
  styleUrls: ['./ia-sugestoes.component.css']
})
export class IASugestoesComponent {
  @Input() tipo: 'descricao' | 'tecnico' | 'prioridade' | 'tempo' = 'descricao';
  @Input() inputValue: string = '';
  @Input() disabled: boolean = false;
  @Output() sugestaoGerada = new EventEmitter<string>();

  gerando = false;

  constructor(
    private iaService: IAAssistenteService,
    private snackBar: MatSnackBar
  ) { }

  gerarSugestao() {
    if (!this.inputValue || this.disabled) {
      this.snackBar.open('Preencha o campo necessário primeiro', 'Fechar', { 
        duration: 3000,
        panelClass: ['error-snackbar']
      });
      return;
    }

    this.gerando = true;

    let observable;
    let mensagemSucesso = '';

    switch (this.tipo) {
      case 'descricao':
        observable = this.iaService.gerarDescricao(this.inputValue);
        mensagemSucesso = '✨ Descrição gerada pela IA!';
        break;
      case 'tecnico':
        observable = this.iaService.sugerirTecnico(this.inputValue);
        mensagemSucesso = '✨ Técnico sugerido pela IA!';
        break;
      case 'prioridade':
        observable = this.iaService.analisarPrioridade(this.inputValue);
        mensagemSucesso = '✨ Prioridade analisada pela IA!';
        break;
      case 'tempo':
        observable = this.iaService.estimarTempo(this.inputValue);
        mensagemSucesso = '✨ Tempo estimado pela IA!';
        break;
    }

    observable.subscribe(
      (response) => {
        const valor = response.descricao || response.especialidade || response.prioridade || response.tempo || '';
        this.sugestaoGerada.emit(valor);
        this.snackBar.open(mensagemSucesso, 'Fechar', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.gerando = false;
      },
      (error) => {
        console.error('Erro ao gerar sugestão com IA:', error);
        this.snackBar.open('Erro ao gerar sugestão com IA', 'Fechar', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
        this.gerando = false;
      }
    );
  }

  get textoBotao(): string {
    if (this.gerando) return 'Gerando...';
    
    switch (this.tipo) {
      case 'descricao': return 'Gerar Descrição IA';
      case 'tecnico': return 'Sugerir Técnico IA';
      case 'prioridade': return 'Analisar Prioridade IA';
      case 'tempo': return 'Estimar Tempo IA';
      default: return 'Gerar com IA';
    }
  }

  get icone(): string {
    return this.gerando ? 'hourglass_empty' : 'auto_awesome';
  }
}
