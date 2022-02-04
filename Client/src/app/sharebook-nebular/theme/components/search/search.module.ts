import { NgModule } from '@angular/core';

import { NbSharedModule } from '../shared/shared.module';
import { NbOverlayModule } from '../cdk/overlay/overlay.module';
import { NbIconModule } from '../icon/icon.module';
import { NbButtonModule } from '../button/button.module';

import { NbSearchComponent, NbSearchFieldComponent } from './search.component';
import { NbSearchService } from './search.service';
import {NbActionsModule} from "../actions/actions.module";
import {NbCardModule} from "../card/card.module";
import {ComponentsModule} from "../../../../components/components.module";
import {FontAwesomeModule} from "@fortawesome/angular-fontawesome";


@NgModule({
    imports: [
        NbSharedModule,
        NbOverlayModule,
        NbIconModule,
        NbButtonModule,
        NbActionsModule,
        NbCardModule,
        ComponentsModule,
        FontAwesomeModule,
    ],
    declarations: [
        NbSearchComponent,
        NbSearchFieldComponent,
    ],
    exports: [
        NbSearchComponent,
        NbSearchFieldComponent,
    ],
    providers: [
        NbSearchService,
    ]
})
export class NbSearchModule {
}
