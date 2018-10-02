#! /bin/bash
echo ===executing tests for mews distributors web app ===

echo === running UI demo tests ===
robot tests
rm -rf *.png
