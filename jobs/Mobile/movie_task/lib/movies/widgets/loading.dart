import 'package:flutter/material.dart';

class BottomLoader extends StatelessWidget {
  const BottomLoader({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) => const Center(
        child:
            SizedBox(height: 24, width: 24, child: CircularProgressIndicator()),
      );
}
