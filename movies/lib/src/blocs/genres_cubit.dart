import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/api/genres_api.dart';

class GenresCubit extends Cubit<Map<int, String>> {
  GenresCubit() : super({});
  
  final client = GenresApi();

  Future<void> fetch() async {
    emit(await client.getGenres());
  }
}