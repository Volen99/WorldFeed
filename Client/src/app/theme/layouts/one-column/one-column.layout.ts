import { Component } from '@angular/core';

@Component({
  selector: 'ngx-one-column-layout',
  styleUrls: ['./one-column.layout.scss'],
  template: `
      <nb-layout windowMode>
          <nb-layout-header fixed>
              <ngx-header></ngx-header>
          </nb-layout-header>

          <nb-sidebar class="menu-sidebar" tag="menu-sidebar" responsive start>
              <ng-content select="nb-menu"></ng-content>
              <app-share-button></app-share-button>
          </nb-sidebar>

          <nb-layout-column>
              <ng-content select="router-outlet"></ng-content>
          </nb-layout-column>

<!--          <nb-layout-footer fixed>-->
<!--              <app-footer></app-footer>-->
<!--          </nb-layout-footer>-->
      </nb-layout>
  `,
})
export class OneColumnLayoutComponent {}
