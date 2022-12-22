import 'package:flutter/material.dart';

/// Displays detailed information about a movie
class DetailsPage extends StatelessWidget {
  const DetailsPage({super.key});

  static const routeName = '/sample_item';

  @override
  Widget build(BuildContext context) => Scaffold(
      appBar: AppBar(
        title: const Text('Item Details'),
      ),
      body: const Center(
        child: Text('More Information Here'),
      ),
    );
}
