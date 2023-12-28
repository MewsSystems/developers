import { FC, PropsWithChildren } from 'react';
import StyledMainContent from './MainContent.styles';

const MainContent: FC<PropsWithChildren> = ({ children }) => {
    return <StyledMainContent>{children}</StyledMainContent>;
};

export default MainContent;
