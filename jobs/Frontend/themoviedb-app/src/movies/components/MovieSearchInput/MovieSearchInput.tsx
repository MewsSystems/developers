import { FC } from 'react';
import Input from '../../../shared/components/Input/Input';
import StyledMovieSearchInput, {
    StyledSearchIcon,
} from './MovieSearchInput.styles';

interface Props {
    value: string;
    onChange: (value: string) => void;
}

const MovieSearchInput: FC<Props> = ({ value, onChange }) => {
    return (
        <StyledMovieSearchInput>
            <Input
                value={value}
                placeholder="Search movies here"
                onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    onChange(e.target.value)
                }
            />
            <StyledSearchIcon />
        </StyledMovieSearchInput>
    );
};

export default MovieSearchInput;
