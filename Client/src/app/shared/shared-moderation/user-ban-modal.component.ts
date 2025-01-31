import {Component, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {NgbModalRef} from '@ng-bootstrap/ng-bootstrap/modal/modal-ref';
import {FormReactive} from "../shared-forms/form-reactive";
import {IUser} from "../../core/interfaces/common/users";
import {FormValidatorService} from "../shared-forms/form-validator.service";
import {USER_BAN_REASON_VALIDATOR} from "../shared-forms/form-validators/user-validators";

import {
  faTimes,
} from '@fortawesome/pro-light-svg-icons';
import {UsersService} from "../../core/backend/common/services/users.service";
import {NbToastrService} from "../../sharebook-nebular/theme/components/toastr/toastr.service";

@Component({
  selector: 'app-user-ban-modal',
  templateUrl: './user-ban-modal.component.html',
  styleUrls: ['./user-ban-modal.component.scss']
})
export class UserBanModalComponent extends FormReactive implements OnInit {
  @ViewChild('modal', {static: true}) modal: NgbModal;
  @Output() userBanned = new EventEmitter<IUser | IUser[]>();

  private usersToBan: IUser | IUser[];
  private openedModal: NgbModalRef;

  constructor(
    protected formValidatorService: FormValidatorService,
    private modalService: NgbModal,
    private notifier: NbToastrService,
    private userService: UsersService
  ) {
    super();
  }

  ngOnInit() {
    this.buildForm({
      reason: USER_BAN_REASON_VALIDATOR
    });
  }

  openModal(user: IUser | IUser[]) {
    this.usersToBan = user;
    this.openedModal = this.modalService.open(this.modal, {centered: true});
  }

  hide() {
    this.usersToBan = undefined;
    this.openedModal.close();
  }

  async banUser() {
    const reason = this.form.value['reason'] || undefined;

    this.userService.banUsers(this.usersToBan, reason)
      .subscribe(
        () => {
          const message = Array.isArray(this.usersToBan)
            ? `${this.usersToBan.length} users banned.`
            : `User ${this.usersToBan.screenName} banned.`;

          this.notifier.success(message, 'Success');

          this.userBanned.emit(this.usersToBan);
          this.hide();
        },

        err => this.notifier.danger(err.message, 'Error')
      );
  }

}
