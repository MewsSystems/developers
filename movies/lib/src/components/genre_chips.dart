import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/blocs/genres_cubit.dart';

/// Wrap that generates [GenreChips] according to a list of [genreIds]
class GenreChips extends StatelessWidget {
  const GenreChips({Key? key, required this.genreIds}) : super(key: key);

  final List<int> genreIds;
  @override
  Widget build(BuildContext context) =>
      BlocBuilder<GenresCubit, Map<int, String>>(
        builder: (context, genres) => Wrap(
          spacing: 4,
          runSpacing: 4,
          children: [
            for (var genreId in genreIds)
              if (genres[genreId] != null)
                Container(
                  padding: const EdgeInsets.all(4),
                  color: Colors.black54,
                  child: Text(genres[genreId]!, style: const TextStyle(fontSize: 12),),
                )
          ],
        ),
      );
}
