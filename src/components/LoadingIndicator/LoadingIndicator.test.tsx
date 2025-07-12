import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import { LoadingIndicator } from './LoadingIndicator';

describe('<LoadingIndicator />', () => {
  it('renders correctly with appropriate accessibility attributes and elements', () => {
    render(<LoadingIndicator />);

    const statusElement = screen.getByRole('status');
    expect(statusElement).toBeInTheDocument();
    expect(statusElement).toHaveAttribute('aria-live', 'polite');

    const loadingText = screen.getByText(/loading/i);
    expect(loadingText).toBeInTheDocument();
    expect(loadingText).toHaveClass('sr-only');

    const icon = statusElement.querySelector('[aria-hidden]');
    expect(icon).toBeInTheDocument();
  });
});
