const MOCKED_DETAILS_MOVIE = {
  adult: false,
  backdrop_path: '/hQ4pYsIbP22TMXOUdSfC2mjWrO0.jpg',
  belongs_to_collection: {
    id: 1382526,
    name: "Aki Kaurismäki's Proletariat Trilogy",
    poster_path: '/bUrReoZFLGti6ehkBW0xw8f12MT.jpg',
    backdrop_path: '/zAUItK1Nr473DIe8gWMsZ0DMR7L.jpg',
  },
  budget: 0,
  genres: [
    { id: 35, name: 'Comedy' },
    { id: 18, name: 'Drama' },
    { id: 10749, name: 'Romance' },
    { id: 80, name: 'Crime' },
  ],
  homepage: '',
  id: 2,
  imdb_id: 'tt0094675',
  origin_country: ['FI'],
  original_language: 'fi',
  original_title: 'Ariel',
  overview:
    'A Finnish man goes to the city to find a job after the mine where he worked is closed and his father commits suicide.',
  popularity: 1.8037,
  poster_path: '/ojDg0PGvs6R9xYFodRct2kdI6wC.jpg',
  production_companies: [
    {
      id: 2303,
      logo_path: null,
      name: 'Villealfa Filmproductions',
      origin_country: 'FI',
    },
  ],
  production_countries: [
    {
      iso_3166_1: 'FI',
      name: 'Finland',
    },
  ],
  release_date: '1988-10-21',
  revenue: 0,
  runtime: 73,
  spoken_languages: [
    {
      english_name: 'Finnish',
      iso_639_1: 'fi',
      name: 'suomi',
    },
  ],
  status: 'Released',
  tagline: '',
  title: 'Ariel',
  video: false,
  vote_average: 7.109,
  vote_count: 353,
};

const MOCKED_LIST_MOVIES = {
  page: 2,
  results: [
    {
      adult: false,
      backdrop_path: '/uTz92k0CfZpn1p3xRmPbgt7MQTz.jpg',
      genre_ids: [35],
      id: 13342,
      original_language: 'en',
      original_title: 'Fast Times at Ridgemont High',
      overview:
        'Based on the real-life adventures chronicled by Cameron Crowe, Fast Times follows a group of high school students growing up in Southern California. Stacy Hamilton and Mark Ratner are looking for a love interest, and are helped along by their older classmates, Linda Barrett and Mike Damone, respectively. At the center of the film is Jeff Spicoli, a perpetually stoned surfer who faces-off with the resolute Mr. Hand—a man convinced that everyone is on dope.',
      popularity: 3.1891,
      poster_path: '/s1DA8H7qwoOcAEhow2rCzuQtpuO.jpg',
      release_date: '1982-08-13',
      title: 'Fast Times at Ridgemont High',
      video: false,
      vote_average: 6.799,
      vote_count: 1284,
    },
    {
      adult: false,
      backdrop_path: null,
      genre_ids: [35],
      id: 264862,
      original_language: 'de',
      original_title: 'Der Kurpfuscher und seine fixen Töchter',
      overview:
        'Arriving in an alpine village a crook is mistaken for a recently deceased doctor and decides to impersonate him. Three girls stranded in the same village are sheltered by the "doctor" and naked German hilarity inevitably ensues.',
      popularity: 0.3278,
      poster_path: '/i9q89Ie6b1ZTZVQJ7SkHr4HkAB6.jpg',
      release_date: '1980-08-15',
      title: 'The Quack and His Fast Daughters',
      video: false,
      vote_average: 4.6,
      vote_count: 5,
    },
  ],
  total_pages: 39,
  total_results: 765,
};

export { MOCKED_DETAILS_MOVIE, MOCKED_LIST_MOVIES };
