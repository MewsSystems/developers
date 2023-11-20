import React from 'react';
import { act, render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import { store } from './app/store';
import App from './App';
import userEvent from '@testing-library/user-event';

describe('App integration tests', () => {
  test('renders home page', async () => {
    await act(()=>render(
      <Provider store={store}>
        <App />
      </Provider>
    ));
    expect(await screen.findByText(/mewmovier/i)).toBeVisible()
  });

  test('searching something works', async () => {
    await act(()=>render(
      <Provider store={store}>
        <App />
      </Provider>
    ));

    const input = await screen.findByPlaceholderText(/Search!/i);
    await act(()=>userEvent.type(input, "star wars"));
    const link = await screen.findByText("Star Wars");
    expect(link).toBeVisible();
  });

  test('searching something works', async () => {
    render(
      <Provider store={store}>
        <App />
      </Provider>
    );

    const input = await screen.findByPlaceholderText(/Search!/i);
    await act(()=>userEvent.type(input, "star wars"));
    const link = await screen.findByText("Star Wars");
    expect(link).toBeVisible();
  });

  test('clicking on result takes to /movie with the correct movie', async () => {
    await act(()=>render(
      <Provider store={store}>
        <App />
      </Provider>
    ));

    const input = await screen.findByPlaceholderText(/Search!/i);
    await act(()=>userEvent.type(input, "star wars"));

    const link = await screen.findByText("Star Wars");
    expect(link).toBeVisible();
    await act(()=>userEvent.click(link));

    expect(await screen.findByAltText("poster")).toBeInTheDocument();
    expect(window.location.pathname).toBe('/movie');
    expect(await screen.findAllByText("Star Wars"));
  });

});