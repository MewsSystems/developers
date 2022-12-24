import 'package:flutter/material.dart';
import 'package:movies/constants.dart';

class MovieChip extends StatelessWidget {
  const MovieChip({Key? key, required this.label, this.icon}) : super(key: key);

  final String label;
  final IconData? icon;
  @override
  Widget build(BuildContext context) => Chip(
        backgroundColor: chipColor,
        labelPadding: EdgeInsets.only(right: 8, left: icon == null ? 8 : 0),
        avatar: icon == null
            ? null
            : Icon(
                icon,
                size: 16,
              ),
        label: Text(
          label,
          style: const TextStyle(color: Colors.white),
        ),
      );
}
