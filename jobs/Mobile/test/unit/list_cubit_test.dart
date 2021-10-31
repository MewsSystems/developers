import 'package:bloc_test/bloc_test.dart';
import 'package:either_dart/either.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mockito/annotations.dart';
import 'package:mockito/mockito.dart';
import 'package:movie_search/api/api_manager.dart';
import 'package:movie_search/bloc/list/list_cubit.dart';
import 'package:movie_search/model/api_error/api_error.dart';
import 'package:movie_search/model/movies_list_response/movies_list_response.dart';

import 'list_cubit_test.mocks.dart';

@GenerateMocks([ApiManager])
void main() {
  MockApiManager mockApiManager = MockApiManager();

  group('LoginCubit', () {
    blocTest<ListCubit, ListState>(
      'emits [] when created',
      build: () => ListCubit(mockApiManager),
      expect: () => <ListState>[],
    );

    blocTest<ListCubit, ListState>(
      'emits [] when submitted empty input',
      build: () => ListCubit(mockApiManager),
      act: (cubit) async {
        await cubit.refresh('');
      },
      expect: () => <ListState>[],
    );

    blocTest<ListCubit, ListState>('emits [Loading, LoadedData] on search',
        build: () => ListCubit(mockApiManager),
        act: (cubit) async {
          String testInput = 'a';
          Future<Either<ApiError, MoviesListResponse>> future = Future.value(const Right<ApiError, MoviesListResponse>(
              MoviesListResponse(page: 1, totalPages: 10, totalResults: 100500, items: [])));
          when(mockApiManager.loadMovieList(testInput, 1)).thenAnswer((_) => future);
          await cubit.refresh(testInput);
        },
        expect: () => [
              isA<Loading>(),
              isA<LoadedData>()
                  .having((LoadedData state) => state.items, 'state.items', equals([]))
                  .having((LoadedData state) => state.pageNumber, 'state.pageNumber', equals(1))
            ]);

    blocTest<ListCubit, ListState>(
      'emits [Loading, LoadedData] on loading next page',
      build: () => ListCubit(mockApiManager),
      act: (cubit) async {
        String testInput = 'a';
        Future<Either<ApiError, MoviesListResponse>> future = Future.value(const Right<ApiError, MoviesListResponse>(
            MoviesListResponse(page: 1, totalPages: 10, totalResults: 100500, items: [])));
        when(mockApiManager.loadMovieList(testInput, 1)).thenAnswer((_) => future);
        await cubit.refresh(testInput);
        future = Future.value(const Right<ApiError, MoviesListResponse>(
            MoviesListResponse(page: 2, totalPages: 10, totalResults: 100500, items: [])));
        when(mockApiManager.loadMovieList(testInput, 2)).thenAnswer((_) => future);
        await cubit.loadNextPage(testInput);
      },
      expect: () => [
        isA<Loading>(),
        isA<LoadedData>()
            .having((LoadedData state) => state.items, 'state.items', equals([]))
            .having((LoadedData state) => state.pageNumber, 'state.pageNumber', equals(1)),
        isA<Loading>(),
        isA<LoadedData>()
            .having((LoadedData state) => state.items, 'state.items', equals([]))
            .having((LoadedData state) => state.pageNumber, 'state.pageNumber', equals(2))
      ],
    );
  });
}
