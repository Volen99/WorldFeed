import {Validators} from '@angular/forms';
import {BuildFormValidator} from './form-validator.model';

export const LOGIN_USERNAME_VALIDATOR: BuildFormValidator = {
  VALIDATORS: [
    Validators.required
  ],
  MESSAGES: {
    'required': `Username is required.`
  }
};

export const LOGIN_PASSWORD_VALIDATOR: BuildFormValidator = {
  VALIDATORS: [
    Validators.required
  ],
  MESSAGES: {
    'required': `Password is required.`
  }
};
