import { FC } from 'react';

interface Props {
    value: string;
    onChange: (value: string) => void;
}

const MovieSearchInput: FC<Props> = ({ value, onChange }) => {
    return (
        <div>
            <input
                value={value}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    onChange(e.target.value)
                }
            />
            <br />
            <h3>line loader</h3>
        </div>
    );
};

export default MovieSearchInput;
