import {Observable, of} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {Inject, Injectable, LOCALE_ID} from '@angular/core';
import {HTMLServerConfig, ServerConfig} from "../../shared/models/server/server-config.model";

@Injectable()
export class ServerService {
  private configObservable: Observable<ServerConfig>;
  private htmlConfig: HTMLServerConfig;

  private configReset = false;

  private configLoaded = false;
  private config: ServerConfig = {
    instance: {
      name: 'Chessbook',
      shortDescription: 'Chessbook, a social media for chess players  ' +
        'using C# and Angular.',
      isNSFW: false,
      defaultNSFWPolicy: 'do_not_list' as 'do_not_list',
      defaultClientRoute: '',
      customizations: {
        javascript: '',
        css: ''
      }
    },
    plugin: {
      registered: [],
      registeredExternalAuths: [],
      registeredIdAndPassAuths: []
    },
    theme: {
      registered: [],
      default: 'default'
    },
    email: {
      enabled: false
    },
    contactForm: {
      enabled: false
    },
    serverVersion: 'Unknown',
    signup: {
      allowed: true,
      allowedForCurrentIP: false,
      requiresEmailVerification: false
    },
    transcoding: {
      profile: 'default',
      availableProfiles: ['default'],
      enabledResolutions: [],
      hls: {
        enabled: false
      },
      webtorrent: {
        enabled: true
      }
    },
    live: {
      enabled: false,
      allowReplay: true,
      maxDuration: null,
      maxInstanceLives: -1,
      maxUserLives: -1,
      transcoding: {
        enabled: false,
        profile: 'default',
        availableProfiles: ['default'],
        enabledResolutions: []
      },
      rtmp: {
        port: 1935
      }
    },
    avatar: {
      file: {
        size: {max: 0},
        extensions: []
      }
    },
    banner: {
      file: {
        size: {max: 0},
        extensions: []
      }
    },
    video: {
      image: {
        size: {max: 0},
        extensions: []
      },
      file: {
        extensions: []
      }
    },
    videoCaption: {
      file: {
        size: {max: 0},
        extensions: []
      }
    },
    user: {
      videoQuota: -1,
      videoQuotaDaily: -1
    },
    import: {
      videos: {
        http: {
          enabled: false
        },
        torrent: {
          enabled: false
        }
      }
    },
    trending: {
      videos: {
        intervalDays: 0,
        algorithms: {
          enabled: ['best', 'hot', 'most-viewed', 'most-liked'],
          default: 'most-viewed'
        }
      }
    },
    autoBlacklist: {
      videos: {
        ofUsers: {
          enabled: false
        }
      }
    },
    tracker: {
      enabled: true
    },
    followings: {
      instance: {
        autoFollowIndex: {
          indexUrl: 'https://instances.joinpeertube.org'
        }
      }
    },
    broadcastMessage: {
      enabled: false,
      message: '',
      level: 'info',
      dismissable: false
    },
    search: {
      remoteUri: {
        users: true,
        anonymous: false
      },
      searchIndex: {
        enabled: false,
        url: '',
        disableLocalSearch: false,
        isDefaultSearch: false
      }
    }
  };

  constructor(
    private http: HttpClient,
    @Inject(LOCALE_ID) private localeId: string
  ) {
    this.loadConfigLocally();
  }

  getServerVersionAndCommit() {
    const serverVersion = this.config.serverVersion;
    const commit = this.config.serverCommit || '';

    let result = serverVersion;
    if (commit) result += '...' + commit;

    return result;
  }

  resetConfig() {
    this.configLoaded = false;
    this.configReset = true;

    // Notify config update
    this.getConfig().subscribe(() => {
      // empty, to fire a reset config event
    });
  }

  getConfig() {
    if (this.configLoaded) return of(this.config);

    return this.configObservable;
  }

  getHTMLConfig () {
    this.htmlConfig = {
      autoBlacklist: {videos: {ofUsers: {enabled: false}}},
      avatar: {file: {extensions: [], size: {max: 0}}},
      banner: {file: {extensions: [], size: {max: 0}}},
      broadcastMessage: {dismissable: false, enabled: false, level: undefined, message: ""},
      contactForm: {enabled: false},
      email: {enabled: false},
      followings: {instance: {autoFollowIndex: {indexUrl: ""}}},
      import: {videos: {http: {enabled: false}, torrent: {enabled: false}}},
      instance: {
        customizations: {css: "", javascript: ""},
        defaultClientRoute: "",
        defaultNSFWPolicy: undefined,
        isNSFW: false,
        name: "",
        shortDescription: ""
      },
      live: {
        allowReplay: false,
        enabled: false,
        maxDuration: 0,
        maxInstanceLives: 0,
        maxUserLives: 0,
        rtmp: {port: 0},
        transcoding: {availableProfiles: [], enabled: false, enabledResolutions: [], profile: ""}
      },
      plugin: {registered: [], registeredExternalAuths: [], registeredIdAndPassAuths: []},
      search: {
        remoteUri: {
          users: true,
          anonymous: false,
        },
        searchIndex: {
          enabled: false,
          url: '',
          disableLocalSearch: false,
          isDefaultSearch: false,
        }
      },
      serverVersion: "",
      theme: {default: "", registered: []},
      tracker: {enabled: false},
      transcoding: {
        availableProfiles: [],
        enabledResolutions: [],
        hls: {enabled: false},
        profile: "",
        webtorrent: {enabled: false}
      },
      trending: {videos: {algorithms: {default: "", enabled: []}, intervalDays: 0}},
      user: {videoQuota: 0, videoQuotaDaily: 0},
      video: {file: {extensions: []}, image: {extensions: [], size: {max: 0}}},
      videoCaption: {file: {extensions: [], size: {max: 0}}}

    };

    return this.htmlConfig;
  }

  getTmpConfig() {
    return this.config;
  }

  private loadConfigLocally() {
    const configString = window['PeerTubeServerConfig'];
    if (!configString) return;

    try {
      const parsed = JSON.parse(configString);
      Object.assign(this.config, parsed);
    } catch (err) {
      console.error('Cannot parse config saved in from index.html.', err);
    }
  }
}
