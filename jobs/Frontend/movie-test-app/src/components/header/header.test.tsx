import { Header } from './';
import { render, screen } from '@testing-library/react';
import { test, describe, expect, vi } from 'vitest';
import { ThemeProvider } from 'styled-components';
import { defaultTheme } from '../../theme/theme.ts';
import userEvent from '@testing-library/user-event';

describe('Header', () => {
  test('test settings ', async () => {
    const handleUpdateSearchQuery = vi.fn();
    render(
      <ThemeProvider theme={defaultTheme}>
        <Header handleUpdateSearchQuery={handleUpdateSearchQuery} searchQuery={''} isMobile={false} />
      </ThemeProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    const settingsButton = screen.getByTestId('settings-button');
    expect(settingsButton).toBeDefined();
    await userEvent.click(settingsButton);
    const settingsContainer = screen.getByTestId('theme-select');
    expect(settingsContainer).toBeDefined();
    const searchInput = screen.getByTestId('search-input');
    expect(searchInput).toBeDefined();
    expect(handleUpdateSearchQuery).not.toHaveBeenCalled();
  });

  test('test back button ', async () => {
    const handleBack = vi.fn();
    const handleUpdateSearchQuery = vi.fn();
    render(
      <ThemeProvider theme={defaultTheme}>
        <Header
          handleUpdateSearchQuery={handleUpdateSearchQuery}
          searchQuery={''}
          isMobile={false}
          hasBackButton
          handlePressBackButton={handleBack}
        />
      </ThemeProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    const backButton = screen.getByTestId('back-button');
    expect(backButton).toBeDefined();
    await userEvent.click(backButton);
    expect(handleBack).toHaveBeenCalled();
    expect(handleUpdateSearchQuery).not.toHaveBeenCalled();
    const searchInput = screen.getByTestId('search-input');
    expect(searchInput).toBeDefined();
    expect(handleUpdateSearchQuery).not.toHaveBeenCalled();
  });

  test('test search input ', async () => {
    const handleUpdateSearchQuery = vi.fn();
    render(
      <ThemeProvider theme={defaultTheme}>
        <Header handleUpdateSearchQuery={handleUpdateSearchQuery} searchQuery={''} isMobile={false} />
      </ThemeProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    const searchInput = screen.getByTestId('search-input');
    expect(searchInput).toBeDefined();
    await userEvent.type(searchInput, 'test');
    expect(handleUpdateSearchQuery).toHaveBeenCalled();
  });

  test('test logo click ', async () => {
    const handleClickLogo = vi.fn();
    const handleUpdateSearchQuery = vi.fn();
    render(
      <ThemeProvider theme={defaultTheme}>
        <Header
          handleUpdateSearchQuery={handleUpdateSearchQuery}
          searchQuery={''}
          isMobile={false}
          handleClickLogo={handleClickLogo}
        />
      </ThemeProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    const logo = screen.getByTestId('logo-image');
    expect(logo).toBeDefined();
    await userEvent.click(logo);
    expect(handleClickLogo).toHaveBeenCalled();
    expect(handleUpdateSearchQuery).not.toHaveBeenCalled();
  });

  test('test mobile ', async () => {
    const handleUpdateSearchQuery = vi.fn();
    render(
      <ThemeProvider theme={defaultTheme}>
        <Header handleUpdateSearchQuery={handleUpdateSearchQuery} searchQuery={''} isMobile />
      </ThemeProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    const searchInput = screen.getByTestId('search-input');
    expect(searchInput).toBeDefined();
    const logo = screen.queryByTestId('logo-image');
    expect(logo).toBeNull();
    expect(handleUpdateSearchQuery).not.toHaveBeenCalled();
  });

  test('test settings mobile ', async () => {
    const handleUpdateSearchQuery = vi.fn();
    render(
      <ThemeProvider theme={defaultTheme}>
        <Header handleUpdateSearchQuery={handleUpdateSearchQuery} searchQuery={''} isMobile />
      </ThemeProvider>,
    );

    const header = screen.getByTestId('header');
    expect(header).toBeDefined();
    const settingsButton = screen.getByTestId('settings-button');
    expect(settingsButton).toBeDefined();
    await userEvent.click(settingsButton);
    const settingsContainer = screen.getByTestId('theme-select');
    expect(settingsContainer).toBeDefined();
    expect(handleUpdateSearchQuery).not.toHaveBeenCalled();
  });
});
