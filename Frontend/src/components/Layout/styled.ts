import { Link } from 'react-router-dom';
import styled from 'styled-components';
import Flex, { FlexItem } from '../common/Flex';

export const PageHeader = styled(Flex).attrs({
  as: 'header',
  flexWrap: 'nowrap',
  center: true,
})`
  padding: ${({ theme }) => theme.space[4]};
  height: ${({ theme }) => theme.space[5]};
  line-height: ${({ theme }) => theme.lineHeights.heading};
  font-size: ${({ theme }) => theme.fontSizes.l};
  background-color: ${({ theme }) => theme.colors.primary};
  color: ${({ theme }) => theme.colors.secondaryLight};
`;

export const PageMain = styled(FlexItem).attrs({
  as: 'main',
  $display: 'flex',
  grow: 1,
})`
  background-color: ${({ theme }) => theme.colors.background};
`;

export const PageFooter = styled(Flex).attrs({
  as: 'footer',
})`
  padding: ${({ theme }) => theme.space[4]};
`;

export const NavLinks = styled.ul`
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
  list-style: none;
  margin: 0;
  padding: 0;

  li + li {
    margin-left: ${({ theme }) => theme.space[4]};
  }

  li:last-child {
    margin-right: auto;
  }
`;

export const NavLink = styled(Link)`
  display: flex;
  align-items: center;
  position: relative;
  color: ${({ theme }) => theme.colors.secondaryLight};
  text-decoration: none;
  text-transform: capitalize;

  &::after {
    content: '';
    display: block;
    position: absolute;
    left: 0;
    bottom: ${({ theme }) => `-${theme.space[1]}`};
    height: ${({ theme }) => theme.space[1]};
    width: 0;
    background: ${({ theme }) => theme.colors.secondaryLight};
    transition: width 150ms linear;
  }

  &:hover::after {
    width: 100%;
  }

  > svg {
    height: 1.6rem;
    padding: ${({ theme }) => theme.space[3]};
  }
`;
