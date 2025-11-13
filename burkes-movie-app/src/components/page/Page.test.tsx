import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';

import { PageContainer } from './Page';

describe('Page', () => {
  it('renders children correctly', () => {
    const testMessage = 'test message';

    render(<PageContainer>{testMessage}</PageContainer>);

    expect(screen.getByText('test message')).toBeInTheDocument();
  });
});
