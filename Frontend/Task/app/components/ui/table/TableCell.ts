import styled  from "styled-components";


export const TableCell = styled.div`
    display: flex;
    flex: 1 1 1px;
    align-items: center;
    width: 100%;
    min-height: 22px;
    line-height: 22px;
    padding: 10px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    color: #444;
`

export const TextTableCell = styled(TableCell)`
    display: block;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    padding: 0;
`

export const HeaderTextTableCell = styled(TextTableCell)`
    font-size: 13px;
    font-weight: bold;
`