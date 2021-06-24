import {Component, Input, OnInit} from '@angular/core';
import {Router} from "@angular/router";

import {NbDialogRef} from "../../../../sharebook-nebular/theme/components/dialog/dialog-ref";
import {ShowcaseDialogComponent} from "../../../../pages/modal-overlays/dialog/showcase-dialog/showcase-dialog.component";
import {PostsService} from "../../posts.service";
import {IUser} from "../../../../core/interfaces/common/users";
import {UserStore} from "../../../../core/stores/user.store";

@Component({
  selector: 'app-likes',
  templateUrl: './likes.component.html',
  styleUrls: ['./likes.component.scss']
})
export class LikesComponent implements OnInit {
  @Input() title: string;
  @Input() postId: number;

  constructor(protected ref: NbDialogRef<ShowcaseDialogComponent>, private router: Router, private postService: PostsService,
              private userStore: UserStore) { }

  ngOnInit(): void {
    this.postService.getLikers(this.postId)
      .subscribe((data) => {
        this.users = data;
    });
  }

  users: IUser[];

  dismiss() {
    this.ref.close();
  }

  userClickHandler(screenName: string) {
    this.ref.close();
    this.router.navigate([`/${screenName.substring(1)}`]);
  }

  isManageable (user: IUser) {
    return user?.id === this.userStore.getUser().id;
  }

}
