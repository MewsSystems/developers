const jackReacher = 'Jack Reacher';
const whenTheManComesAround = 'Jack Reacher: When the Man Comes Around';
const neverGoBack = 'Jack Reacher: Never Go Back';

const overview =
  "One morning in an ordinary town, five people are shot dead in a seemingly random attack. All evidence points to a single suspect: an ex-military sniper who is quickly brought into custody. The interrogation yields one written note: 'Get Jack Reacher!'. Reacher, an enigmatic ex-Army investigator, believes the authorities have the right man but agrees to help the sniper's defense attorney. However, the more Reacher delves into the case, the less clear-cut it appears. So begins an extraordinary chase for the truth, pitting Jack Reacher against an unexpected enemy, with a skill for violence and a secret to keep.";

const jackReacherMovie = {
  adult: false,
  backdrop_path: '/iwvP8XVpYVmJ3xfF9xdBi5uAOWl.jpg',
  genre_ids: [80, 18, 53, 28],
  id: 75780,
  original_language: 'en',
  original_title: jackReacher,
  overview,
  popularity: 92.994,
  poster_path: '/uQBbjrLVsUibWxNDGA4Czzo8lwz.jpg',
  release_date: '2012-12-20',
  title: jackReacher,
  video: false,
  vote_average: 6.62,
  vote_count: 6748,
};

const mockData = [
  jackReacherMovie,
  {
    adult: false,
    backdrop_path: null,
    genre_ids: [99],
    id: 1045592,
    original_language: 'en',
    original_title: whenTheManComesAround,
    overview:
      "Cast and crew speak on adapting One Shot as the first Jack Reacher film, casting Tom Cruise, earning Lee Child's blessing, additional character qualities and the performances that shape them, Lee Child's cameo in the film, and shooting the film's climax.",
    popularity: 11.277,
    poster_path: '/tcOPca5Ook6aR9mehrnxD9kfk7m.jpg',
    release_date: '2013-05-07',
    title: whenTheManComesAround,
    video: false,
    vote_average: 10,
    vote_count: 1,
  },
  {
    adult: false,
    backdrop_path: '/ww1eIoywghjoMzRLRIcbJLuKnJH.jpg',
    genre_ids: [28, 53],
    id: 343611,
    original_language: 'en',
    original_title: neverGoBack,
    overview:
      'When Major Susan Turner is arrested for treason, ex-investigator Jack Reacher undertakes the challenging task to prove her innocence and ends up exposing a shocking conspiracy.',
    popularity: 43.165,
    poster_path: '/cOg3UT2NYWHZxp41vpxAnVCOC4M.jpg',
    release_date: '2016-10-19',
    title: neverGoBack,
    video: false,
    vote_average: 5.974,
    vote_count: 4683,
  },
];

export {
  mockData,
  jackReacher,
  neverGoBack,
  whenTheManComesAround,
  jackReacherMovie,
  overview,
};
