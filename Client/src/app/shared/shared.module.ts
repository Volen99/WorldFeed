import {NgModule} from '@angular/core';
import {RouterModule} from "@angular/router";
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from "@fortawesome/angular-fontawesome";

import {UserQueryParameterGeneratorService} from "./services/user-query-parameter-generator.service";
import {PostsModule} from "./posts/posts.module";
import {ShareButtonComponent} from "../share-button/share-button.component";
import {NbButtonModule} from "../sharebook-nebular/theme/components/button/button.module";
import {VideoListHeaderComponent} from "./post-miniature/video-list-header.component";
import {NbTooltipModule} from "../sharebook-nebular/theme/components/tooltip/tooltip.module";
import {RelationshipsService} from "./shared-main/relationships/relationships.service";
import {RelationshipsApi} from "./shared-main/relationships/backend/relationships.api";
import {SurveyService} from "./services/survey.service";
import { ListUsersComponent } from './list-users/list-users.component';
import {NbCardModule} from "../sharebook-nebular/theme/components/card/card.module";
import {UserFollowModule} from "./user-follow/user-follow.module";
import {SharedMainModule} from "./shared-main/shared-main.module";

@NgModule({
  declarations: [ShareButtonComponent, VideoListHeaderComponent, ListUsersComponent],
  imports: [
    CommonModule,
    RouterModule,
    PostsModule,
    NbButtonModule,
    NbTooltipModule,
    FontAwesomeModule,
    NbCardModule,
    UserFollowModule,
    SharedMainModule,
  ],
  exports: [
    ShareButtonComponent,
    ListUsersComponent,
  ],
  providers: [
    UserQueryParameterGeneratorService,
    RelationshipsService,
    RelationshipsApi,
    SurveyService,
  ]
})
export class SharedModule {
}
