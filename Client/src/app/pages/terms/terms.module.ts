import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {TermsRoutingModule} from "./terms-routing.module";
import {TermsComponent} from "./terms.component";

@NgModule({
  declarations: [
   TermsComponent
  ],

  imports: [
    CommonModule,
    TermsRoutingModule,
  ],

  providers: [
  ],
})
export class TermsModule {
}
