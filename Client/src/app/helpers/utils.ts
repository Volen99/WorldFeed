import {DatePipe} from '@angular/common';
import {HttpErrorResponse} from "@angular/common/http";

import {environment} from '../../environments/environment';
import {HttpStatusCode} from "../shared/core-utils/miscs";
import {NbToastrService} from "../sharebook-nebular/theme/components/toastr/toastr.service";

export type SbNullableInput = string | null | undefined;
export type SbBooleanInput = boolean | SbNullableInput;

// Thanks: https://stackoverflow.com/questions/901115/how-can-i-get-query-string-values-in-javascript ♥
function getParameterByName(name: string, url: string) {
  if (!url) {
    url = window.location.href;
  }
  name = name.replace(/[\[\]]/g, '\\$&');

  const regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)');
  const results = regex.exec(url);

  if (!results) {
    return null;
  }
  if (!results[2]) {
    return '';
  }

  return decodeURIComponent(results[2].replace(/\+/g, ' '));
}


function getAbsoluteAPIUrl() {
  let absoluteAPIUrl = environment.apiUrl;              // : environment.hmr === true

  if (!absoluteAPIUrl) {
    // The API is on the same domain
    absoluteAPIUrl = window.location.origin;
  }

  return absoluteAPIUrl;
}

function getAbsoluteEmbedUrl() {
  let absoluteEmbedUrl = environment.apiUrl;
  if (!absoluteEmbedUrl) {
    // The Embed is on the same domain
    absoluteEmbedUrl = window.location.origin;
  }

  return absoluteEmbedUrl;
}

const datePipe = new DatePipe('en');

function dateToHuman(date: string) {
  return datePipe.transform(date, 'medium');
}

function durationToString(duration: number) {
  const hours = Math.floor(duration / 3600);
  const minutes = Math.floor((duration % 3600) / 60);
  const seconds = duration % 60;

  const minutesPadding = minutes >= 10 ? '' : '0';
  const secondsPadding = seconds >= 10 ? '' : '0';
  const displayedHours = hours > 0 ? hours.toString() + ':' : '';

  return (
    displayedHours + minutesPadding + minutes.toString() + ':' + secondsPadding + seconds.toString()
  ).replace(/^0/, '');
}

function immutableAssign<A, B>(target: A, source: B) {
  return Object.assign({}, target, source);
}

// Thanks: https://gist.github.com/ghinda/8442a57f22099bdb2e34 ♥
function objectToFormData(obj: any, form?: FormData, namespace?: string) {
  const fd = form || new FormData();
  let formKey;

  for (const key of Object.keys(obj)) {
    if (namespace) {
      formKey = `${namespace}[${key}]`;
    } else {
      formKey = key;
    }

    if (obj[key] === undefined) {
      continue;
    }

    if (Array.isArray(obj[key]) && obj[key].length === 0) {
      fd.append(key, null);
      continue;
    }

    if (obj[key] !== null && typeof obj[key] === 'object' && !(obj[key] instanceof File)) {
      objectToFormData(obj[key], fd, formKey);
    } else {
      fd.append(formKey, obj[key]);
    }
  }

  return fd;
}

function objectLineFeedToHtml(obj: any, keyToNormalize: string) {
  return immutableAssign(obj, {
    [keyToNormalize]: lineFeedToHtml(obj[keyToNormalize])
  });
}

function lineFeedToHtml(text: string) {
  if (!text) {
    return text;
  }

  return text.replace(/\r?\n|\r/g, '<br />');
}

function removeElementFromArray<T>(arr: T[], elem: T) {
  const index = arr.indexOf(elem);
  if (index !== -1) {
    arr.splice(index, 1);
  }
}

function sortBy(obj: any[], key1: string, key2?: string) {
  return obj.sort((a, b) => {
    const elem1 = key2 ? a[key1][key2] : a[key1];
    const elem2 = key2 ? b[key1][key2] : b[key1];

    if (elem1 < elem2) {
      return -1;
    }
    if (elem1 === elem2) {
      return 0;
    }
    return 1;
  });
}

