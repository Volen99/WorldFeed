import {Injectable} from '@angular/core';
import {HttpParams} from "@angular/common/http";
import {from, of, Observable} from 'rxjs';
import {catchError, concatMap, filter, first, map, shareReplay, throttleTime, toArray} from 'rxjs/operators';
import {SortMeta} from "primeng/api";
import {DataSource} from 'ng2-smart-table/lib/lib/data-source/data-source';

import {UsersApi} from '../api/users.api';
import {UserData, IUser} from '../../../interfaces/common/users';
import {RestExtractor} from "../../../rest/rest-extractor";
import {RestService} from "../../../rest/rest.service";
import {LocalStorageService, SessionStorageService} from "../../../wrappers/storage.service";
import {UserUpdate} from "../../../../shared/models/users/user-update.model";
import {UserCreate} from "../../../../shared/models/users/user-create.model";
import {UserUpdateMe} from "../../../../shared/models/users/user-update-me.model";
import {RestPagination} from "../../../rest/rest-pagination";
import {ResultList} from "../../../../shared/models";
import {User} from "../../../../shared/shared-main/user/user.model";
import {UserRole} from "../../../../shared/models/users/user-role";
import {UserStore} from "../../../stores/user.store";
import {UserLocalStorageKeys} from "../../../../../root-helpers/users/user-local-storage-keys";
import {ComponentPaginationLight} from "../../../rest/component-pagination.model";

@Injectable()
export class UsersService extends UserData {
  static extractUsers(result: ResultList<User>) {
    const users: User[] = [];

    for (const userJSON of result.data) {
      users.push(new User(userJSON));
    }

    return {data: users, total: result.total};
  }

  private userCache: { [id: number]: Observable<IUser> } = {};
  private themes: string[] = [];
  private themeFromLocalStorage: string;

  get gridDataSource(): DataSource {
    return this.api.usersDataSource;
  }

  get(id: number): Observable<IUser> {
    return this.api.get(id);
  }

  constructor(private restExtractor: RestExtractor,
              private restService: RestService,
              private localStorageService: LocalStorageService,
              private sessionStorageService: SessionStorageService,
              private userStore: UserStore,
              private api: UsersApi) {
    super();
  }

  list(pageNumber: number = 1, pageSize: number = 10): Observable<IUser[]> {
    return this.api.list(pageNumber, pageSize);
  }

  getCurrentUser(): Observable<IUser> {
    return this.api.getCurrent()
      .pipe(
        map(u => {
          if (u && !u.setting) {
            u.setting = {};
          }
          return u;
        }));
  }

  create(user: any): Observable<IUser> {
    return this.api.add(user);
  }

  update(user: any): Observable<IUser> {
    return this.api.update(user);
  }

  updateCurrent(user: any): Observable<IUser> {
    return this.api.updateCurrent(user);
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(id);
  }

  /* ### Admin methods ### */

