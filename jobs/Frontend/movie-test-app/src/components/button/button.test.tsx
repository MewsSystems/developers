import Button from './';
import { render, screen } from '@testing-library/react';
import { test, describe, expect } from 'vitest';
import { theme, ThemeColors } from '../../assets/colors/theme/theme.ts';
import { ThemeProvider } from 'styled-components';

const butttonText = 'Test Text';

describe('ThemedButton', () => {
  test('test blue color ', async () => {
    const themeColor: ThemeColors = 'blue';
    render(
      <ThemeProvider theme={theme[themeColor]}>
        <Button>{butttonText}</Button>
      </ThemeProvider>,
    );
    const button = screen.getByRole('button', {
      name: butttonText,
    });
    expect(button).toBeDefined();
    expect(button).toHaveStyle(`border-color: ${theme.blue.primary}`);
    expect(button).toHaveStyle(`color: ${theme.blue.primary}`);
  });
});
