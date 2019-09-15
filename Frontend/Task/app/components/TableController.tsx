import React from 'react'
import styled from 'styled-components';
import { TIME_TO_UPDATE } from '@constants/config';

const TableController: React.FC = () => {
    return (
        <TableControllerComponent>
            {`Update every ${TIME_TO_UPDATE / 1000} seconds`}
        </TableControllerComponent>
    )
};

export const TableControllerComponent = styled.div`
    height: 40px;
    padding: 0 15px;
    background-color: #f5f5f5;
    display: flex;
    align-items: center;
    justify-content: flex-end;
    font-size: 13px;
`

TableController.displayName = 'TableController';

export default TableController;