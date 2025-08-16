import { render, screen, fireEvent } from '@testing-library/react';
import SearchBox from './SearchBox';

describe('SearchBox component', () => {
	it('renders search box with provided query', () => {
		const props = {
			query: 'test query',
			onSearch: jest.fn(),
		};

		render(<SearchBox {...props} />);
		const searchBox = screen.getByRole('textbox');

		expect(searchBox).toHaveValue('test query');
	});

	it('trims empty strings', () => {
		const props = {
			query: '',
			onSearch: jest.fn(),
		};

		render(<SearchBox {...props} />);
		const searchBox = screen.getByRole('textbox');
		fireEvent.change(searchBox, { target: { value: '  ' } });

		expect(props.onSearch).not.toHaveBeenCalled();
	});

	it('calls onSearch with input value when input changes', () => {
		const props = {
			query: '',
			onSearch: jest.fn(),
		};

		render(<SearchBox {...props} />);
		const searchBox = screen.getByRole('textbox');
		fireEvent.change(searchBox, { target: { value: 'test' } });

		expect(props.onSearch).toHaveBeenCalledWith('test');
	});

	it('does not call onSearch if input value is empty', () => {
		const props = {
			query: '',
			onSearch: jest.fn(),
		};

		render(<SearchBox {...props} />);
		const searchBox = screen.getByRole('textbox');
		fireEvent.change(searchBox, { target: { value: '' } });

		expect(props.onSearch).not.toHaveBeenCalled();
	});
});
