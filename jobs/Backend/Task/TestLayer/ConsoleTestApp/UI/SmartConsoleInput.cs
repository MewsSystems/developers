using Spectre.Console;

namespace ConsoleTestApp.UI;

/// <summary>
/// Smart console input handler with context-aware tab completion.
/// </summary>
public class SmartConsoleInput
{
    private readonly AutoCompleteEngine _autocomplete;

    public SmartConsoleInput(AutoCompleteEngine autocomplete)
    {
        _autocomplete = autocomplete;
    }

    public string ReadLine()
    {
        var input = "";
        var cursorPosition = 0;
        var promptLine = Console.CursorTop;

        AnsiConsole.Markup("[bold cyan1]>[/] ");

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return input;
            }
            else if (key.Key == ConsoleKey.Tab)
            {
                // Get current suggestions
                var currentSuggestions = _autocomplete.GetSuggestions(input);

                if (currentSuggestions.Length == 0)
                {
                    continue;
                }

                // Clear the current line first
                Console.SetCursorPosition(0, promptLine);
                Console.Write(new string(' ', Console.BufferWidth - 1));
                Console.SetCursorPosition(0, promptLine);

                // Show all suggestions below
                Console.WriteLine();
                ShowSuggestions(currentSuggestions);

                // Show usage hint if available
                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    var hint = _autocomplete.GetUsageHint(parts[0]);
                    if (!string.IsNullOrEmpty(hint))
                    {
                        AnsiConsole.MarkupLine($"[dim]Usage: {hint}[/]");
                    }
                }

                // Redraw the prompt on a new line with current input
                Console.WriteLine();
                promptLine = Console.CursorTop;
                AnsiConsole.Markup("[bold cyan1]>[/] ");
                Console.Write(input);
                Console.SetCursorPosition(2 + cursorPosition, promptLine);
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (cursorPosition > 0)
                {
                    input = input.Remove(cursorPosition - 1, 1);
                    cursorPosition--;
                    RedrawLine(input, cursorPosition);
                }
            }
            else if (key.Key == ConsoleKey.Delete)
            {
                if (cursorPosition < input.Length)
                {
                    input = input.Remove(cursorPosition, 1);
                    RedrawLine(input, cursorPosition);
                }
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (cursorPosition > 0)
                {
                    cursorPosition--;
                    Console.SetCursorPosition(2 + cursorPosition, Console.CursorTop);
                }
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                if (cursorPosition < input.Length)
                {
                    cursorPosition++;
                    Console.SetCursorPosition(2 + cursorPosition, Console.CursorTop);
                }
            }
            else if (key.Key == ConsoleKey.Home)
            {
                cursorPosition = 0;
                Console.SetCursorPosition(2 + cursorPosition, Console.CursorTop);
            }
            else if (key.Key == ConsoleKey.End)
            {
                cursorPosition = input.Length;
                Console.SetCursorPosition(2 + cursorPosition, Console.CursorTop);
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                // Clear the line
                input = "";
                cursorPosition = 0;
                RedrawLine(input, cursorPosition);
            }
            else if (!char.IsControl(key.KeyChar))
            {
                // Regular character
                input = input.Insert(cursorPosition, key.KeyChar.ToString());
                cursorPosition++;
                RedrawLine(input, cursorPosition);
            }
        }
    }

    private void RedrawLine(string input, int cursorPos)
    {
        // Clear current line
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.BufferWidth - 1));
        Console.SetCursorPosition(0, Console.CursorTop);

        // Redraw prompt
        AnsiConsole.Markup("[bold cyan1]>[/] ");

        // Write input
        Console.Write(input);

        // Set cursor position (2 = "> " prefix length)
        Console.SetCursorPosition(2 + cursorPos, Console.CursorTop);
    }

    private void ShowSuggestions(string[] suggestions)
    {
        if (suggestions.Length == 0)
        {
            return;
        }

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddColumn();

        // Group suggestions into rows of 4
        for (int i = 0; i < suggestions.Length; i += 4)
        {
            var row = suggestions.Skip(i).Take(4).ToArray();

            // Determine color based on content
            var coloredRow = row.Select(s =>
            {
                if (s.StartsWith("[[") && s.EndsWith("]]"))
                {
                    return $"[dim]{s}[/]"; // Placeholders in dim (already escaped)
                }
                else if (new[] { "rest", "soap", "grpc" }.Contains(s.ToLower()))
                {
                    return $"[cyan1]{s}[/]"; // Protocols in cyan
                }
                else if (new[] { "EUR", "USD", "GBP", "JPY", "CHF", "CZK", "RON" }.Contains(s.ToUpper()))
                {
                    return $"[yellow]{s}[/]"; // Currencies in yellow
                }
                else if (new[] { "ECB", "CNB", "BNR" }.Contains(s.ToUpper()))
                {
                    return $"[green]{s}[/]"; // Providers in green
                }
                else if (new[] { "Admin", "Consumer" }.Contains(s))
                {
                    return $"[magenta1]{s}[/]"; // Roles in magenta
                }
                else if (new[] { "true", "false" }.Contains(s.ToLower()))
                {
                    return $"[blue]{s}[/]"; // Booleans in blue
                }
                else if (new[] { "latest", "historical" }.Contains(s.ToLower()))
                {
                    return $"[yellow]{s}[/]"; // Options in yellow
                }
                else if (int.TryParse(s, out _))
                {
                    return $"[cyan1]{s}[/]"; // Numbers in cyan
                }
                else
                {
                    return $"[white]{s}[/]"; // Commands/other in white
                }
            }).ToArray();

            // Pad with empty strings if needed
            while (coloredRow.Length < 4)
            {
                coloredRow = coloredRow.Concat(new[] { "" }).ToArray();
            }

            grid.AddRow(coloredRow[0], coloredRow[1], coloredRow[2], coloredRow[3]);
        }

        AnsiConsole.Write(grid);
    }
}
