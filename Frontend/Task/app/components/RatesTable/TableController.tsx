import React from 'react'
import styled from 'styled-components';
import IconSearch from '@icons/search';
import { TIME_TO_UPDATE } from '@constants/config';
import Input from '../ui/Input';

interface TableControllerProps {
    searchTerm: string,
    onSearch: (value: string) => void
}

const TableController: React.FC<TableControllerProps> = ({searchTerm, onSearch}: TableControllerProps) => {
    return (
        <TableControllerComponent>
            <Input
                type="text"
                value={searchTerm || ''}
                placeholder="Search by pair..."
                clearBtn={true}
                icon={IconSearch}
                onChange={onSearch}
                onCancel={onSearch}
                onEnterPressed={onSearch}
            />
            <span>{`Update every ${TIME_TO_UPDATE / 1000} seconds`}</span>
        </TableControllerComponent>
    )
};

export const TableControllerComponent = styled.div`
    height: 40px;
    padding: 0 15px;
    background-color: #f5f5f5;
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-size: 13px;
`

TableController.displayName = 'TableController';

export default TableController;