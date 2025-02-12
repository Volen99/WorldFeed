import {Validators, AbstractControl} from '@angular/forms';
import {BuildFormValidator} from './form-validator.model';

export const VIDEO_PLAYLIST_DISPLAY_NAME_VALIDATOR: BuildFormValidator = {
  VALIDATORS: [
    Validators.required,
    Validators.minLength(1),
    Validators.maxLength(120)
  ],
  MESSAGES: {
    'required': `Display name is required.`,
    'minlength': `Display name must be at least 1 character long.`,
    'maxlength': `Display name cannot be more than 120 characters long.`
  }
};

export const VIDEO_PLAYLIST_PRIVACY_VALIDATOR: BuildFormValidator = {
  VALIDATORS: [
    Validators.required
  ],
  MESSAGES: {
    'required': `Privacy is required.`
  }
};

export const VIDEO_PLAYLIST_DESCRIPTION_VALIDATOR: BuildFormValidator = {
  VALIDATORS: [
    Validators.minLength(3),
    Validators.maxLength(1000)
  ],
  MESSAGES: {
    'minlength': `Description must be at least 3 characters long.`,
    'maxlength': `Description cannot be more than 1000 characters long.`
  }
};

export const VIDEO_PLAYLIST_CHANNEL_ID_VALIDATOR: BuildFormValidator = {
  VALIDATORS: [],
  MESSAGES: {
    'required': `The channel is required when the playlist is public.`
  }
};

// export function setPlaylistChannelValidator(channelControl: AbstractControl, privacy: VideoPlaylistPrivacy) {
//   if (privacy.toString() === VideoPlaylistPrivacy.PUBLIC.toString()) {
//     channelControl.setValidators([Validators.required]);
//   } else {
//     channelControl.setValidators(null);
//   }
//
//   channelControl.markAsDirty();
//   channelControl.updateValueAndValidity();
// }
