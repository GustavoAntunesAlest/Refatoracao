import { Component } from '@angular/core';

// ❌ PROBLEMA: Navegação manual sem Angular Router
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'LegacyProcs - Sistema de Ordens de Serviço';
  
  // ❌ Flag para controlar tela ativa (deveria usar router)
  telaAtiva: string = 'os';
  
  mudarTela(tela: string) {
    this.telaAtiva = tela;
  }
}
