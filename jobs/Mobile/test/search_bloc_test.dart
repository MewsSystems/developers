import 'package:bloc_test/bloc_test.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mews_imdb/search/bloc/search_bloc.dart';
import 'package:movie_repository/movie_repository.dart' as rep;
import 'package:mocktail/mocktail.dart';

class MockMovieRepository extends Mock implements rep.MovieRepository {}

class MockSearchResult extends Mock implements rep.SearchResult {}

const List<rep.MoviePreview> previews = [
  rep.MoviePreview(
    id: 1,
    title: 'mock-title1',
    voteAverage: 7.7,
    releaseDate: null,
    overview: 'Some value',
  ),
  rep.MoviePreview(
    id: 7,
    title: 'mock-title2',
    voteAverage: 4.2,
    releaseDate: null,
    overview: 'Some value',
  ),
  rep.MoviePreview(
    id: 3,
    title: 'mock-title3',
    voteAverage: 1.3,
    releaseDate: null,
    overview: 'Some value',
  ),
  rep.MoviePreview(
    id: 4,
    title: 'mock-title4',
    voteAverage: 2.2,
    releaseDate: null,
    overview: 'Some value',
  ),
  rep.MoviePreview(
    id: 5,
    title: 'mock-title5',
    voteAverage: 0.2,
    releaseDate: null,
    overview: 'Some value',
  ),
  rep.MoviePreview(
    id: 9,
    title: 'mock-title6',
    voteAverage: 2.5,
    releaseDate: null,
    overview: 'Some value',
  ),
];

const int totalResults = 1;
const int totalPages = 1;
const int page = 1;

void main() {
  group('SearchBloc', () {
    late rep.SearchResult searchResult;
    late rep.MovieRepository movieRepository;
    late SearchBloc mockSearchBloc;

    setUp(() {
      searchResult = MockSearchResult();
      movieRepository = MockMovieRepository();
      mockSearchBloc = SearchBloc(movieRepository: movieRepository);
      when(() => searchResult.previews).thenReturn(previews);
      when(() => searchResult.totalPages).thenReturn(totalPages);
      when(() => searchResult.totalResults).thenReturn(totalResults);
      when(() => movieRepository.search(any()))
          .thenAnswer((_) async => searchResult);

      test('init state', () {
        expect(mockSearchBloc.state, equals(SearchStatus.initial));
      });
    });
  });
}
