import 'package:flutter/material.dart';
import 'package:movies/pages/search_page/_widgets/loader.dart';

class PaginationLoader extends StatelessWidget {
  const PaginationLoader({super.key});

  @override
  Widget build(BuildContext context) => const SafeArea(
        child: SizedBox(
          height: 150.0,
          child: Loader(),
        ),
      );
}
