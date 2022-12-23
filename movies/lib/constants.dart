import 'package:movies/src/model/movie.dart';

const originalPosterPrefix = 'https://image.tmdb.org/t/p/original';
const smallPosterPrefix = 'https://image.tmdb.org/t/p/w200';

final List<Movie> items = [
  Movie(
    id: 1234,
    title: 'Avatar 2',
    originalTitle: 'Avatar 2',
    originalLanguage: 'French',
    backdropPath:
        'https://fr.web.img4.acsta.net/pictures/22/11/02/14/49/4565071.jpg',
    posterPath:
        'https://fr.web.img4.acsta.net/pictures/22/11/02/14/49/4565071.jpg',
    releaseDate: DateTime(2022, 12, 14),
    popularity: 9,
    voteAverage: 8.3,
    adult: false,
    overview: 'Lorem ipsum',
    genreIds: [1, 2],
  ),
];
