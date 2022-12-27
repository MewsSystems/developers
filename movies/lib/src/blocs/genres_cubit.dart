import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/api/genres_api.dart';

/// Fetches and holds the API's movie genre ids and their corresponding names
class GenresCubit extends Cubit<Map<int, String>> {
  GenresCubit() : super({});
  
  final client = GenresApi();

  Future<void> fetch() async {
    emit(await client.getGenres());
  }
}