import { FC } from 'react';

interface Props {
    page: number;
    totalPages: number;
    onChange: (value: number) => void;
}

// TODO move to shared folder
const Pagination: FC<Props> = ({ page, totalPages, onChange }) => {
    return (
        <div>
            <button
                type="button"
                disabled={page === 1}
                onClick={() => onChange(page - 1)}>
                {'<'}
            </button>
            <h3>
                Page {page} of {totalPages}
            </h3>
            <button
                type="button"
                disabled={page === totalPages}
                onClick={() => onChange(page + 1)}>
                {'>'}
            </button>
        </div>
    );
};

export default Pagination;
