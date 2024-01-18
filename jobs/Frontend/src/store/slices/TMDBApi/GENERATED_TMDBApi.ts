import { TMDBApiSlice as api } from "./TMDBApiSlice";
const injectedRtkApi = api.injectEndpoints({
  endpoints: (build) => ({
    get3SearchMovie: build.query<
      Get3SearchMovieApiResponse,
      Get3SearchMovieApiArg
    >({
      query: (queryArg) => ({
        url: `/3/search/movie`,
        params: {
          query: queryArg.query,
          include_adult: queryArg.includeAdult,
          language: queryArg.language,
          primary_release_year: queryArg.primaryReleaseYear,
          page: queryArg.page,
          region: queryArg.region,
          year: queryArg.year,
        },
      }),
    }),
    get3MovieByMovieId: build.query<
      Get3MovieByMovieIdApiResponse,
      Get3MovieByMovieIdApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}`,
        params: {
          append_to_response: queryArg.appendToResponse,
          language: queryArg.language,
        },
      }),
    }),
    get3MovieByMovieIdImages: build.query<
      Get3MovieByMovieIdImagesApiResponse,
      Get3MovieByMovieIdImagesApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/images`,
        params: {
          include_image_language: queryArg.includeImageLanguage,
          language: queryArg.language,
        },
      }),
    }),
    get3MovieByMovieIdAccountStates: build.query<
      Get3MovieByMovieIdAccountStatesApiResponse,
      Get3MovieByMovieIdAccountStatesApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/account_states`,
        params: {
          session_id: queryArg.sessionId,
          guest_session_id: queryArg.guestSessionId,
        },
      }),
    }),
    get3MovieByMovieIdAlternativeTitles: build.query<
      Get3MovieByMovieIdAlternativeTitlesApiResponse,
      Get3MovieByMovieIdAlternativeTitlesApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/alternative_titles`,
        params: { country: queryArg.country },
      }),
    }),
    get3MovieByMovieIdChanges: build.query<
      Get3MovieByMovieIdChangesApiResponse,
      Get3MovieByMovieIdChangesApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/changes`,
        params: {
          end_date: queryArg.endDate,
          page: queryArg.page,
          start_date: queryArg.startDate,
        },
      }),
    }),
    get3MovieByMovieIdCredits: build.query<
      Get3MovieByMovieIdCreditsApiResponse,
      Get3MovieByMovieIdCreditsApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/credits`,
        params: { language: queryArg.language },
      }),
    }),
    get3MovieByMovieIdExternalIds: build.query<
      Get3MovieByMovieIdExternalIdsApiResponse,
      Get3MovieByMovieIdExternalIdsApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/external_ids`,
      }),
    }),
    get3MovieByMovieIdKeywords: build.query<
      Get3MovieByMovieIdKeywordsApiResponse,
      Get3MovieByMovieIdKeywordsApiArg
    >({
      query: (queryArg) => ({ url: `/3/movie/${queryArg.movieId}/keywords` }),
    }),
    get3MovieByMovieIdLists: build.query<
      Get3MovieByMovieIdListsApiResponse,
      Get3MovieByMovieIdListsApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/lists`,
        params: { language: queryArg.language, page: queryArg.page },
      }),
    }),
    get3MovieByMovieIdRecommendations: build.query<
      Get3MovieByMovieIdRecommendationsApiResponse,
      Get3MovieByMovieIdRecommendationsApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/recommendations`,
        params: { language: queryArg.language, page: queryArg.page },
      }),
    }),
    get3MovieByMovieIdReleaseDates: build.query<
      Get3MovieByMovieIdReleaseDatesApiResponse,
      Get3MovieByMovieIdReleaseDatesApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/release_dates`,
      }),
    }),
    get3MovieByMovieIdReviews: build.query<
      Get3MovieByMovieIdReviewsApiResponse,
      Get3MovieByMovieIdReviewsApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/reviews`,
        params: { language: queryArg.language, page: queryArg.page },
      }),
    }),
    get3MovieByMovieIdSimilar: build.query<
      Get3MovieByMovieIdSimilarApiResponse,
      Get3MovieByMovieIdSimilarApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/similar`,
        params: { language: queryArg.language, page: queryArg.page },
      }),
    }),
    get3MovieByMovieIdTranslations: build.query<
      Get3MovieByMovieIdTranslationsApiResponse,
      Get3MovieByMovieIdTranslationsApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/translations`,
      }),
    }),
    get3MovieByMovieIdVideos: build.query<
      Get3MovieByMovieIdVideosApiResponse,
      Get3MovieByMovieIdVideosApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/videos`,
        params: { language: queryArg.language },
      }),
    }),
    get3MovieByMovieIdWatchProviders: build.query<
      Get3MovieByMovieIdWatchProvidersApiResponse,
      Get3MovieByMovieIdWatchProvidersApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/watch/providers`,
      }),
    }),
    post3MovieByMovieIdRating: build.mutation<
      Post3MovieByMovieIdRatingApiResponse,
      Post3MovieByMovieIdRatingApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/rating`,
        method: "POST",
        body: queryArg.body,
        headers: { "Content-Type": queryArg["Content-Type"] },
        params: {
          guest_session_id: queryArg.guestSessionId,
          session_id: queryArg.sessionId,
        },
      }),
    }),
    delete3MovieByMovieIdRating: build.mutation<
      Delete3MovieByMovieIdRatingApiResponse,
      Delete3MovieByMovieIdRatingApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/${queryArg.movieId}/rating`,
        method: "DELETE",
        headers: { "Content-Type": queryArg["Content-Type"] },
        params: {
          guest_session_id: queryArg.guestSessionId,
          session_id: queryArg.sessionId,
        },
      }),
    }),
    get3MoviePopular: build.query<
      Get3MoviePopularApiResponse,
      Get3MoviePopularApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/popular`,
        params: {
          language: queryArg.language,
          page: queryArg.page,
          region: queryArg.region,
        },
      }),
    }),
    get3MovieTopRated: build.query<
      Get3MovieTopRatedApiResponse,
      Get3MovieTopRatedApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/top_rated`,
        params: {
          language: queryArg.language,
          page: queryArg.page,
          region: queryArg.region,
        },
      }),
    }),
    get3MovieUpcoming: build.query<
      Get3MovieUpcomingApiResponse,
      Get3MovieUpcomingApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/upcoming`,
        params: {
          language: queryArg.language,
          page: queryArg.page,
          region: queryArg.region,
        },
      }),
    }),
    get3MovieNowPlaying: build.query<
      Get3MovieNowPlayingApiResponse,
      Get3MovieNowPlayingApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/now_playing`,
        params: {
          language: queryArg.language,
          page: queryArg.page,
          region: queryArg.region,
        },
      }),
    }),
    get3MovieLatest: build.query<
      Get3MovieLatestApiResponse,
      Get3MovieLatestApiArg
    >({
      query: () => ({ url: `/3/movie/latest` }),
    }),
    get3MovieChanges: build.query<
      Get3MovieChangesApiResponse,
      Get3MovieChangesApiArg
    >({
      query: (queryArg) => ({
        url: `/3/movie/changes`,
        params: {
          end_date: queryArg.endDate,
          page: queryArg.page,
          start_date: queryArg.startDate,
        },
      }),
    }),
  }),
  overrideExisting: false,
});
export { injectedRtkApi as TMDBApi };
export type Get3SearchMovieApiResponse = /** status 200 200 */ {
  page?: number;
  results?: {
    adult?: boolean;
    backdrop_path?: string;
    genre_ids?: number[];
    id?: number;
    original_language?: string;
    original_title?: string;
    overview?: string;
    popularity?: number;
    poster_path?: string;
    release_date?: string;
    title?: string;
    video?: boolean;
    vote_average?: number;
    vote_count?: number;
  }[];
  total_pages?: number;
  total_results?: number;
};
export type Get3SearchMovieApiArg = {
  query: string;
  includeAdult?: boolean;
  language?: string;
  primaryReleaseYear?: string;
  page?: number;
  region?: string;
  year?: string;
};
export type Get3MovieByMovieIdApiResponse = /** status 200 200 */ {
  adult?: boolean;
  backdrop_path?: string;
  belongs_to_collection?: any;
  budget?: number;
  genres?: {
    id?: number;
    name?: string;
  }[];
  homepage?: string;
  id?: number;
  imdb_id?: string;
  original_language?: string;
  original_title?: string;
  overview?: string;
  popularity?: number;
  poster_path?: string;
  production_companies?: {
    id?: number;
    logo_path?: string;
    name?: string;
    origin_country?: string;
  }[];
  production_countries?: {
    iso_3166_1?: string;
    name?: string;
  }[];
  release_date?: string;
  revenue?: number;
  runtime?: number;
  spoken_languages?: {
    english_name?: string;
    iso_639_1?: string;
    name?: string;
  }[];
  status?: string;
  tagline?: string;
  title?: string;
  video?: boolean;
  vote_average?: number;
  vote_count?: number;
};
export type Get3MovieByMovieIdApiArg = {
  movieId: number;
  /** comma separated list of endpoints within this namespace, 20 items max */
  appendToResponse?: string;
  language?: string;
};
export type Get3MovieByMovieIdImagesApiResponse = /** status 200 200 */ {
  backdrops?: {
    aspect_ratio?: number;
    height?: number;
    iso_639_1?: any;
    file_path?: string;
    vote_average?: number;
    vote_count?: number;
    width?: number;
  }[];
  id?: number;
  logos?: {
    aspect_ratio?: number;
    height?: number;
    iso_639_1?: string;
    file_path?: string;
    vote_average?: number;
    vote_count?: number;
    width?: number;
  }[];
  posters?: {
    aspect_ratio?: number;
    height?: number;
    iso_639_1?: string;
    file_path?: string;
    vote_average?: number;
    vote_count?: number;
    width?: number;
  }[];
};
export type Get3MovieByMovieIdImagesApiArg = {
  movieId: number;
  /** specify a comma separated list of ISO-639-1 values to query, for example: `en,null` */
  includeImageLanguage?: string;
  language?: string;
};
export type Get3MovieByMovieIdAccountStatesApiResponse = /** status 200 200 */ {
  id?: number;
  favorite?: boolean;
  rated?: {
    value?: number;
  };
  watchlist?: boolean;
};
export type Get3MovieByMovieIdAccountStatesApiArg = {
  movieId: number;
  sessionId?: string;
  guestSessionId?: string;
};
export type Get3MovieByMovieIdAlternativeTitlesApiResponse =
  /** status 200 200 */ {
    id?: number;
    titles?: {
      iso_3166_1?: string;
      title?: string;
      type?: string;
    }[];
  };
export type Get3MovieByMovieIdAlternativeTitlesApiArg = {
  movieId: number;
  /** specify a ISO-3166-1 value to filter the results */
  country?: string;
};
export type Get3MovieByMovieIdChangesApiResponse = /** status 200 200 */ Blob;
export type Get3MovieByMovieIdChangesApiArg = {
  movieId: number;
  endDate?: string;
  page?: number;
  startDate?: string;
};
export type Get3MovieByMovieIdCreditsApiResponse = /** status 200 200 */ {
  id?: number;
  cast?: {
    adult?: boolean;
    gender?: number;
    id?: number;
    known_for_department?: string;
    name?: string;
    original_name?: string;
    popularity?: number;
    profile_path?: string;
    cast_id?: number;
    character?: string;
    credit_id?: string;
    order?: number;
  }[];
  crew?: {
    adult?: boolean;
    gender?: number;
    id?: number;
    known_for_department?: string;
    name?: string;
    original_name?: string;
    popularity?: number;
    profile_path?: string;
    credit_id?: string;
    department?: string;
    job?: string;
  }[];
};
export type Get3MovieByMovieIdCreditsApiArg = {
  movieId: number;
  language?: string;
};
export type Get3MovieByMovieIdExternalIdsApiResponse = /** status 200 200 */ {
  id?: number;
  imdb_id?: string;
  wikidata_id?: any;
  facebook_id?: string;
  instagram_id?: any;
  twitter_id?: any;
};
export type Get3MovieByMovieIdExternalIdsApiArg = {
  movieId: number;
};
export type Get3MovieByMovieIdKeywordsApiResponse = /** status 200 200 */ {
  id?: number;
  keywords?: {
    id?: number;
    name?: string;
  }[];
};
export type Get3MovieByMovieIdKeywordsApiArg = {
  movieId: string;
};
export type Get3MovieByMovieIdListsApiResponse = /** status 200 200 */ {
  id?: number;
  page?: number;
  results?: {
    description?: string;
    favorite_count?: number;
    id?: number;
    item_count?: number;
    iso_639_1?: string;
    list_type?: string;
    name?: string;
    poster_path?: any;
  }[];
  total_pages?: number;
  total_results?: number;
};
export type Get3MovieByMovieIdListsApiArg = {
  movieId: number;
  language?: string;
  page?: number;
};
export type Get3MovieByMovieIdRecommendationsApiResponse =
  /** status 200 200 */ {};
export type Get3MovieByMovieIdRecommendationsApiArg = {
  movieId: number;
  language?: string;
  page?: number;
};
export type Get3MovieByMovieIdReleaseDatesApiResponse = /** status 200 200 */ {
  id?: number;
  results?: {
    iso_3166_1?: string;
    release_dates?: {
      certification?: string;
      descriptors?: any;
      iso_639_1?: string;
      note?: string;
      release_date?: string;
      type?: number;
    }[];
  }[];
};
export type Get3MovieByMovieIdReleaseDatesApiArg = {
  movieId: number;
};
export type Get3MovieByMovieIdReviewsApiResponse = /** status 200 200 */ {
  id?: number;
  page?: number;
  results?: {
    author?: string;
    author_details?: {
      name?: string;
      username?: string;
      avatar_path?: string;
      rating?: any;
    };
    content?: string;
    created_at?: string;
    id?: string;
    updated_at?: string;
    url?: string;
  }[];
  total_pages?: number;
  total_results?: number;
};
export type Get3MovieByMovieIdReviewsApiArg = {
  movieId: number;
  language?: string;
  page?: number;
};
export type Get3MovieByMovieIdSimilarApiResponse = /** status 200 200 */ {
  page?: number;
  results?: {
    adult?: boolean;
    backdrop_path?: string;
    genre_ids?: number[];
    id?: number;
    original_language?: string;
    original_title?: string;
    overview?: string;
    popularity?: number;
    poster_path?: string;
    release_date?: string;
    title?: string;
    video?: boolean;
    vote_average?: number;
    vote_count?: number;
  }[];
  total_pages?: number;
  total_results?: number;
};
export type Get3MovieByMovieIdSimilarApiArg = {
  movieId: number;
  language?: string;
  page?: number;
};
export type Get3MovieByMovieIdTranslationsApiResponse = /** status 200 200 */ {
  id?: number;
  translations?: {
    iso_3166_1?: string;
    iso_639_1?: string;
    name?: string;
    english_name?: string;
    data?: {
      homepage?: string;
      overview?: string;
      runtime?: number;
      tagline?: string;
      title?: string;
    };
  }[];
};
export type Get3MovieByMovieIdTranslationsApiArg = {
  movieId: number;
};
export type Get3MovieByMovieIdVideosApiResponse = /** status 200 200 */ {
  id?: number;
  results?: {
    iso_639_1?: string;
    iso_3166_1?: string;
    name?: string;
    key?: string;
    site?: string;
    size?: number;
    type?: string;
    official?: boolean;
    published_at?: string;
    id?: string;
  }[];
};
export type Get3MovieByMovieIdVideosApiArg = {
  movieId: number;
  language?: string;
};
export type Get3MovieByMovieIdWatchProvidersApiResponse =
  /** status 200 200 */ {
    id?: number;
    results?: {
      AE?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      AL?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      AR?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      AT?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      AU?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      BA?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      BB?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      BE?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      BG?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      BH?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      BO?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      BR?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      BS?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      CA?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      CH?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      CL?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      CO?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      CR?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      CV?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      CZ?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      DE?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      DK?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      DO?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      EC?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      EE?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      EG?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      ES?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        ads?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      FI?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      FJ?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      FR?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      GB?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      GF?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      GI?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      GR?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      GT?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      HK?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      HN?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      HR?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        ads?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      HU?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      ID?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      IE?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      IL?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      IN?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      IQ?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      IS?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      IT?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      JM?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      JO?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      JP?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      KR?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      KW?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      LB?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      LI?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      LT?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      LV?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      MD?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      MK?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      MT?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      MU?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      MX?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      MY?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      MZ?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      NL?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      NO?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      NZ?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      OM?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      PA?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      PE?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      PH?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      PK?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      PL?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      PS?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      PT?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      PY?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      QA?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      RO?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      RS?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      RU?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      SA?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      SE?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      SG?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      SI?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      SK?: {
        link?: string;
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      SM?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      SV?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      TH?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      TR?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      TT?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      TW?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      UG?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      US?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      UY?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      VE?: {
        link?: string;
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      YE?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
      ZA?: {
        link?: string;
        flatrate?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        rent?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
        buy?: {
          logo_path?: string;
          provider_id?: number;
          provider_name?: string;
          display_priority?: number;
        }[];
      };
    };
  };
export type Get3MovieByMovieIdWatchProvidersApiArg = {
  movieId: number;
};
export type Post3MovieByMovieIdRatingApiResponse = /** status 200 200 */ {
  status_code?: number;
  status_message?: string;
};
export type Post3MovieByMovieIdRatingApiArg = {
  movieId: number;
  guestSessionId?: string;
  sessionId?: string;
  "Content-Type": string;
  body: {
    RAW_BODY: string;
  };
};
export type Delete3MovieByMovieIdRatingApiResponse = /** status 200 200 */ {
  status_code?: number;
  status_message?: string;
};
export type Delete3MovieByMovieIdRatingApiArg = {
  movieId: number;
  "Content-Type"?: string;
  guestSessionId?: string;
  sessionId?: string;
};
export type Get3MoviePopularApiResponse = /** status 200 200 */ {
  page?: number;
  results?: {
    adult?: boolean;
    backdrop_path?: string;
    genre_ids?: number[];
    id?: number;
    original_language?: string;
    original_title?: string;
    overview?: string;
    popularity?: number;
    poster_path?: string;
    release_date?: string;
    title?: string;
    video?: boolean;
    vote_average?: number;
    vote_count?: number;
  }[];
  total_pages?: number;
  total_results?: number;
};
export type Get3MoviePopularApiArg = {
  language?: string;
  page?: number;
  /** ISO-3166-1 code */
  region?: string;
};
export type Get3MovieTopRatedApiResponse = /** status 200 200 */ {
  page?: number;
  results?: {
    adult?: boolean;
    backdrop_path?: string;
    genre_ids?: number[];
    id?: number;
    original_language?: string;
    original_title?: string;
    overview?: string;
    popularity?: number;
    poster_path?: string;
    release_date?: string;
    title?: string;
    video?: boolean;
    vote_average?: number;
    vote_count?: number;
  }[];
  total_pages?: number;
  total_results?: number;
};
export type Get3MovieTopRatedApiArg = {
  language?: string;
  page?: number;
  /** ISO-3166-1 code */
  region?: string;
};
export type Get3MovieUpcomingApiResponse = /** status 200 200 */ {
  dates?: {
    maximum?: string;
    minimum?: string;
  };
  page?: number;
  results?: {
    adult?: boolean;
    backdrop_path?: string;
    genre_ids?: number[];
    id?: number;
    original_language?: string;
    original_title?: string;
    overview?: string;
    popularity?: number;
    poster_path?: string;
    release_date?: string;
    title?: string;
    video?: boolean;
    vote_average?: number;
    vote_count?: number;
  }[];
  total_pages?: number;
  total_results?: number;
};
export type Get3MovieUpcomingApiArg = {
  language?: string;
  page?: number;
  /** ISO-3166-1 code */
  region?: string;
};
export type Get3MovieNowPlayingApiResponse = /** status 200 200 */ {
  dates?: {
    maximum?: string;
    minimum?: string;
  };
  page?: number;
  results?: {
    adult?: boolean;
    backdrop_path?: string;
    genre_ids?: number[];
    id?: number;
    original_language?: string;
    original_title?: string;
    overview?: string;
    popularity?: number;
    poster_path?: string;
    release_date?: string;
    title?: string;
    video?: boolean;
    vote_average?: number;
    vote_count?: number;
  }[];
  total_pages?: number;
  total_results?: number;
};
export type Get3MovieNowPlayingApiArg = {
  language?: string;
  page?: number;
  /** ISO-3166-1 code */
  region?: string;
};
export type Get3MovieLatestApiResponse = /** status 200 200 */ {
  adult?: boolean;
  backdrop_path?: any;
  belongs_to_collection?: any;
  budget?: number;
  genres?: any;
  homepage?: string;
  id?: number;
  imdb_id?: any;
  original_language?: string;
  original_title?: string;
  overview?: string;
  popularity?: number;
  poster_path?: any;
  production_companies?: any;
  production_countries?: any;
  release_date?: string;
  revenue?: number;
  runtime?: number;
  spoken_languages?: any;
  status?: string;
  tagline?: string;
  title?: string;
  video?: boolean;
  vote_average?: number;
  vote_count?: number;
};
export type Get3MovieLatestApiArg = void;
export type Get3MovieChangesApiResponse = /** status 200 200 */ {
  results?: {
    id?: number;
    adult?: boolean;
  }[];
  page?: number;
  total_pages?: number;
  total_results?: number;
};
export type Get3MovieChangesApiArg = {
  endDate?: string;
  page?: number;
  startDate?: string;
};
export const {
  useGet3SearchMovieQuery,
  useLazyGet3SearchMovieQuery,
  useGet3MovieByMovieIdQuery,
  useLazyGet3MovieByMovieIdQuery,
  useGet3MovieByMovieIdImagesQuery,
  useLazyGet3MovieByMovieIdImagesQuery,
  useGet3MovieByMovieIdAccountStatesQuery,
  useLazyGet3MovieByMovieIdAccountStatesQuery,
  useGet3MovieByMovieIdAlternativeTitlesQuery,
  useLazyGet3MovieByMovieIdAlternativeTitlesQuery,
  useGet3MovieByMovieIdChangesQuery,
  useLazyGet3MovieByMovieIdChangesQuery,
  useGet3MovieByMovieIdCreditsQuery,
  useLazyGet3MovieByMovieIdCreditsQuery,
  useGet3MovieByMovieIdExternalIdsQuery,
  useLazyGet3MovieByMovieIdExternalIdsQuery,
  useGet3MovieByMovieIdKeywordsQuery,
  useLazyGet3MovieByMovieIdKeywordsQuery,
  useGet3MovieByMovieIdListsQuery,
  useLazyGet3MovieByMovieIdListsQuery,
  useGet3MovieByMovieIdRecommendationsQuery,
  useLazyGet3MovieByMovieIdRecommendationsQuery,
  useGet3MovieByMovieIdReleaseDatesQuery,
  useLazyGet3MovieByMovieIdReleaseDatesQuery,
  useGet3MovieByMovieIdReviewsQuery,
  useLazyGet3MovieByMovieIdReviewsQuery,
  useGet3MovieByMovieIdSimilarQuery,
  useLazyGet3MovieByMovieIdSimilarQuery,
  useGet3MovieByMovieIdTranslationsQuery,
  useLazyGet3MovieByMovieIdTranslationsQuery,
  useGet3MovieByMovieIdVideosQuery,
  useLazyGet3MovieByMovieIdVideosQuery,
  useGet3MovieByMovieIdWatchProvidersQuery,
  useLazyGet3MovieByMovieIdWatchProvidersQuery,
  usePost3MovieByMovieIdRatingMutation,
  useDelete3MovieByMovieIdRatingMutation,
  useGet3MoviePopularQuery,
  useLazyGet3MoviePopularQuery,
  useGet3MovieTopRatedQuery,
  useLazyGet3MovieTopRatedQuery,
  useGet3MovieUpcomingQuery,
  useLazyGet3MovieUpcomingQuery,
  useGet3MovieNowPlayingQuery,
  useLazyGet3MovieNowPlayingQuery,
  useGet3MovieLatestQuery,
  useLazyGet3MovieLatestQuery,
  useGet3MovieChangesQuery,
  useLazyGet3MovieChangesQuery,
} = injectedRtkApi;
