import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {ConnectComponent} from "./connect.component";

const routes: Routes = [
  {
    path: '',
    component: ConnectComponent,
    data: {
      meta: {
        title: `Connect`
      }
    },
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [ RouterModule ]
})
export class ConnectRoutingModule {
}
