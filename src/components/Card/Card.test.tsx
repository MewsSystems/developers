import { render, screen } from '@testing-library/react';
import { Card } from './Card';

describe('Card', () => {
  it('renders children', () => {
    render(<Card>hello world</Card>);

    expect(screen.getByText('hello world')).toBeInTheDocument();
  });

  it('does not already feature custom className', () => {
    render(<Card>child</Card>);

    const card = screen.getByText('child');

    expect(card).not.toHaveClass('custom-class');
  });

  it('appends additional className', () => {
    render(<Card className="custom-class">child</Card>);

    const card = screen.getByText('child');

    expect(card).toHaveClass('custom-class');
  });

  it('spreads additional props onto the div', () => {
    render(
      <Card data-testid="my-card" id="special-card">
        data here
      </Card>
    );

    const card = screen.getByTestId('my-card');

    expect(card).toHaveAttribute('id', 'special-card');
  });
});
