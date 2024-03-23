import { describe, expect, it } from 'vitest';

import { App } from './App';
import { customRender } from './test/utils/customRender';

describe('App', () => {
  it('renders', () => {
    const { container } = customRender(<App />);
    expect(container).toBeInTheDocument();
  });
});
