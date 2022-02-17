import 'package:flutter/material.dart';

const SizedBox kHorizontal8 = SizedBox(width: 8);
const SizedBox kHorizontal12 = SizedBox(width: 12);
const SizedBox kHorizontal16 = SizedBox(width: 16);

const SizedBox kVertical8 = SizedBox(height: 8);
const SizedBox kVertical12 = SizedBox(height: 12);
const SizedBox kVertical16 = SizedBox(height: 16);
const SizedBox kVertical60 = SizedBox(height: 60);

const kShadow = <BoxShadow>[
  BoxShadow(
    color: _shadowColor,
    blurRadius: 4,
    offset: const Offset(2, 2),
  ),
];

const _shadowColor = Color(0xFFE1E1E1);
