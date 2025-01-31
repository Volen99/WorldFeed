import {Component, Input} from '@angular/core';
import {SafeResourceUrl} from '@angular/platform-browser';
import {User} from "../shared-main/user/user.model";

type ActorInput = {
  screenName: string
  avatar?: { url?: string, path: string }
  profileImageUrlHttps: string
};

export type ActorAvatarSize = '18' | '25' | '32' | '34' | '36' | '40' | '100' | '120';

@Component({
  selector: 'my-actor-avatar',
  styleUrls: ['./actor-avatar.component.scss'],
  templateUrl: './actor-avatar.component.html'
})
export class ActorAvatarComponent {
  @Input() account: ActorInput;
  @Input() channel: ActorInput;

  @Input() previewImage: SafeResourceUrl;

  @Input() size: ActorAvatarSize;

  // Use an external link
  @Input() href: string;
  // Use routerLink
  @Input() internalHref: string | any[];

  @Input() set title(value) {
    this._title = value;
  }

  private _title: string;

  get title() {
    if (this._title) return this._title;
    if (this.account) return `${this.account.screenName} (user profile)`;
    if (this.channel) return `${this.channel.screenName} (user profile)`;

    return '';
  }

  get alt() {
    if (this.account) return `Account avatar`;
    if (this.channel) return `Channel avatar`;

    return '';
  }

  getClass(type: 'avatar' | 'initial') {
    const base = ['avatar'];

    if (this.size) base.push(`avatar-${this.size}`);

    if (this.channel) base.push('channel');
    else base.push('account');

    if (type === 'initial' && this.initial) {
      base.push('initial');
      base.push(this.getColorTheme());
    }

    return base;
  }

  get defaultAvatarUrl() {
    if (this.channel) {
      return User.GET_DEFAULT_ANONYMOUS_AVATAR_URL();
    }

    return User.GET_DEFAULT_ANONYMOUS_AVATAR_URL();
  }

  get avatarUrl() {
    if (this.account) {
      return this.account.profileImageUrlHttps;
    }
    if (this.channel) {
      return this.channel.profileImageUrlHttps;  // User.GET_ACTOR_AVATAR_URL(this.channel);
    }

    return '';
  }

  get initial() {
    const name = this.account?.screenName;
    if (!name) {
      return '';
    }

    return name.slice(0, 1);
  }

  hasActor() {
    return !!this.account || !!this.channel;
  }

  private getColorTheme() {
    // Keep consistency with CSS
    const themes = {
      abc: 'blue',
      def: 'green',
      ghi: 'purple',
      jkl: 'gray',
      mno: 'yellow',
      pqr: 'orange',
      stvu: 'red',
      wxyz: 'dark-blue'
    };

    const theme = Object.keys(themes)
      .find(chars => chars.includes(this.initial));

    return themes[theme];
  }
}
