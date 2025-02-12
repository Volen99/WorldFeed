import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";

import {PostWatchComponent} from "./post-watch.component";

const routes: Routes = [
  {
    path: '',
    component: PostWatchComponent,
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class PostWatchRoutingModule {
}
