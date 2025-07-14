import { render, screen } from '@testing-library/react';
import { ChipList } from './ChipList';

describe('ChipList', () => {
  it('renders nothing if items is empty', () => {
    const { container } = render(<ChipList items={[]} />);

    expect(container).toBeEmptyDOMElement();
  });

  it('renders chips for each item', () => {
    render(<ChipList items={['Drama', 'Comedy', 'Action']} />);

    expect(screen.getByText('Drama')).toBeInTheDocument();
    expect(screen.getByText('Comedy')).toBeInTheDocument();
    expect(screen.getByText('Action')).toBeInTheDocument();

    expect(screen.getAllByRole('listitem')).toHaveLength(3);
  });

  it('renders title if provided', () => {
    render(<ChipList title="Genres" items={['Drama']} />);

    expect(screen.getByRole('heading', { name: /genres/i })).toBeInTheDocument();
  });

  it('applies direction row by default', () => {
    render(<ChipList items={['One']} />);

    const ul = screen.getByRole('list');

    expect(ul).toHaveClass('flex');

    expect(ul).not.toHaveClass('flex-col');
  });

  it('applies direction col', () => {
    render(<ChipList items={['One', 'Two']} direction="col" />);

    const ul = screen.getByRole('list');

    expect(ul).toHaveClass('flex-col');
  });

  it('applies custom bg and text color', () => {
    render(<ChipList items={['Green']} bgColor="bg-green-200" textColor="text-green-800" />);

    const chip = screen.getByText('Green');

    expect(chip).toHaveClass('bg-green-200');
    expect(chip).toHaveClass('text-green-800');
  });

  it('applies custom className to wrapper', () => {
    render(<ChipList items={['One']} className="my-custom-class" />);

    expect(screen.getByRole('list').parentElement).toHaveClass('my-custom-class');
  });

  it('renders each chip with correct classes', () => {
    render(<ChipList items={['Alpha']} />);

    const chip = screen.getByText('Alpha');

    expect(chip).toHaveClass('rounded-full');
    expect(chip).toHaveClass('px-3');
    expect(chip).toHaveClass('py-1');
    expect(chip).toHaveClass('text-sm');
  });

  it('renders title as <h4> with correct class', () => {
    render(<ChipList title="My Chips" items={['Beta']} />);

    const heading = screen.getByRole('heading', { name: 'My Chips' });

    expect(heading.tagName).toBe('H4');
  });
});
