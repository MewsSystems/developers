import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';

import { Page } from './Page';

describe('Page', () => {
  it('renders children correctly', () => {
    const testMessage = 'test message';

    render(<Page>{testMessage}</Page>);

    expect(screen.getByText('test message')).toBeInTheDocument();
  });
});
