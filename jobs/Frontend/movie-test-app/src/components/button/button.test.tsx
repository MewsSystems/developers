import Button from './';
import { render, screen } from '@testing-library/react';
import { test, describe, expect } from 'vitest';
import { palette, defaultTheme, ThemeColors } from '../../theme/theme.ts';
import { ThemeProvider } from 'styled-components';

const butttonText = 'Test Text';

describe('ThemedButton', () => {
  test('test blue color ', async () => {
    render(
      <ThemeProvider theme={defaultTheme}>
        <Button>{butttonText}</Button>
      </ThemeProvider>,
    );
    const button = screen.getByRole('button', {
      name: butttonText,
    });
    expect(button).toBeDefined();
    expect(button).toHaveStyle(`border-color: #084254`);
    expect(button).toHaveStyle(`color: #084254`);
  });

  test('test red color ', async () => {
    const themeColor: ThemeColors = 'red';
    render(
      <ThemeProvider theme={{ ...defaultTheme, colors: { ...palette[themeColor] } }}>
        <Button>{butttonText}</Button>
      </ThemeProvider>,
    );
    const button = screen.getByRole('button', {
      name: butttonText,
    });
    expect(button).toBeDefined();
    expect(button).toHaveStyle(`border-color: #6D0A37`);
    expect(button).toHaveStyle(`color: #6D0A37`);
  });

  test('test purple color ', async () => {
    const themeColor: ThemeColors = 'purple';
    render(
      <ThemeProvider theme={{ ...defaultTheme, colors: { ...palette[themeColor] } }}>
        <Button>{butttonText}</Button>
      </ThemeProvider>,
    );
    const button = screen.getByRole('button', {
      name: butttonText,
    });
    expect(button).toBeDefined();
    expect(button).toHaveStyle(`border-color: #250A52`);
    expect(button).toHaveStyle(`color: #250A52`);
  });
});