  addUser(userCreate: UserCreate) {
    return this.api.add(userCreate)
      .pipe(
        map(this.restExtractor.extractDataBool),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  updateUser(userId: number, userUpdate: UserUpdate) {
    return this.api.updateUser(userId, userUpdate)
      .pipe(
        map(this.restExtractor.extractDataBool),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  updateUsers(users: IUser[], userUpdate: UserUpdate) {
    return from(users)
      .pipe(
        concatMap(u => this.api.updateUsers(u.id, userUpdate)),
        toArray(),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  removeUser(usersArg: IUser | IUser[]) {
    const users = Array.isArray(usersArg) ? usersArg : [usersArg];

    return from(users)
      .pipe(
        concatMap(u => this.api.delete(u.id)),
        toArray(),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  banUsers(usersArg: IUser | IUser[], reason?: string) {
    const body = reason ? {reason} : {};
    const users = Array.isArray(usersArg) ? usersArg : [usersArg];

    return from(users)
      .pipe(
        concatMap(u => this.api.ban(u.id + '/block', body)),
        toArray(),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  unbanUsers(usersArg: IUser | IUser[]) {
    const users = Array.isArray(usersArg) ? usersArg : [usersArg];

    return from(users)
      .pipe(
        concatMap(u => this.api.unban(u.id + '/unblock', {})),
        toArray(),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  getUserWithCache(userId: number) {
    if (!this.userCache[userId]) {
      this.userCache[userId] = this.getUser(userId).pipe(shareReplay());
    }

    return this.userCache[userId];
  }

  getUser(userId: number, withStats: boolean = false) {
    const params = new HttpParams().append('withStats', withStats + '');
    return this.api.getUser(userId, params, withStats)
      .pipe(catchError(err => this.restExtractor.handleError(err)));
  }

  getAnonymousUser() {
    return new User({
      // local storage keys
      theme: this.localStorageService.getItem(UserLocalStorageKeys.THEME) || 'default',
    });
  }

  getUsers(parameters: {
    pagination: ComponentPaginationLight
  }) {
    const {pagination} = parameters;

    const restPagination = this.restService.componentPaginationToRestPagination(pagination);

    let params = new HttpParams();
    params = this.restService.addRestGetParams(params, restPagination);

    return this.api.getUsers('who_to_follow', params);
  }

  getUsersForAdmin(parameters: {
    pagination: RestPagination
    sort: SortMeta
    search?: string
  }): Observable<ResultList<User>> {
    const {pagination, sort, search} = parameters;

    let params = new HttpParams();
    params = this.restService.addRestGetParams(params, pagination, sort);

    if (search) {
      const filters = this.restService.parseQueryStringFilter(search, {
        blocked: {
          prefix: 'banned:',
          isBoolean: true
        }
      });

      params = this.restService.addObjectParams(params, filters);
    }

    // @ts-ignore
    return this.api.getUsersForAdmin('list', {params}) // <ResultList<User>>
      .pipe(
        map(res => this.restExtractor.convertResultListDateToHuman(res)),
        map(res => this.restExtractor.applyToResultListData(res, this.formatUser.bind(this))),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  changeEmail(password: string, newEmail: string) {
    const url = 'me/edit-email';
    const body: UserUpdateMe = {
      currentPassword: password,
      email: newEmail
    };

    return this.api.changeEmail(url, body);
    // .pipe(
    //   map(this.restExtractor.extractDataBool),
    //   catchError(err => this.restExtractor.handleError(err))
    // );
  }

  changePassword(currentPassword: string, newPassword: string, confirmPassword: string) {
    const url = 'reset-pass';
    const body = {
      oldPassword: currentPassword,
      password: newPassword,
      confirmPassword: confirmPassword,
    };

    return this.api.changePassword(url, body)
      .pipe(
        map(this.restExtractor.extractDataBool),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  changeAvatar(avatarForm: FormData) {
    const url = 'avatar';

    return this.api.changeAvatar(url, avatarForm)
      .pipe(catchError(err => this.restExtractor.handleError(err)));
  }

  changeBanner(bannerForm: FormData) {
    const url = 'banner';

    return this.api.changeAvatar(url, bannerForm)
      .pipe(catchError(err => this.restExtractor.handleError(err)));
  }

  deleteMe() {
    const url = 'me/delete';

    return this.api.deleteMe(url)
      .pipe(
        map(this.restExtractor.extractDataBool),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  deleteAvatar() {
    const url = 'me/avatar';

    return this.api.deleteAvatar(url)
      .pipe(
        map(this.restExtractor.extractDataBool),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  deleteBanner() {
    const url = 'me/banner';

    return this.api.deleteBanner(url)
      .pipe(
        map(this.restExtractor.extractDataBool),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  updateMyProfile(profile: UserUpdateMe) {
    const url = 'me';

    return this.api.updateMyProfile(url, profile)
      .pipe(
        map(this.restExtractor.extractDataBool),
        catchError(err => this.restExtractor.handleError(err))
      );
  }

  updateCurrentPersonal(body: any): Observable<IUser> {
    return this.api.updateCurrentPersonal(body);
  }

  getYourBirthday(userId: number) {
    return this.api.getYourBirthday('birthday', userId);
  }

  getAnonymousOrLoggedUser() {
    if (!this.userStore.isLoggedIn()) {
      return of(this.getAnonymousUser());
    }

    return this.userStore.onUserStateChange()
      .pipe(
        first(),
        map(() => this.userStore.getUser())
      );
  }

  updateUserAsAdmin(body: any) {
    return this.api.updateUserAsAdmin(body);
  }

  getAnonymousUserTheme() {
    if (this.themeFromLocalStorage) {
      return this.themeFromLocalStorage;
    }

    const theme = this.getAnonymousUser().theme;

    if (theme !== 'default') {
      return theme;
    }

    const instanceTheme = 'default';
    if (instanceTheme !== 'default') {
      return instanceTheme;
    }

    // Default to dark theme if available and wanted by the user
    if (
      this.themes.find(t => t === 'cosmic') // && window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches
    ) {
      return 'cosmic';
    }

    return instanceTheme;
  }

  private formatUser(user: IUser) {
    const roleLabels: { [id in UserRole]: string } = {
      [UserRole.REGISTERED]: `Registered`,
      [UserRole.ADMINISTRATOR]: `Administrator`,
      [UserRole.MODERATOR]: `Moderator`
    };

    return Object.assign(user, {
      roleLabel: roleLabels[user.roles[0]], // TODO: fix role[0]
    });
  }

  updateMyAnonymousProfile(profile: UserUpdateMe) {
    const localStorageKeys: { [id in keyof UserUpdateMe]: string } = {
      theme: UserLocalStorageKeys.THEME,
    };

    const obj = Object.keys(localStorageKeys)
      .filter(key => key in profile)
      .map(key => ([localStorageKeys[key], profile[key]]));

    for (const [key, value] of obj) {
      try {
        if (value === undefined) {
          this.localStorageService.removeItem(key);
          continue;
        }

        const localStorageValue = typeof value === 'string'
          ? value
          : JSON.stringify(value);

        this.localStorageService.setItem(key, localStorageValue);
      } catch (err) {
        console.error(`Cannot set ${key}->${value} in localStorage. Likely due to a value impossible to stringify.`, err);
      }
    }
  }

  listenAnonymousUpdate() {
    return this.localStorageService.watch([
      UserLocalStorageKeys.THEME,
    ]).pipe(
      throttleTime(200),
      filter(() => this.userStore.isLoggedIn() !== true),
      map(() => this.getAnonymousUser())
    );
  }

}
