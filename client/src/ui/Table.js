import React from 'react';
import styled from 'styled-components';

const TableRow = ({
  items,
  component: Component,
  wrapper: Wrapper,
  className,
}) => (
  <Wrapper>
    {items.map ((item, index) => (
      <Component key={index} className={className}>
        {item}
      </Component>
    ))}
  </Wrapper>
);

const Table = ({rows}) => (
  <TableWrapper>
    <TableRow wrapper={RowHeader} component={HeaderCell} items={rows[0]} />
    <Body>
      {rows
        .splice (1)
        .map ((row, index) => (
          <TableRow
            className={'highlight'}
            wrapper={RowBody}
            component={Cell}
            items={row}
            key={index}
          />
        ))}
    </Body>
  </TableWrapper>
);

const TableWrapper = styled ('div')`
	width: 100%;
	border: 1px solid #EEEEEE;
	border-collapse: collapse;
  border-radius: 5px;
  overflow: hidden;
`;

const Content = styled ('div')`
  padding: 18px 0;
	flex: 1 1 20%;
  text-align: center;
  transition: all 0.3s ease-out;

  &.highlight:hover {
    box-shadow: 0 14px 28px rgba(0,0,0,0.25), 0 10px 10px rgba(0,0,0,0.22);
    background: white;
    border-radius: 5px;
    cursor: pointer;
    transform: scale(1.1);
  }
`;

const Cell = ({children, className}) => (
  <Content className={className}>{children || '...'}</Content>
);

const HeaderCell = styled (Cell)`
	text-transform: uppercase;
	color: white;
`;

const Row = styled ('div')`
	display: flex;
	width: 100%;
`;

const RowHeader = styled (Row)`
	background: #000;
`;
const RowBody = styled (Row)`
	&:nth-of-type(odd) {
		background: #EEEEEE;
	}
`;

const Body = styled ('div')``;

export {Cell, Table};
