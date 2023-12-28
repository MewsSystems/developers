import { FC } from 'react';
import StyledLoadingBar from './LoadingBar.styles';

export interface LoadingBarProps {
    loading: boolean;
}

const LoadingBar: FC<LoadingBarProps> = ({ loading }) => {
    return <StyledLoadingBar loading={loading} />;
};

export default LoadingBar;
