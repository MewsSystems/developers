#!/bin/bash

readonly CONFIG="playwright.config.ts"
readonly PATTERN=".spec.ts"

run_all_tests() {
  pnpm exec playwright test -c "$CONFIG"
}

run_specific_test() {
  local spec

  read -p "Enter test name: (Without ${PATTERN} pattern) " spec

  pnpm exec playwright test -c "$CONFIG" "${spec}${PATTERN}"
}

echo "Testing CLI 0.1.0"
echo "---------------------------------"

PS3="Please select: (1-3) "

select option in "All tests" "Specific test" "Cancel";
do
  case $option in
    "All tests")
      run_all_tests
      break
      ;;
    "Specific test")
      run_specific_test
      break
      ;;
    "Cancel")
      echo "No changes made."
      exit 0
      ;;
    *)
      echo "Invalid option; please select 1-3."
      ;;
  esac
done
