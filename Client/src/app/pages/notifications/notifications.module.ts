import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FontAwesomeModule} from "@fortawesome/angular-fontawesome";

import { SidebarColumnModule } from '../../shared/sidebar-column/sidebar-column.module';
import { NotificationsRoutingModule } from './notifications-routing.module';
import { NotificationsComponent } from './notifications.component';
import { MentionsComponent } from './mentions/mentions.component';
import {SharedMainModule} from "../../shared/shared-main/shared-main.module";
import {NbCardModule} from "../../sharebook-nebular/theme/components/card/card.module";
import {NbRouteTabsetModule} from "../../sharebook-nebular/theme/components/route-tabset/route-tabset.module";
import {ComponentsModule} from "../../components/components.module";
import {UserNotificationsComponent} from "./user-notifications/user-notifications.component";
import {NbButtonModule} from "../../sharebook-nebular/theme/components/button/button.module";

@NgModule({
  declarations: [
    NotificationsComponent,
    MentionsComponent,
    UserNotificationsComponent
  ],
  exports: [
    UserNotificationsComponent
  ],
  imports: [
    NotificationsRoutingModule,
    CommonModule,
    SidebarColumnModule,
    SharedMainModule,
    NbCardModule,
    NbRouteTabsetModule,
    ComponentsModule,
    FontAwesomeModule,
    NbButtonModule,
  ]
})
export class NotificationsModule {
}
