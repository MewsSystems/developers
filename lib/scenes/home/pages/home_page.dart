import 'package:flutter/material.dart';

class HomePage extends StatelessWidget {
  const HomePage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Movies"),
        bottom: PreferredSize(
          child: TextField(),
          preferredSize: const Size(double.infinity, 80),
        ),
      ),
      body: Container(),
    );
  }
}
