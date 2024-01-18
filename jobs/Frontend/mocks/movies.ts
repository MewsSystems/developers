import { Get3SearchMovieApiResponse } from '@/store';

type Movies = Exclude<Get3SearchMovieApiResponse['results'], undefined>[number]

export const movie1: Movies = {
  "adult": false,
  "backdrop_path": "/4qCqAdHcNKeAHcK8tJ8wNJZa9cx.jpg",
  "genre_ids": [
    12,
    28,
    878
  ],
  "id": 11,
  "original_language": "en",
  "original_title": "Star Wars",
  "overview": "Princess Leia is captured and held hostage by the evil Imperial forces in their effort to take over the galactic Empire. Venturesome Luke Skywalker and dashing captain Han Solo team together with the loveable robot duo R2-D2 and C-3PO to rescue the beautiful princess and restore peace and justice in the Empire.",
  "popularity": 78.013,
  "poster_path": "/6FfCtAuVAW8XJjZ7eWeLibRLWTw.jpg",
  "release_date": "1977-05-25",
  "title": "Star Wars",
  "video": false,
  "vote_average": 8.205,
  "vote_count": 19549
}

export const movie2: Movies = {
  "adult": false,
  "backdrop_path": "/epVMXf10WqFkONzKR8V76Ypj5Y3.jpg",
  "genre_ids": [
    12,
    28,
    878
  ],
  "id": 181808,
  "original_language": "en",
  "original_title": "Star Wars: The Last Jedi",
  "overview": "Rey develops her newly discovered abilities with the guidance of Luke Skywalker, who is unsettled by the strength of her powers. Meanwhile, the Resistance prepares to do battle with the First Order.",
  "popularity": 171.611,
  "poster_path": "/kOVEVeg59E0wsnXmF9nrh6OmWII.jpg",
  "release_date": "2017-12-13",
  "title": "Star Wars: The Last Jedi",
  "video": false,
  "vote_average": 6.814,
  "vote_count": 14573
}

export const movie3: Movies = {
  "adult": false,
  "backdrop_path": "/iflKt34Ck2JpY2PY9wW1zwdJgJi.jpg",
  "genre_ids": [
    16,
    878,
    12,
    10751
  ],
  "id": 782054,
  "original_language": "ja",
  "original_title": "映画ドラえもん のび太の宇宙小戦争 2021",
  "overview": "One day during summer vacation, a palm-sized alien named Papi appears from a small rocket that Nobita picks up. He is the president of Pirika, a small planet in outer space, and has come to Earth to escape the rebels. Doraemon and his friends are puzzled by Papi’s small size, but as they play together using the secret tool “Small Light”, they gradually become friends. However, a whale-shaped space battleship comes to earth and attacks Doraemon, Nobita and the others in order to capture Papi. Feeling responsible for getting everyone involved, Papi tries to stand up to the rebels. Doraemon and his friends leave for the planet Pirika to protect their dear friend and his home.",
  "popularity": 22.849,
  "poster_path": "/48gKZioIDeUOI0afbYv3kh9u9RQ.jpg",
  "release_date": "2022-03-04",
  "title": "Doraemon: Nobita's Little Star Wars 2021",
  "video": false,
  "vote_average": 6,
  "vote_count": 104
}
