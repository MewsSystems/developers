import { ChevronLeft, ChevronRight } from '@styled-icons/heroicons-solid';
import { FC } from 'react';
import Button from '../Button/Button';
import StyledPagination from './Pagination.styles';

interface Props {
    page: number;
    totalPages: number | undefined;
    onChange: (value: number) => void;
}

const Pagination: FC<Props> = ({ page, totalPages, onChange }) => {
    if (!totalPages) {
        return null;
    }

    return (
        <StyledPagination>
            <Button
                variant="icon"
                disabled={page === 1}
                onClick={() => onChange(page - 1)}>
                <ChevronLeft />
            </Button>
            <p>
                Page {page} of {totalPages}
            </p>
            <Button
                variant="icon"
                disabled={page === totalPages}
                onClick={() => onChange(page + 1)}>
                <ChevronRight />
            </Button>
        </StyledPagination>
    );
};

export default Pagination;
