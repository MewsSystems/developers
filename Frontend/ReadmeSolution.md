# Mews frontend developer task

This fork contains a solution to your task developed by [Mark Collis](mark@markcollis.dev).

## Requirement

**High level requirement**: Develop a client application to display live exchange rate data for a selection of available currency pairs.

**Selection**: The user shall be able to filter displayed currency pairs. I have interpreted this as follows:
 - Display a list of available currency pairs
 - Enable this list to be filtered by matching against a text input
 - Enable each currency pair to be individually selected or deselected for display
 - Enable all currency pairs matching the current filter to be collectively selected or deselected for display

**Display**: The user shall be able to see the shortcut name ({name1}/{name2}), current value and trend (growing, declining or stagnating) for each selected currency pair.

**Responsiveness**: A loading message shall be shown before the configuration has been received. Rate requests shall be sent for all available currency pairs so that the information is available to display immediately when they are selected. Configuration and selection data shall be saved to localStorage on exit so that they are immediately available when the page is refreshed.

**Error handling**: If the configuration request fails and no configuration is available in localStorage, an error message is displayed. If an individual rate request fails, no action is taken. However, if three successive rate requests fail, an error message is displayed until the next successful request so that the user is not misled into using out of date information.
