import 'package:dio/dio.dart';
import 'package:movies/config/consts.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/models/movie_search_response_model.dart';
import 'package:retrofit/retrofit.dart';

part 'client.g.dart';

// *** CLIENT *** //
@RestApi(baseUrl: Api.baseUrl)
abstract class APIClient {
  factory APIClient(Dio dio, {String baseUrl}) = _APIClient;

  @GET(Api.searchMoviesPath)
  Future<MovieSearchResponse> getMovies(
    @Query('api_key') String apiKey,
    @Query('language') String language,
    @Query('query') String query,
    @Query('page') int page,
    @Query('include_adult') bool includeAdult,
  );

  @GET('${Api.detailedMoviesPath}/{id}')
  Future<DetailedMovie> getMovieById(
    @Path('id') int id,
    @Query('api_key') String apiKey,
    @Query('language') String language,
  );
}
