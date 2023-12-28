import styled from 'styled-components';

const StyledHeader = styled.header`
    display: flex;
    align-items: center;
    justify-content: space-between;
    color: ${(props) => props.theme.colors.primary};
`;

export default StyledHeader;
