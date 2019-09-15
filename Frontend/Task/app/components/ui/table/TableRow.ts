import styled from "styled-components";

export const TableRowHeader = styled.div`
    display: flex;
    flex: 1 1 0;
    width: 100%;
    border-bottom: 1px solid #ddd;
    position: relative;
`

export const TableRow = styled(TableRowHeader)`
    &:hover {
        background-color: #ebebeb;
    }
`