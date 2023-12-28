import { FC, PropsWithChildren } from 'react';
import StyledGlobalContainer from './GlobalContainer.styles';

const GlobalContainer: FC<PropsWithChildren> = ({ children }) => {
    return <StyledGlobalContainer>{children}</StyledGlobalContainer>;
};

export default GlobalContainer;
