import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:move_app/data/data_providers/move_api.dart';
import 'package:http/http.dart' as http;

class MockResponse extends Mock implements http.Response {}

class MockHttpClient extends Mock implements http.Client {}

class FakeUri extends Fake implements Uri {}

void main() {

  late http.Client _httpClient;
  late MovieAPI _mockMovieAPI;

  setUpAll(() {
    registerFallbackValue(FakeUri());
  });

  setUp(() {
    _httpClient = MockHttpClient();
    _mockMovieAPI = MovieAPI(httpClient: _httpClient);
  });

  test('throws FetchRawMoviesRequestFailure on non-400 response', () async {
    final response = MockResponse();
    when(() => response.statusCode).thenReturn(400);
    when(() => _httpClient.get(any())).thenAnswer((_) async => response);
    expect(
      () async => await _mockMovieAPI.fetchRawMovies('tit'),
      throwsA(isA<FetchRawMoviesRequestFailure>()),
    );
  });

  test('test https request called once for fetchRawMovies', () async {
    final response = MockResponse();
    when(() => response.statusCode).thenReturn(200);
    when(() => _httpClient.get(any())).thenAnswer((_) async => response);
    when(() => response.body).thenReturn('result: []');
    try {
      await _mockMovieAPI.fetchRawMovies('tit');
    } catch (_) {}

    verify(
    () => _httpClient.get(
      Uri.https(
        'api.themoviedb.org', 
        '/3/search/movie/', 
        {'query': 'tit', 'api_key': "ccd4dac29abf285035c8ad91af5e1f6e"}
      )
    )
    ).called(1);
  });

  test('test https request called once for fetchRawMovieDetail', () async {
    final response = MockResponse();
    when(() => response.statusCode).thenReturn(200);
    when(() => _httpClient.get(any())).thenAnswer((_) async => response);
    when(() => response.body).thenReturn('result: []');
    try {
    await _mockMovieAPI.fetchRawMovieDetail('3');
    } catch (_) {}

    verify(
    () => _httpClient.get(
        Uri.https(
        'api.themoviedb.org', 
        '/3/movie/3', 
        {'api_key': 'ccd4dac29abf285035c8ad91af5e1f6e'}
        )
      )
    ).called(1);
  });

  test('throws FetchRawMovieRequestFailure on non-400 response', () async {
    final response = MockResponse();
    when(() => response.statusCode).thenReturn(400);
    when(() => _httpClient.get(any())).thenAnswer((_) async => response);
    expect(
      () async => await _mockMovieAPI.fetchRawMovieDetail('3'),
      throwsA(isA<FetchRawMovieDetailRequestFailure>()),
    );
  });


}
