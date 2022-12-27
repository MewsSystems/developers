import 'package:flutter/material.dart';
import 'package:movies/src/pages/search/result_list.dart';


/// Used to scroll to the top of the list
class TopButton extends StatelessWidget {
  const TopButton({super.key});

  @override
  Widget build(BuildContext context) => FloatingActionButton(
    mini: true,
        onPressed: () => resultsScrollController.animateTo(
          0,
          duration: const Duration(milliseconds: 300),
          curve: Curves.ease,
        ),
        child: const Icon(Icons.arrow_upward),
      );
}
