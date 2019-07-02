import styled from 'styled-components';

export const CurrencyItem = styled.span`
  width: 75%;
  display: inline-block;
  }
`;

export const AppWrapper = styled.article`
  max-width: 75rem;
  margin: 0 auto;
`;

export const Header = styled.header`
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 0 2rem;
  height: 4.5rem;
  background-color: #304ffe;
  font-size: 2.5rem;
  color: #fff;
`;
export const ListContainer = styled.section`
  display: flex;
  flex-wrap: wrap;
`;

export const FilterContainer = styled.div`
  width: calc(100% - 2rem);
  margin: 0 1rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24);
  border-radius: 6px;
  padding: 2rem;
  box-sizing: border-box;
  margin-bottom: 1.5rem;
  @media screen and (min-width: 800px) {
    width: calc(100% / 3 - 2rem);
  }
`;
export const CurrenciesContainer = styled.div`
  width: calc(100% - 2rem);
  margin: 0 1rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24);
  border-radius: 6px;
  padding: 2rem;
  box-sizing: border-box;
  margin-bottom: 1.5rem;
  @media screen and (min-width: 800px) {
    width: calc(200% / 3 - 2rem);
  }
`;

export const FilterWrapper = styled.ul`
  display: inline-block;
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  @media screen and (min-width: 800px) {
    flex-flow: column;
  }
`;

export const FilterItem = styled.li`
  margin: 0 0.25rem 0.75rem;
  width: 6rem;
  padding: 0.5rem 0;
  color: ${props => (props.active ? 'white' : '#000')};
  background-color: ${props => (props.active ? '#0026ca' : '#fff')};
  text-align: center;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24);
  border-radius: 3px;
  &:hover {
    box-shadow: 0 2px 3px rgba(0, 0, 0, 0.12), 0 2px 3px rgba(0, 0, 0, 0.24);
  }
  @media screen and (min-width: 800px) {
    margin: 0 0 0.75rem;
  }
`;

export const FilterReset = styled.div`
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24);
  text-transform: capitalize;
  margin-bottom: 0.75rem;
  padding: 0.5rem 2rem;
  border-radius: 6px;
  background-color: #ef5350;
  color: #fff;
  &:hover {
    box-shadow: 0 2px 3px rgba(0, 0, 0, 0.12), 0 2px 3px rgba(0, 0, 0, 0.24);
    background-color: #b61827;
  }
`;

export const CurrencyHeader = styled.h2`
  margin: 0;
  padding: 0;
  font-size: 1.5rem;
  font-weight: 500;
  text-align: center;
  width: 75%;
  display: inline-block;
  margin-bottom: 2rem;
`;
export const RatesHeader = styled.h2`
  margin: 0;
  padding: 0;
  font-size: 1.5rem;
  font-weight: 500;
  text-align: center;
  width: 25%;
  display: inline-block;
  margin-bottom: 2rem;
`;

export const CurrencyTrend = styled.div`
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 25%;
  color: ${({ direction }) =>
    (direction === 'growing' && '#00c400') ||
    (direction === 'declining' && '#e53935') ||
    (direction === 'stagnating' && '#000')};
  font-size: 1rem;
  line-height: 1rem;
  svg {
    display: inline-block;
    height: 24px;
    width: 24px;
  }
  span {
    margin-right: 0.25rem;
  }
`;

export const SingleCurrency = styled.div`
  box-sizing: border-box;
  font-size: 1rem;
  line-height: 1.5rem;
  margin: 0.25rem 0;
  padding: 0.25rem 0.5rem;
  &:nth-child(even) {
    background-color: #f5f6ff;
    border-radius: 6px;
  }
`;

export const StyledSpinner = styled.div`
  display: block;
  position: relative;
  width: 64px;
  height: 64px;
  margin: 3rem auto;
  div {
    transform-origin: 32px 32px;
    animation: spin 0.8s linear infinite;
  }
  div:after {
    content: ' ';
    display: block;
    position: absolute;
    top: 3px;
    left: 29px;
    width: 3px;
    height: 14px;
    border-radius: 100px 100px 100px 100px;
    background: #cbcbcc;
  }
  div:nth-child(1) {
    transform: rotate(0deg);
    animation-delay: -0.7s;
  }
  div:nth-child(2) {
    transform: rotate(45deg);
    animation-delay: -0.6s;
  }
  div:nth-child(3) {
    transform: rotate(90deg);
    animation-delay: -0.5s;
  }
  div:nth-child(4) {
    transform: rotate(135deg);
    animation-delay: -0.4s;
  }
  div:nth-child(5) {
    transform: rotate(180deg);
    animation-delay: -0.3s;
  }
  div:nth-child(6) {
    transform: rotate(225deg);
    animation-delay: -0.2s;
  }
  div:nth-child(7) {
    transform: rotate(270deg);
    animation-delay: -0.1s;
  }
  div:nth-child(8) {
    transform: rotate(315deg);
    animation-delay: -0s;
  }

  @keyframes spin {
    0% {
      opacity: 1;
    }
    100% {
      opacity: 0;
    }
  }
`;
