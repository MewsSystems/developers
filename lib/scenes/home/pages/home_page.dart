import 'package:flutter/material.dart';

import '../widgets/search_field.dart';

class HomePage extends StatelessWidget {
  const HomePage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Movies"),
        bottom: const PreferredSize(
          preferredSize: Size(double.infinity, 40),
          child: SearchField(),
        ),
      ),
      body: Container(),
    );
  }
}
