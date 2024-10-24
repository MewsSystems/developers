import Button from './';
import { render, screen } from '@testing-library/react';
import { test, describe, expect } from 'vitest';
import { theme } from '../../assets/colors/theme/theme.ts';
import { ThemeProvider } from 'styled-components';

const butttonText = 'Test Text';

describe('ThemedButton', () => {
  test('test blue color ', async () => {
    render(
      <ThemeProvider theme={theme['blue']}>
        <Button>{butttonText}</Button>
      </ThemeProvider>,
    );
    const button = screen.getByRole('button', {
      name: butttonText,
    });
    expect(button).toBeDefined();
  });
});
