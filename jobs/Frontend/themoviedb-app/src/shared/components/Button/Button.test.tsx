import { screen } from '@testing-library/react';
import { expect, vi } from 'vitest';
import { render } from '../../../tests/utils';
import Button from './Button';

describe('Button', () => {
    it('should render Button', () => {
        render(<Button variant="primary">Click me</Button>);
        const btn = screen.getByRole('button', { name: 'Click me' });
        expect(btn).toBeInTheDocument();
    });

    it('should execute onClick fn if provided', () => {
        const onClickFn = vi.fn();
        render(
            <Button variant="primary" onClick={onClickFn}>
                Click me
            </Button>
        );
        screen.getByRole('button', { name: 'Click me' }).click();
        expect(onClickFn).toHaveBeenCalled();
    });
});