function scrollToTop(behavior: 'auto' | 'smooth' = 'auto') {
  window.scrollTo({
    left: 0,
    top: 0,
    behavior
  });
}

function isInViewport(el: HTMLElement) {
  const bounding = el.getBoundingClientRect();
  return (
    bounding.top >= 0 &&
    bounding.left >= 0 &&
    bounding.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
    bounding.right <= (window.innerWidth || document.documentElement.clientWidth)
  );
}

function isXPercentInViewport(el: HTMLElement, percentVisible: number) {
  const rect = el.getBoundingClientRect();
  const windowHeight = (window.innerHeight || document.documentElement.clientHeight);

  return !(
    Math.floor(100 - (((rect.top >= 0 ? 0 : rect.top) / +-(rect.height / 1)) * 100)) < percentVisible ||
    Math.floor(100 - ((rect.bottom - windowHeight) / rect.height) * 100) < percentVisible
  );
}

export function convertToBoolProperty(val: any): boolean {
  if (typeof val === 'string') {
    val = val.toLowerCase().trim();

    return (val === 'true' || val === '');
  }

  return !!val;
}

function genericUploadErrorHandler(parameters: {
  err: Pick<HttpErrorResponse, 'message' | 'status' | 'headers'>
  name: string
  notifier: NbToastrService
  sticky?: boolean
}) {
  const {err, name, notifier, sticky} = {sticky: false, ...parameters};
  const title = `The upload failed`;
  let message = err.message;

  if (err instanceof ErrorEvent) { // network error
    message = `The connection was interrupted`;
    notifier.danger(message, title, null); // sticky
  } else if (err.status === HttpStatusCode.INTERNAL_SERVER_ERROR_500) {
    message = `The server encountered an error`;
    notifier.danger(message, title, null);
  } else if (err.status === HttpStatusCode.REQUEST_TIMEOUT_408) {
    message = `Your ${name} file couldn't be transferred before the set timeout (usually 10min)`;
    notifier.danger(message, title, null);
  } else if (err.status === HttpStatusCode.PAYLOAD_TOO_LARGE_413) {
    const maxFileSize = err.headers?.get('X-File-Maximum-Size') || '8G';
    message = `Your ${name} file was too large (max. size: ${maxFileSize})`;
    notifier.danger(message, title, null);
  } else {
    notifier.danger(err.message, title);
  }

  return message;
}

function uploadErrorHandler(parameters: {
  err: HttpErrorResponse
  name: string
  notifier: NbToastrService
  sticky?: boolean
}) {
  const {err, name, notifier, sticky} = {sticky: false, ...parameters};
  const title = `The upload failed`;
  let message = err.message;

  if (err instanceof ErrorEvent) { // network error
    message = `The connection was interrupted`;
    notifier.danger(message, title, null); // sticky
  } else if (err.status === HttpStatusCode.REQUEST_TIMEOUT_408) {
    message = `Your ${name} file couldn't be transferred before the set timeout (usually 10min)`;
    notifier.danger(message, title, null);
  } else if (err.status === HttpStatusCode.PAYLOAD_TOO_LARGE_413) {
    const maxFileSize = err.headers?.get('X-File-Maximum-Size') || '8G';
    message = `Your ${name} file was too large (max. size: ${maxFileSize})`;
    notifier.danger(message, title, null);
  } else {
    notifier.danger(err.message, title);
  }

  return message;
}

export {
  sortBy,
  durationToString,
  lineFeedToHtml,
  getParameterByName,
  getAbsoluteAPIUrl,
  dateToHuman,
  immutableAssign,
  objectToFormData,
  getAbsoluteEmbedUrl,
  objectLineFeedToHtml,
  removeElementFromArray,
  scrollToTop,
  isInViewport,
  isXPercentInViewport,
  uploadErrorHandler,
  genericUploadErrorHandler,
};
