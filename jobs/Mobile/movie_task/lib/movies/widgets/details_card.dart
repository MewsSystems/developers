import 'package:flutter/material.dart';
import 'package:optimus/optimus.dart';

class DetailsCard extends StatelessWidget {
  const DetailsCard({Key? key, required this.title, required this.content})
      : super(key: key);

  final String title;
  final String content;

  @override
  Widget build(BuildContext context) => OptimusCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            OptimusSubtitle(
              child: Text(
                title,
              ),
            ),
            OptimusParagraphSmall(child: Text(content)),
          ],
        ),
      );
}
