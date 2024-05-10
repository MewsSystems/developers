import '@testing-library/jest-dom';
import { render, screen, fireEvent } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { DebouncedSearchInput } from '.';


describe('DebouncedSearchForm', () => {
    const user = userEvent.setup({
        delay: null,
    });

    // Fake timers using Jest
    beforeEach(() => {
        jest.useFakeTimers();
    });

    // Running all pending timers and switching to real timers using Jest
    afterEach(() => {
        jest.runOnlyPendingTimers();
        jest.useRealTimers();
    });

    test('Renders the component', () => {
        const mockOnSearchChange = jest.fn();
        render(
            <DebouncedSearchInput onSearchChange={mockOnSearchChange} />,
        );

        expect(screen.getByRole('searchbox')).toBeInTheDocument();;
    });

    test('Renders the default placeholder', () => {
        const mockOnSearchChange = jest.fn();
        render(
            <DebouncedSearchInput onSearchChange={mockOnSearchChange} />,
        );

        const placeholder = screen.getByPlaceholderText('Search...');

        expect(placeholder).toBeInTheDocument();
    });

    test('Renders the custom placeholder', () => {
        const mockOnSearchChange = jest.fn();
        render(
            <DebouncedSearchInput
                onSearchChange={mockOnSearchChange}
                placeholder="My custom placeholder"
            />,
        );

        const customPlaceholder = screen.getByPlaceholderText(
            'My custom placeholder',
        );

        expect(screen.queryByPlaceholderText('Search...')).not.toBeInTheDocument();
        expect(customPlaceholder).toBeInTheDocument();
    });

    test('Debounces 300ms before calling the onChange', () => {
        const mockOnSearchChange = jest.fn();
        render(
            <DebouncedSearchInput onSearchChange={mockOnSearchChange} />,
        );

        const input = screen.getByPlaceholderText('Search...');
        fireEvent.change(input, { target: { value: 'star wars' } });

        expect(mockOnSearchChange).not.toHaveBeenCalledWith('star wars');
        jest.advanceTimersByTime(300);
        expect(mockOnSearchChange).toHaveBeenCalledWith('star wars');
    });

    test('Debounces custom time before calling the onChange', () => {
        const mockOnSearchChange = jest.fn();
        render(
            <DebouncedSearchInput
                onSearchChange={mockOnSearchChange}
                delay={600}
            />,
        );

        const input = screen.getByPlaceholderText('Search...');
        fireEvent.change(input, { target: { value: 'harry' } });

        jest.advanceTimersByTime(300);
        expect(mockOnSearchChange).not.toHaveBeenCalled();

        jest.advanceTimersByTime(300);
        expect(mockOnSearchChange).toHaveBeenCalledWith('harry');
    });

    it('should perform a search', async () => {
        const onSearchChange = jest.fn();
        render(<DebouncedSearchInput onSearchChange={onSearchChange} />);
        const el = await screen.findByPlaceholderText('Search...');

        await user.type(el, 'star wars');

        jest.advanceTimersByTime(200);

        expect(onSearchChange).not.toHaveBeenCalled();

        jest.advanceTimersByTime(400);

        expect(onSearchChange).toHaveBeenCalledTimes(1);
        expect(onSearchChange).toHaveBeenCalledWith('star wars');
    });
});
